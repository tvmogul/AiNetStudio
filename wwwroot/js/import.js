// ✅ addTransactions 
document.getElementById("addTransactions").addEventListener("click", async function () {

    const currentDb = getCurrentDatabase(); // ✅ Get current selected database

    //// ✅ Get the current account button and check its value
    const accountButton = document.getElementById("importAccountDropdownButton");
    const accountName = accountButton ? accountButton.textContent.trim().toLowerCase() : '';
    const accountId = accountButton ? accountButton.getAttribute('data-id') : '';

    // ✅ If "none" is selected, block and show message
    if (accountName === 'none' || accountId === 'NONE') {
        alert("You must create at least one account for the selected company before loading transactions.");
        return; // ✅ Stop further execution
    }


    let _columnMappings = {}; // key: column index, value: one of the _availableFields strings

    // ✅ ADD THIS BLOCK HERE
    $('#transtable thead select').each(function (index) {
        const selected = $(this).val();
        if (selected && selected !== "") {
            _columnMappings[index] = selected;
        }
    });

    // ✅ ADDED: Helper function to convert byte arrays to Base64
    function arrayToBase64(uintArray) {
        if (!uintArray) return null;
        const bytes = new Uint8Array(uintArray);
        return btoa(String.fromCharCode(...bytes));
    }

    const table = document.getElementById("transtable");
    const previousPageLength = table1.page.len();
    table1.page.len(-1).draw(); // ✅ Temporarily show all rows
    const rows = table1.rows({ search: 'applied' }).indexes().toArray().map(i => table1.row(i).node());

    console.log("Detected rows to import:", rows.length);
    console.log("Column Mappings:", _columnMappings);

    if (!rows.length) {
        alert("❌ No rows to import. Please load data first.");
        return;
    }

    // ✅ Fetch Account ID from server
    const selectedAccountID = await fetch('/GlobalState/GetAccount')
        .then(r => r.text())
        .then(text => text.trim());

    // ✅ Also check button text for "none"
    const accountButtonText = $('#accountDropdownButton').text().trim();
    console.log("Account Check:", selectedAccountID, accountButtonText);

    // ✅ STRICT CHECK: If account is "none" (lowercase)
    if (selectedAccountID === "none" || accountButtonText === "none") {
        alert("❌ Must select or create an Account.");
        return;
    }

    const cleaned = selectedAccountID.replace(/-/g, '');
    if (!cleaned || cleaned.length !== 32) {
        alert("❌ Select a valid account.");
        return;
    }

    const overlay = document.createElement("div");
    overlay.id = "importOverlay";
    Object.assign(overlay.style, {
        position: "fixed",
        top: "0",
        left: "0",
        width: "100%",
        height: "100%",
        backgroundColor: "rgba(0, 0, 0, 0.6)",
        color: "#fff",
        fontSize: "2rem",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        zIndex: "9999"
    });
    overlay.textContent = "Importing… please wait";
    document.body.appendChild(overlay);

    let currentAccount = getCurrentAccount();
    const idBytes = uuidToDotNetBytes(currentAccount);

    const batchID = crypto.randomUUID(); // ✅ Added batch ID

    const values = Object.values(_columnMappings);
    const requiredSet1 = ["Date", "Payment", "Deposit"];
    const requiredSet2 = ["Date", "TransType", "Amount"];

    const hasRequiredSet1 = requiredSet1.every(key => values.includes(key));
    const hasRequiredSet2 = requiredSet2.every(key => values.includes(key));

    if (!hasRequiredSet1 && !hasRequiredSet2) {
        alert("Missing required column mappings. You must map either:\n- Date, Payment, Deposit\nOR\n- Date, Amount");
        document.body.removeChild(overlay);
        return;
    }

    const demoTransactions = [];
    const invalidRowIndices = [];

    for (let i = 0; i < rows.length; i++) {
        const row = rows[i];

        const cells = $(row).find("td").map((_, td) => td.textContent.trim()).get();

        console.log("table1 test:", table1, typeof table1.row);

        let rowInvalid = false;

        console.log("Row " + i + ":", row);
        console.log("Row innerHTML:", row.innerHTML);

        try {
            const getMappedValue = (name) => {
                const colIndex = Object.keys(_columnMappings).find(key => _columnMappings[key] === name);
                if (colIndex === undefined) {
                    console.warn(`❌ No mapping found for ${name}`);
                    return null;
                }
                const val = cells[parseInt(colIndex)];
                console.log(`Mapped value for "${name}" in column ${colIndex}:`, val);
                return val;
            };

            const rawDate = getMappedValue("Date");
            let formattedDate = "";
            if (!rawDate || isNaN(Date.parse(rawDate)) || rawDate.toLowerCase() === "date") {
                console.warn(`❌ Invalid or missing date in row ${i}`);
                rowInvalid = true;
            } else {
                const d = new Date(rawDate);
                formattedDate = d.getFullYear() + "-" +
                    String(d.getMonth() + 1).padStart(2, '0') + "-" +
                    String(d.getDate()).padStart(2, '0');
            }

            let parsedAmount = 0;
            let paymentVal = 0;
            let depositVal = 0;

            if (hasRequiredSet1) {
                const paymentRaw = getMappedValue("Payment");
                const depositRaw = getMappedValue("Deposit");

                paymentVal = parseFloat(paymentRaw?.replace(/[^0-9.-]/g, "") || 0);
                depositVal = parseFloat(depositRaw?.replace(/[^0-9.-]/g, "") || 0);

                parsedAmount = depositVal - paymentVal;

                if (paymentVal === 0 && depositVal === 0) {
                    const amountRaw = getMappedValue("Amount")?.replace(/[^0-9.-]/g, "").trim();
                    const transTypeRaw = getMappedValue("TransType")?.trim().toLowerCase();
                    if (amountRaw && !isNaN(amountRaw)) {
                        let baseAmount = parseFloat(amountRaw);
                        switch (transTypeRaw) {
                            case "expense":
                            case "debit":
                                parsedAmount = -Math.abs(baseAmount);
                                paymentVal = -parsedAmount;
                                break;
                            case "income":
                            case "credit":
                                parsedAmount = Math.abs(baseAmount);
                                depositVal = parsedAmount;
                                break;
                            default:
                                parsedAmount = baseAmount;
                                if (parsedAmount > 0) depositVal = parsedAmount;
                                if (parsedAmount < 0) paymentVal = -parsedAmount;
                                break;
                        }
                    }
                }
            } else if (hasRequiredSet2) {
                const amountRaw = getMappedValue("Amount")?.replace(/[^0-9.-]/g, "").trim();
                const transTypeRaw = getMappedValue("TransType")?.trim().toLowerCase();
                if (!amountRaw || isNaN(amountRaw) || amountRaw.toLowerCase() === "amount") {
                    console.warn(`❌ Invalid or missing amount in row ${i}`);
                    rowInvalid = true;
                } else {
                    let baseAmount = parseFloat(amountRaw);
                    switch (transTypeRaw) {
                        case "expense":
                        case "debit":
                            parsedAmount = -Math.abs(baseAmount);
                            paymentVal = -parsedAmount;
                            break;
                        case "income":
                        case "credit":
                            parsedAmount = Math.abs(baseAmount);
                            depositVal = parsedAmount;
                            break;
                        default:
                            parsedAmount = baseAmount;
                            if (parsedAmount > 0) depositVal = parsedAmount;
                            if (parsedAmount < 0) paymentVal = -parsedAmount;
                            break;
                    }
                }
            }

            if (rowInvalid) {
                row.style.backgroundColor = "#f08080";
                invalidRowIndices.push(i + 1);
                continue;
            }

            const t = {
                AccountID: arrayToBase64(idBytes),
                TransactionID: arrayToBase64(uuidToDotNetBytes(crypto.randomUUID())),
                BatchID: arrayToBase64(uuidToDotNetBytes(batchID)),
                RecurringID: null,
                CampaignID: null,
                Date: formattedDate,
                Reference: getMappedValue("CheckNumber"),
                Category: "Uncategorized",
                IsTaxRelated: false,
                Description: getMappedValue("Description"),
                Payee: getMappedValue("Payee") || getMappedValue("Description"),
                Tag: fileNameDisplay.value,
                Memo: getMappedValue("Memo"),
                Amount: parsedAmount,
                Payment: paymentVal,
                Deposit: depositVal,
                Cleared: 0,
                CreatedOn: new Date().toISOString(),
                ModifiedOn: new Date().toISOString(),
                TransType: parsedAmount >= 0 ? "Credit" : "Debit",
                Reconciled: 0,
                ExternalSource: "Batch",
                IsSplit: 0,
                SplitCount: 0,
                Currency: "USD",
                BankTransactionId: getMappedValue("BankTransactionId")
            };

            let mappedType = "Unknown";
            if (hasRequiredSet1) {
                mappedType = parsedAmount > 0 ? "Deposit" : (parsedAmount < 0 ? "Payment" : "Unknown");
            } else if (hasRequiredSet2) {
                const rawType = getMappedValue("TransType")?.toLowerCase();
                switch (rawType) {
                    case "expense":
                    case "debit":
                        mappedType = "Payment";
                        break;
                    case "income":
                    case "credit":
                        mappedType = "Deposit";
                        break;
                    case "":
                    case null:
                        mappedType = "Unknown";
                        break;
                    default:
                        mappedType = "Other";
                        break;
                }
            }

            t.TransType = mappedType === "Payment" ? "Expense" :
                mappedType === "Deposit" ? "Income" :
                    "Neither";

            demoTransactions.push(t);

        } catch (ex) {
            console.error(`❌ Error importing row ${i + 1}:`, ex);
            row.style.backgroundColor = "#f08080";
            invalidRowIndices.push(i + 1);
        }
    }

    if (demoTransactions.length > 0) {

        $.ajax({
            url: "/Transactions/Import",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(demoTransactions),
            success: function (response) {
                alert(`✅ ${demoTransactions.length} transactions imported successfully.`);
                _columnMappings = {}; // Clear mappings
            },
            error: function (xhr, status, error) {
                console.error("❌ Import failed:", error);
                alert("❌ Server failed to import transactions.");
            },
            complete: function () {
                if (overlay && overlay.parentNode === document.body) {
                    document.body.removeChild(overlay);
                }
                table1.page.len(previousPageLength).draw(); // ✅ Restore page length
            }
        });
    } else {
        alert("❌ No valid transactions to import.");
    }

    document.body.removeChild(overlay); // ✅ Remove overlay
    table1.page.len(previousPageLength).draw(); // ✅ Restore page length
});

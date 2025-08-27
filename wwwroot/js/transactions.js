// Converts a canonical GUID string to .NET Guid.ToByteArray() layout (little-endian for first 3 parts)
function dotNetGuidToBytes(guidStr) {
    if (!guidStr || typeof guidStr !== 'string') return null;
    const hex = guidStr.replace(/-/g, '').toLowerCase();
    if (hex.length !== 32) return null;

    const toPairs = h => h.match(/.{1,2}/g).map(b => parseInt(b, 16));

    // parts: a(8), b(4), c(4), d(16) — .NET byte[] is LE for a,b,c and BE for d
    const a = hex.slice(0, 8);
    const b = hex.slice(8, 12);
    const c = hex.slice(12, 16);
    const d = hex.slice(16); // 16 hex => 8 bytes

    const revPairs = h => toPairs(h).reverse();   // reverse by bytes for LE
    const aLE = revPairs(a);
    const bLE = revPairs(b);
    const cLE = revPairs(c);
    const dBE = toPairs(d);

    return new Uint8Array([...aLE, ...bLE, ...cLE, ...dBE]);
}

// Alias some parts of your code expect
function uuidToDotNetBytes(guidStr) {
    return dotNetGuidToBytes(guidStr);
}


// ✅ deleteTransaction
function deleteTransaction(rowData) {
    if (!rowData || !rowData.TransactionID) {
        alert('No row selected.');
        return;
    }

    // Ensure we send byte[16] like the rest of your code
    const idBytes = Array.isArray(rowData.TransactionID)
        ? rowData.TransactionID
        : Array.from(dotNetGuidToBytes(rowData.TransactionID));

    // (Optional but often required by API): include AccountID as byte[]
    const accountBytes = Array.from(uuidToDotNetBytes(getCurrentAccount()));

    $.ajax({
        url: '/Transaction/DeleteTransaction',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            TransactionID: Array.from(idBytes),
            AccountID: accountBytes
        }),
        success: function () {
            const btn = document.getElementById('loadTransactions');
            if (btn) btn.click();
        },
        error: function (xhr, status, error) {
            console.error('❌ Failed to delete transaction:', error, xhr && xhr.responseText);
            alert('Failed to delete transaction.');
        }
    });
}


// ✅ updateTransactions 
$('#updateTransactions').on('click', function () {
    console.log("Update Transaction button clicked");
    const mode = $('input[name="mode"]:checked').val();
    if (mode == "edit") {
        UpdateTransaction();
    }
    else if (mode == "new") {
        qqq();
    }
});

// ✅ UpdateTransaction 
function UpdateTransaction() {

   /* const isChecked = document.getElementById("cbSplit").checked;*/

    let transactionID = null;
    const selectedRowData = table1.row('.selected').data();
    if (selectedRowData) {
        transactionID = selectedRowData.TransactionID;
        console.log("Selected TransactionID:", transactionID);
    } else {
        console.warn("⚠️ No row selected");
        return;
    }

    // ✅ Block update if Category = "Split" AND IsSplit = 1
    if (selectedRowData.Category === "Split" && selectedRowData.IsSplit === 1) {
        alert('❌ To edit a Split transaction, please use the "Split" button in the table header.');
        return; // ✅ Stop execution
    }

    const _accountID = getCurrentAccount(); // returns GUID string
    const accountID = uuidToDotNetBytes(_accountID); // convert to 16-byte array

    const postData = {}; // ✅ Declare first

    console.log("✅ DEBUG AccountID (converted):", accountID);
    console.log("✅ DEBUG AccountID (Array.from):", Array.from(accountID));

    postData.TransactionID = Array.from(transactionID);
    postData.ParentTransactionID = selectedRowData.ParentTransactionID ? Array.from(selectedRowData.ParentTransactionID) : null;
    postData.AccountID = Array.from(accountID); // ✅ Confirmed working format
    postData.BatchID = Array.from(selectedRowData.BatchID);
    postData.RecurringID = selectedRowData.RecurringID ? Array.from(selectedRowData.RecurringID) : null;

    postData.CampaignID = selectedRowData.CampaignID ? Array.from(selectedRowData.CampaignID) : null;

    const date = $('#dtDateTrans').val();
    //const date = selectedRowData.Date;
    if (!date || date.trim() === "") {
        alert("❌ Date is required.");
        return;
    }

    const reference = $('#txtReference').val();
    const payee = $('#txtPayee').val();
    const description = $('#txtDescription').val();
    const memo = $('#txtMemo').val();

    const payment = parseFloat($('#txtPayment').val().replace(/,/g, '')) || 0;
    const deposit = parseFloat($('#txtDeposit').val().replace(/,/g, '')) || 0;

    const category = $('#categoryDropdownButton').text().trim();

    const reconciled = selectedRowData.Reconciled ?? false;

    const result = getTransactionTypeAndAmount(payment, deposit, category);

    postData.Date = date;
    postData.Reference = reference;
    postData.Payee = payee;
    postData.Category = category;
    postData.Description = description;
    postData.TransType = result.TransType;
    postData.Tag = selectedRowData.Tag;
    postData.Memo = memo;
    postData.Amount = result.Amount;
    postData.Payment = payment;
    postData.Deposit = deposit;
    postData.Cleared = selectedRowData.Cleared;
    postData.CreatedOn = selectedRowData.CreatedOn;
    postData.ModifiedOn = new Date().toISOString();
    postData.Reconciled = reconciled;
    postData.ExternalSource = selectedRowData.ExternalSource;
    postData.IsSplit = 0;
    postData.SplitCount = 0; // ✅ Added SplitCount
    postData.Currency = "USD";

    console.log("✅ FINAL POST BODY:", JSON.stringify(postData, null, 2));

    //window.open('', '', 'width=600,height=400').document.body.innerHTML = '<textarea style="width:100%;height:100%;">' + JSON.stringify(postData, null, 2) + '</textarea>';

    $.ajax({
        url: '/Transaction/UpdateTransaction',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(postData),
        success: function (response) {
            console.log('✅ Transaction updated successfully:', response);
            document.getElementById("loadTransactions").click();
        },
        error: function (xhr, status, error) {
            console.error('❌ Failed to update transaction:', error);
        }
    });

}


// ✅ AddTransaction 
function qqq() {
    /* const isChecked = document.getElementById("cbSplit").checked;*/

    const date = $('#dtDateTrans').val();
    if (!date || date.trim() === "") {
        alert("❌ Date is required.");
        return;
    }

    // ✅ Generate new GUIDs
    const transactionID = crypto.randomUUID();
    const transactionIDBytes = uuidToDotNetBytes(transactionID);

    const batchID = crypto.randomUUID();
    const batchIDBytes = uuidToDotNetBytes(batchID);

    const _accountID = getCurrentAccount(); // returns GUID string
    const accountID = uuidToDotNetBytes(_accountID);

    const payment = parseFloat($('#txtPayment').val().replace(/,/g, '')) || 0;
    const deposit = parseFloat($('#txtDeposit').val().replace(/,/g, '')) || 0;
    const category = $('#categoryDropdownButton').text().trim();
    const result = getTransactionTypeAndAmount(payment, deposit, category);

    // ✅ Build JSON payload with numeric arrays for byte[]
    const postData = {
        TransactionID: Array.from(transactionIDBytes),
        ParentTransactionID: null,
        AccountID: Array.from(accountID),
        BatchID: Array.from(batchIDBytes),
        RecurringID: null,
        CampaignID: null,
        Date: $('#dtDateTrans').val(),
        Reference: $('#txtReference').val().trim(),
        Payee: $('#txtPayee').val().trim(),
        Category: category,
        Description: $('#txtDescription').val().trim(),
        TransType: result.TransType,
        Tag: "",
        Memo: $('#txtMemo').val().trim(),
        Amount: result.Amount,
        Payment: payment,
        Deposit: deposit,
        Cleared: 0,
        CreatedOn: new Date().toISOString(),
        ModifiedOn: new Date().toISOString(),
        Reconciled: 0,
        ExternalSource: "",
        IsSplit: 0,
        SplitCount: 0,
        Currency: "USD"
    };

    console.log("✅ FINAL POST BODY:", JSON.stringify(postData, null, 2));

    $.ajax({
        url: '/Transaction/AddTransaction',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(postData),
        success: function (response) {
            console.log('✅ Transaction updated successfully:', response);
            document.getElementById("loadTransactions").click();
        },
        error: function (xhr, status, error) {
            console.error('❌ Failed to update transaction:', error);
        }
    });
}


/**
 * Converts a GUID string to a numeric array (16-byte) in .NET order and adds it to an object.
 * @param {Object} obj - The object to update.
 * @param {string} key - The property name to assign.
 * @param {string} guid - The GUID string (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).
 */
function addGuidAsByteArray(obj, key, guid) {
    if (!guid || typeof guid !== 'string') {
        console.warn(`⚠️ Skipped adding ${key}: Invalid GUID`);
        return;
    }
    const bytes = uuidToDotNetBytes(guid); // ✅ Uses your existing function
    obj[key] = Array.from(bytes);
}

//function qqq() {
//    const postData = {};

//    addGuidAsByteArray(postData, "TransactionID", crypto.randomUUID());
//    addGuidAsByteArray(postData, "AccountID", getCurrentAccount());
//    addGuidAsByteArray(postData, "BatchID", crypto.randomUUID());
//    addGuidAsByteArray(postData, "RecurringID", null); // Will skip gracefully if null
//    addGuidAsByteArray(postData, "CampaignID", null);

//    postData.Date = $('#dtDateTrans').val();
//    postData.Reference = $('#txtReference').val().trim();
//    postData.Payee = $('#txtPayee').val().trim();
//    postData.Category = $('#categoryDropdownButton').text().trim();
//    postData.Description = $('#txtDescription').val().trim();
//    postData.TransType = getTransactionTypeAndAmount(
//        parseFloat($('#txtPayment').val().replace(/,/g, '')) || 0,
//        parseFloat($('#txtDeposit').val().replace(/,/g, '')) || 0,
//        postData.Category
//    ).TransType;
//    postData.Tag = "";
//    postData.Memo = $('#txtMemo').val().trim();
//    postData.Amount = getTransactionTypeAndAmount(
//        parseFloat($('#txtPayment').val().replace(/,/g, '')) || 0,
//        parseFloat($('#txtDeposit').val().replace(/,/g, '')) || 0,
//        postData.Category
//    ).Amount;
//    postData.Payment = parseFloat($('#txtPayment').val().replace(/,/g, '')) || 0;
//    postData.Deposit = parseFloat($('#txtDeposit').val().replace(/,/g, '')) || 0;
//    postData.Cleared = 0;
//    postData.CreatedOn = new Date().toISOString();
//    postData.ModifiedOn = new Date().toISOString();
//    postData.Reconciled = 0;
//    postData.ExternalSource = "";
//    postData.IsSplit = 0;
//    postData.SplitCount = 0;
//    postData.Currency = "USD";

//    console.log("✅ FINAL POST BODY:", JSON.stringify(postData, null, 2));

//    $.ajax({
//        url: '/Transaction/AddTransaction',
//        method: 'POST',
//        contentType: 'application/json',
//        data: JSON.stringify(postData),
//        success: function (response) {
//            console.log('✅ Transaction updated successfully:', response);
//            document.getElementById("loadTransactions").click();
//        },
//        error: function (xhr, status, error) {
//            console.error('❌ Failed to update transaction:', error);
//        }
//    });
//}







// ✅ Event: AI Categories Button Click
$(document).on('click', '.btn-aicategories', function () {
    showTextConfirmModal(
        "AI Tools",
        "AI will analyze the categories below and automatically assign the most appropriate category to any marked as \"Uncategorized\".\n\nDo you want to proceed?",
        () => runAssignCatheories(), // YES
        () => console.log("❌ User canceled") // NO
    );
});

// ✅ runAssignCatheories
function runAssignCatheories() {

    const accountID = getCurrentAccount();
    const dateFrom = $('#dtDateFrom').val();
    const dateTo = $('#dtDateTo').val();

    const requestData = {
        accountID: accountID,
        fromDate: dateFrom,
        toDate: dateTo
    };

    $.ajax({
        url: '/Transaction/RunAssignCategories',
        type: 'POST',
        contentType: 'application/json',
        //data: JSON.stringify(accountID),
        data: JSON.stringify(requestData),
        success: function (response) {
            if (response.success) {
                alert(`✅ Categories updated: ${response.updated}`);
                document.getElementById("loadTransactions").click();
                //location.reload();
            } else {
                alert(`⚠ ${response.message || response.error}`);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
            alert('❌ An unexpected error occurred.');
        }
    });
}

// ✅ Event: AI Audit Risk Button Click
$(document).on('click', '.btn-aiauditrisk', function () {

    showTextConfirmModal(
        "AI Tools",
        "AI will review all transactions and highlight any that may increase the risk of an IRS audit.\n\nDo you want to proceed?",
        () => runAssignAuditRisk(), // YES
        () => console.log("❌ User canceled") // NO
    );
});

// ✅ runAssignAuditRisk
function runAssignAuditRisk() {

    const accountID = getCurrentAccount();

    $.ajax({
        url: '/Transaction/RunAssignAuditRisk',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(accountID),
        success: function (response) {
            if (response.success) {
                alert(`✅ Categories updated: ${response.updated}`);
                document.getElementById("loadTransactions").click();
                //location.reload();
            } else {
                alert(`⚠ ${response.message || response.error}`);
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
            alert('❌ An unexpected error occurred.');
        }
    });
}

// ✅ Delegated Event: Save Splits — post GUIDs as strings (matches server binder)
// ✅ Save Splits: send GUIDs as strings and auto-balance to parent amount
$(document).off('click', '#saveSplitTransaction').on('click', '#saveSplitTransaction', function () {
    if (!window.currentSplitTransaction) {
        alert('No transaction selected.');
        return;
    }

    const parentIdStr = (typeof window.currentSplitTransaction.TransactionID === 'string')
        ? window.currentSplitTransaction.TransactionID
        : byteArrayToGuid(window.currentSplitTransaction.TransactionID);

    const batchIdStr = (typeof window.currentSplitTransaction.BatchID === 'string')
        ? window.currentSplitTransaction.BatchID
        : byteArrayToGuid(window.currentSplitTransaction.BatchID);

    // --- helpers (scoped) ---
    const parseMoney = (val) => {
        const s = String(val ?? '0').replace(/,/g, '').trim();
        const isParen = /^\(.*\)$/.test(s);
        const num = parseFloat(s.replace(/[()]/g, '')) || 0;
        return isParen ? -num : num;
    };
    const round2 = (n) => Math.round(n * 100) / 100;
    const toCents = (n) => Math.round(Number(n) * 100);
    // Convert GUID string to .NET Guid.ToByteArray() layout (LE for first 3 components)
    const guidStringToDotNetBytes = (guidStr) => {
        if (!guidStr || typeof guidStr !== 'string') return [];
        const hex = guidStr.replace(/-/g, '').toLowerCase();
        if (hex.length !== 32) return [];
        const toPairs = h => h.match(/.{1,2}/g).map(b => parseInt(b, 16));
        const rev = h => toPairs(h).reverse();
        const a = rev(hex.slice(0, 8));    // Data1 (4B) LE
        const b = rev(hex.slice(8, 12));   // Data2 (2B) LE
        const c = rev(hex.slice(12, 16));  // Data3 (2B) LE
        const d = toPairs(hex.slice(16));  // Data4 (8B) BE
        return [...a, ...b, ...c, ...d];
    };

    const originalAmount = round2(parseMoney(window.currentSplitTransaction.Amount));
    const originalCents = toCents(originalAmount);

    // --- collect ONLY user-entered child rows (parent/residual row has no .amount-input) ---
    const rows = [];
    let invalid = false;
    const $childRows = $('#splitRowsTable tbody tr').filter(function () {
        return $(this).find('.amount-input').length > 0;
    });

    $childRows.each(function () {
        const $row = $(this);
        const category = $row.find('.category-select').val() || '';
        const description = $row.find('.desc-input').val() || '';
        let amount = parseMoney($row.find('.amount-input').val());

        // match sign to parent
        amount = (originalAmount < 0) ? -Math.abs(amount) : Math.abs(amount);
        amount = round2(amount);

        if (Math.abs(amount) >= 0.005) {
            if (!category) {
                $row.find('.category-select').addClass('is-invalid');
                invalid = true;
                return false; // break
            } else {
                $row.find('.category-select').removeClass('is-invalid');
            }
            rows.push({ Category: category, Description: description, Amount: amount });
        }
    });

    if (invalid) {
        alert('Please select a category for all non-zero split rows.');
        return;
    }

    // === DELETE-ALL PATH ===
    if (rows.length === 0) {
        // Controller expects: [FromBody] TransactionInfo with byte[16] TransactionID
        let txnIdBytes = [];
        const rawId = window.currentSplitTransaction.TransactionID;

        if (Array.isArray(rawId) && rawId.length === 16) {
            txnIdBytes = rawId;
        } else if (rawId && typeof rawId === 'object' && rawId.byteLength === 16) {
            txnIdBytes = Array.from(new Uint8Array(rawId));
        } else {
            txnIdBytes = guidStringToDotNetBytes(parentIdStr);
        }

        $.ajax({
            url: '/Transaction/DeleteSplits',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            processData: false,
            data: JSON.stringify({ TransactionID: txnIdBytes }),
            success: function () {
                if (typeof splitTransactionModal !== 'undefined' && splitTransactionModal) {
                    splitTransactionModal.hide();
                }
                const btn = document.getElementById('loadTransactions');
                if (btn) btn.click();
            },
            error: function (xhr) {
                if (typeof splitTransactionModal !== 'undefined' && splitTransactionModal) {
                    splitTransactionModal.hide();
                }
                alert('Failed to delete splits: ' + ((xhr && (xhr.responseText || xhr.statusText)) || 'Unknown error'));
            }
        });
        return; // 🔚
    }

    // === SAVE PATH (1+ child splits) ===
    // Balance WITHOUT adding an extra row — adjust the last user row by the exact cent diff
    const parentCents = originalCents;
    let childCents = rows.reduce((s, r) => s + toCents(r.Amount || 0), 0);
    let diffCents = parentCents - childCents;

    if (diffCents !== 0) {
        const last = rows[rows.length - 1];
        const lastCents = toCents(last.Amount || 0);
        last.Amount = round2((lastCents + diffCents) / 100);
        // recompute to verify
        childCents = rows.reduce((s, r) => s + toCents(r.Amount || 0), 0);
        diffCents = parentCents - childCents;
    }

    if (diffCents !== 0) {
        alert(`Splits do not sum to parent amount.\nParent: ${(parentCents / 100).toFixed(2)}\nSplits: ${(childCents / 100).toFixed(2)}`);
        return;
    }

    // Save (GUID strings)
    const payloadSave = JSON.stringify({
        ParentTransactionID: parentIdStr,
        BatchID: batchIdStr,
        AccountID: (typeof getCurrentAccount === 'function') ? getCurrentAccount() : '',
        Splits: rows
    });

    $.ajax({
        url: '/Transaction/SaveSplits',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        processData: false,
        data: payloadSave,
        success: function (response) {
            if (response && response.success === false) {
                alert('Failed to save splits: ' + (response.message || 'Unknown error'));
                return;
            }
            if (typeof splitTransactionModal !== 'undefined' && splitTransactionModal) {
                splitTransactionModal.hide();
            }
            const btn = document.getElementById('loadTransactions');
            if (btn) btn.click();
        },
        error: function (xhr) {
            if (typeof splitTransactionModal !== 'undefined' && splitTransactionModal) {
                splitTransactionModal.hide();
            }
            alert('Failed to save splits: ' + ((xhr && (xhr.responseText || xhr.statusText)) || 'Unknown error'));
        }
    });
});


















// ✅ Global Vars
var splitTransactionModal;
var tblSplits;
var currentModalContainer = null;
window.currentSplitTransaction = null;

// ✅ Event: Split Transaction Button Click
$(document).off('click', '.btn-split').on('click', '.btn-split', function () {
    createSplitTransaction();
});

// ✅ createSplitTransaction
function createSplitTransaction() {
    const selectedRowData = table1.row('.selected').data();
    if (!selectedRowData) {
        alert("⚠️ No row selected");
        return;
    }

    window.currentSplitTransaction = selectedRowData;

    $.ajax({
        url: '/Transaction/GetCurrentSplits',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(selectedRowData),
        success: function (response) {
            console.log('✅ Splits received:', response);
            showSplitModal(response);
        },
        error: function (xhr, status, error) {
            console.error('❌ Failed to prepare split', status, error, xhr.responseText);
        }
    });
}

// ✅ showSplitModal
function showSplitModal(data) {
    const modalWrapper = document.getElementById("splitTransactionModalWrapper");
    if (!modalWrapper) {
        alert("❌ Modal template not found in DOM");
        return;
    }

    if (window.currentModalContainer) {
        currentModalContainer.remove();
    }

    currentModalContainer = document.createElement("div");
    currentModalContainer.innerHTML = modalWrapper.innerHTML;
    document.body.appendChild(currentModalContainer);

    const modalElement = currentModalContainer.querySelector("#splitTransactionModal");
    const tableElement = currentModalContainer.querySelector("#splitRowsTable");
    const saveButton = currentModalContainer.querySelector('#saveSplitTransaction');
    saveButton.onclick = () => $('#saveSplitTransaction').trigger('click');

    splitTransactionModal = new bootstrap.Modal(modalElement, {
        backdrop: 'static',
        keyboard: false
    });

    // ✅ Load categories first
    let categoryOptions = [];
    $.ajax({
        url: '/Category/GetUserCompanyCategories',
        type: 'GET',
        success: function (categories) {
            if (!Array.isArray(categories) || categories.length === 0) {
                categoryOptions = [];
            } else {
                categoryOptions = categories.map(category => {
                    return {
                        id: category.categoryID || category.CategoryID,
                        name: category.name || category.Name || "",
                        type: category.categoryType || category.CategoryType || ""
                    };
                });
            }

            // ✅ Always include parent row first
            if (!Array.isArray(data)) {
                data = [];
            }

            // ✅ Normalize data: Convert backend splits to DataTable-friendly format
            const childRows = (data || []).map(item => ({
                TransactionID: item.TransactionID || "",
                Category: item.Category || "",
                Description: item.Description || "",
                Amount: parseFloat(item.Amount) || 0,
                isParent: false
            }));

            // ✅ Add parent row on top
            childRows.unshift({
                TransactionID: window.currentSplitTransaction.TransactionID,
                Category: window.currentSplitTransaction.Category || '',
                Description: window.currentSplitTransaction.Description || '',
                Amount: window.currentSplitTransaction.Amount || 0,
                isParent: true
            });

            initializeSplitTable(childRows, categoryOptions, saveButton);
        },
        error: function () {
            categoryOptions = [];
            initializeSplitTable([], categoryOptions, saveButton);
        }
    });

    function initializeSplitTable(data, categoryOptions, saveButton) {
        tblSplits = $(tableElement).DataTable({
            scrollY: '160px',
            scrollCollapse: true,
            paging: false,
            searching: false,
            info: false,
            autoWidth: false,
            scrollX: false,
            responsive: true,
            destroy: true,
            data: data || [],
            columns: [
                { data: 'TransactionID', visible: false },

                {
                    data: 'Category',
                    defaultContent: '',
                    render: function (data, type, row, meta) {
                        if (row.isParent) {
                            return `<span class="text-muted">${data || '(No Category)'}</span>`;
                        }
                        let selectHtml = `<select class="form-select category-select" data-row="${meta.row}" style="width:150px;">`;
                        selectHtml += `<option value="">Select Category</option>`;
                        categoryOptions.forEach(cat => {
                            const selected = (data === cat.name) ? 'selected' : '';
                            selectHtml += `<option value="${cat.name}" ${selected}>${cat.name} (${cat.type})</option>`;
                        });
                        selectHtml += `</select>`;
                        return selectHtml;
                    }
                },

                {
                    data: 'Description',
                    defaultContent: '',
                    render: function (data, type, row) {
                        if (row.isParent) {
                            return `<span class="text-muted">${data || '(No Description)'}</span>`;
                        }
                        return `<input type="text" class="form-control desc-input" value="${data || ''}"/>`;
                    }
                },

                {
                    data: 'Amount',
                    defaultContent: '0',
                    render: function (value, type, row) {
                        if (row.isParent) {
                            return `<span class="text-muted">${parseFloat(value || 0).toFixed(2)}</span>`;
                        }
                        return `<input type="text" class="form-control amount-input" value="${value !== null && value !== undefined ? parseFloat(value).toFixed(2) : '0.00'}"/>`;
                    }
                },

                {
                    data: null,
                    defaultContent: '',
                    render: function (data, type, row) {
                        return row.isParent
                            ? `<span class="text-muted">--</span>`
                            : `<button type="button" class="btn btn-sm remove-row">❌</button>`;
                    }
                }
            ],
            drawCallback: function () {
                updateSplitTotals(saveButton);
            }
        });

        modalElement.addEventListener('shown.bs.modal', function () {
            tblSplits.columns.adjust().draw();
        });

        splitTransactionModal.show();
    }
}

// ✅ Delegated Event: Add Row
$(document).off('click', '#addSplitRow').on('click', '#addSplitRow', function () {
    if (!tblSplits) return;
    tblSplits.row.add({
        TransactionID: '',
        Category: '',
        Description: '',
        Amount: 0,
        isParent: false
    }).draw();
    updateSplitTotals(currentModalContainer.querySelector('#saveSplitTransaction'));
});

// ✅ Delegated Event: Remove Row
$(document).off('click', '.remove-row').on('click', '.remove-row', function () {
    tblSplits.row($(this).closest('tr')).remove().draw();
    updateSplitTotals(currentModalContainer.querySelector('#saveSplitTransaction'));
});

// ✅ Delegated Event: Update Totals on Amount Change
// Update totals live when a user edits a split amount
$(document).off('input', '.amount-input').on('input', '.amount-input', function () {
    updateSplitTotals(currentModalContainer.querySelector('#saveSplitTransaction'));
});


// ✅ Delegated Event: Category Change updates data
$(document).off('change', '.category-select').on('change', '.category-select', function () {
    const rowIndex = $(this).data('row');
    const categoryName = $(this).val();
    const rowData = tblSplits.row(rowIndex).data();
    rowData.Category = categoryName;
    tblSplits.row(rowIndex).data(rowData).invalidate();
});

// ✅ updateSplitTotals
//function updateSplitTotals(saveButton) {
//    if (!tblSplits) return;

//    let totalSplits = 0;
//    let childCount = 0;

//    tblSplits.rows().every(function () {
//        const rowData = this.data();
//        if (!rowData.isParent) {
//            const amount = parseFloat($(this.node()).find('.amount-input').val()) || rowData.Amount || 0;
//            totalSplits += amount;
//            childCount++;
//        }
//    });

//    const originalAmount = parseFloat(window.currentSplitTransaction?.Amount || 0);
//    const remaining = originalAmount - totalSplits;

//    const totalEl = currentModalContainer.querySelector('#splitTotal');
//    const remainingEl = currentModalContainer.querySelector('#splitRemaining');

//    if (totalEl) totalEl.textContent = totalSplits.toFixed(2);
//    if (remainingEl) {
//        remainingEl.textContent = remaining.toFixed(2);
//        remainingEl.classList.toggle('text-danger', remaining !== 0);
//        remainingEl.classList.toggle('text-success', remaining === 0);
//    }

//    if (saveButton) {
//        if (childCount === 0) {
//            saveButton.disabled = false;
//        } else {
//            saveButton.disabled = remaining !== 0;
//        }
//    }
//}
// ✅ updateSplitTotals — fixed to use absolute parent amount and robust parsing
// ✅ Replaces existing updateSplitTotals
function updateSplitTotals(saveButton) {
    if (!tblSplits) return;

    let childTotal = 0;
    let childCount = 0;

    // Track the parent/default row so we can update its Amount to the residual
    let parentRowApi = null;
    let parentData = null;

    tblSplits.rows().every(function () {
        const data = this.data();

        if (data && data.isParent) {
            parentRowApi = this;
            parentData = { ...data };
            return; // skip parent when summing children
        }

        // Prefer live input value; fallback to row data
        const raw = $(this.node()).find('.amount-input').val();
        const n = parseFloat(String(raw ?? data?.Amount ?? 0).replace(/,/g, ''));
        childTotal += Number.isFinite(n) ? n : 0;
        childCount++;
    });

    // Amount on the originally selected transaction (can be negative)
    const original = parseFloat(window.currentSplitTransaction?.Amount || 0) || 0;

    // Residual goes on the parent/default row so that (parent + children) == original
    let residual = original - childTotal;
    residual = Math.round(residual * 100) / 100;

    // Update the parent/default row's Amount to reflect the residual
    if (parentRowApi && parentData) {
        if ((parseFloat(parentData.Amount) || 0) !== residual) {
            parentData.Amount = residual;
            parentRowApi.data(parentData).invalidate(); // refresh the parent row cell showing Amount
        }
    }

    // For display: remaining after including the auto-balanced parent is always zero
    let remaining = original - (childTotal + residual);
    remaining = Math.round(remaining * 100) / 100;

    // Update the UI counters in the modal
    const totalEl = currentModalContainer?.querySelector('#splitTotal');
    const remainingEl = currentModalContainer?.querySelector('#splitRemaining');

    if (totalEl) totalEl.textContent = (Math.round(childTotal * 100) / 100).toFixed(2);
    if (remainingEl) {
        remainingEl.textContent = remaining.toFixed(2); // will be 0.00 when parent is auto-balanced
        remainingEl.classList.toggle('text-success', remaining === 0);
        remainingEl.classList.toggle('text-danger', remaining !== 0);
    }

    // Enable Apply when balanced; allow saving with no child rows (to clear splits)
    if (saveButton) {
        saveButton.disabled = childCount > 0 ? remaining !== 0 : false;
    }
}




// ✅ Delegated Event: Save Splits
// ✅ Delegated Event: Save Splits (fixed payload types)
// Helper: GUID string ("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx") ➜ .NET Guid.ToByteArray() order (little-endian for first 3 parts)
function guidStringToDotNetBytes(guid) {
    if (!guid || typeof guid !== 'string') return null;
    const hex = guid.replace(/-/g, '').toLowerCase();
    if (hex.length !== 32) return null;

    const toBytes = h => new Uint8Array(h.match(/.{1,2}/g).map(b => parseInt(b, 16)));

    // parts from canonical string: a(8), b(4), c(4), d(16)
    const a = hex.slice(0, 8);
    const b = hex.slice(8, 12);
    const c = hex.slice(12, 16);
    const d = hex.slice(16); // 16 hex => 8 bytes (already big-endian order)

    // little-endian for a,b,c: reverse by bytes (2 hex chars per byte)
    const le = part => {
        const bytes = part.match(/.{1,2}/g);
        bytes.reverse();
        return new Uint8Array(bytes.map(b => parseInt(b, 16)));
    };

    const aLE = le(a);
    const bLE = le(b);
    const cLE = le(c);
    const dBE = toBytes(d);

    return new Uint8Array([...aLE, ...bLE, ...cLE, ...dBE]);
}

// Helper: ensure we have .NET byte[16] for a field that might already be bytes
function ensureDotNetGuidBytes(value) {
    if (!value) return null;
    // If it's already a 16-byte array/typed array, normalize to plain array
    if (Array.isArray(value) && value.length === 16) return Array.from(value);
    if (value instanceof Uint8Array && value.length === 16) return Array.from(value);
    if (typeof value === 'string') {
        const bytes = guidStringToDotNetBytes(value);
        return bytes ? Array.from(bytes) : null;
    }
    return null;
}

// ✅ loadTransactions
document.getElementById("loadTransactions").addEventListener("click", function () {
    const currentDb = getCurrentDatabase();

    // ✅ Get the current account button and check its value
    const accountButton = document.getElementById("transactionAccountDropdownButton");
    const accountName = accountButton ? accountButton.textContent.trim().toLowerCase() : '';
    const accountId = accountButton ? accountButton.getAttribute('data-id') : '';

    // ✅ If "none" is selected, block and show message
    if (accountName === 'none' || accountId === 'NONE') {
        alert("You must create at least one account for the selected company before loading transactions.");
        return; // ✅ Stop further execution
    }

    // ✅ Safe to call getCurrentAccount() now
    const currentAccount = getCurrentAccount();

    var sFrom = document.getElementById("dtDateFrom").value;
    var sTo = document.getElementById("dtDateTo").value;

    $.ajax({
        url: '/Transaction/GetTransactions',
        method: 'GET',
        data: {
            accountID: currentAccount,
            dateFrom: sFrom,
            dateTo: sTo
        },
        success: function (data) {
            console.log("Transactions loaded:", data);
            table1.clear();
            table1.rows.add(data);
            $("table1.dataTable").css("font-size", ".8em");
            table1.columns.adjust().draw();
        },
        error: function (xhr, status, error) {
            console.error("Failed to load transactions:", error);
        }
    });
});





$('#btnGenerateReport').on('click', function () {

    saveCheckedAccountIDsSync();  // Save the selected IDs
    var ids = getSavedAccountIDsSync(); // Retrieve them synchronously
    //alert("Retrieved Account IDs: " + ids.join(", ")); // ✅ Guaranteed to show IDs

    var currentDb = getCurrentDatabase();

    // ✅ Collect selected accounts
    const selectedAccounts = [];
    $('#accountCheckboxList input[type="checkbox"]:checked').each(function () {
        selectedAccounts.push($(this).val());
    });

    if (selectedAccounts.length === 0) {
        alert("Please select at least one account.");
        return;
    }

    const dateRange = getFormattedDateRange();
    const reportName = $('#listBoxReports').val()?.[0] || null;
    const title1 = $('#reportTitle1').val();
    const title2 = $('#reportTitle2').val();
    const dateFrom = $('#dtDateFrom').val();
    const dateTo = $('#dtDateTo').val();
    const isTaxRelatedFlag = document.querySelector('input[name="taxOption"]:checked')?.value === "TaxRelated" ? 1 : 0;
    const showLogoFlag = $('#chkShowLogo').is(':checked') ? 1 : 0;

    // ✅ Get excluded categories CSV from hidden input, normalize spacing
    const excludedCsv = ($('#excludedCategoriesCsv').val() || '')
        .split(',')
        .map(s => s.trim())
        .filter(Boolean)
        .join(',');

    const url = `/Report/${encodeURIComponent(reportName)}` +
        `?title1=${encodeURIComponent(title1)}` +
        `&title2=${encodeURIComponent(title2)}` +
        `&dateRange=${encodeURIComponent(dateRange)}` +
        `&dateFrom=${encodeURIComponent(dateFrom)}` +
        `&dateTo=${encodeURIComponent(dateTo)}` +
        `&isTaxRelatedFlag=${isTaxRelatedFlag}` +
        `&showLogoFlag=${encodeURIComponent(showLogoFlag)}` +
        `&accountIds=${encodeURIComponent(ids.join(','))}` +   // ✅ existing
        `&excludedCategories=${encodeURIComponent(excludedCsv)}`; // ✅ added

    window.location.href = url;
});




//5 records:  '/Report/CategoryExpense?title1=CategoryExpense&title2=CategoryExpense&dateRange=12%2F31%2F2023%20-%2012%2F30%2F2024&dateFrom=2024-01-01&dateTo=2024-12-31&isTaxRelatedFlag=1&showLogoFlag=0&accountIds=GegmMQ8GYkCJoqAXP8qKog%3D%3D'
//100records: '/Report/CategoryExpense?title1=CategoryExpense&title2=CategoryExpense&dateRange=12%2F31%2F2023%20-%2012%2F30%2F2024&dateFrom=2024-01-01&dateTo=2024-12-31&isTaxRelatedFlag=1&showLogoFlag=0&accountIds=oJvprskUTk2gkSxc0IQy4g%3D%3D'


// ✅ GetAccounts
function loadAccountsDropdown(dbFileName) {
    dbFileName = getCurrentDatabase();
    const container = $('#accountCheckboxList');
    container.html('<div><span class="text-muted">Loading accounts...</span></div>');

    $.ajax({
        url: `/Account/GetAccounts?dbFileName=${encodeURIComponent(dbFileName)}`,
        type: 'GET',
        success: function (accounts) {
            container.empty();

            if (!Array.isArray(accounts) || accounts.length === 0) {
                container.html('<div><span class="text-muted">No accounts found</span></div>');
                return;
            }

            accounts.forEach(account => {
                const name = account.name || account.Name;
                const accountID = account.accountID || account.AccountID;

                const checkboxItem = `
                            <label>
                                <input type="checkbox" value="${accountID}" data-name="${name}">
                                ${name}
                            </label>
                        `;
                container.append(checkboxItem);
            });
        },
        error: function () {
            container.html('<div><span class="text-danger">Failed to load accounts</span></div>');
        }
    });
}

// ✅ getSelectedAccountIDs
function getSelectedAccountIDs() {
    const selectedIDs = [];
    $('#accountDropdownMenu input[type="checkbox"]:checked').each(function () {
        selectedIDs.push($(this).val());
    });
    return selectedIDs;
}

// ✅ loadAccountFields
function loadAccountFields(accountID) {
    const dbFileName = getCurrentDatabase(); // get from your app's logic

    $.get(`/Account/GetAccountById?dbFileName=${encodeURIComponent(dbFileName)}&accountID=${encodeURIComponent(accountID)}`, function (accountData) {
        if (accountData) {
            $('#accountName').val(accountData.AccountName || '');
            $('#accountNumber').val(accountData.AccountNumber || '');
            $('#bankID').val(accountData.BankID || '');
            $('#bankName').val(accountData.BankName || '');
            $('#code').val(accountData.Code || '');

            // Set Account Type Dropdown
            if (accountData.Type) {
                $('#typeDropdownButton')
                    .text(accountData.Type)
                    .attr('data-value', accountData.Type);
            }

            // Set Account SubType Dropdown
            if (accountData.SubType) {
                $('#subTypeDropdownButton')
                    .text(accountData.SubType)
                    .attr('data-value', accountData.SubType);
            }

            $('#description').val(accountData.Description || '');

            // ✅ Format opening balance with 2 decimal places
            const formattedBalance = (parseFloat(accountData.OpeningBalance) || 0).toFixed(2);
            $('#openingBalance').val(formattedBalance);

            $('#notes').val(accountData.Notes || '');
        }
    });
}

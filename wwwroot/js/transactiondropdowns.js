// ===================================================================
// ========== TRANSACTION DROPDOWNS FOR COMPANIES AND ACCOUNTS ============
// ===================================================================

function initTransactionDropDowns() {
    const companyMenu = document.getElementById('transactionCompanyDropdownMenu');
    const companyButton = document.getElementById('transactionCompanyDropdownButton');

    // Load companies
    $.ajax({
        url: '/Company/GetCompanies',
        type: 'GET',
        success: function (data) {
            companyMenu.innerHTML = '';

            if (!Array.isArray(data) || data.length === 0) {
                companyMenu.innerHTML = '<li><span class="dropdown-item-text text-muted">No companies found</span></li>';
                companyButton.textContent = 'Select Company';
                return;
            }

            data.forEach((company, index) => {
                const dbName = company.databaseName || company.DatabaseName;

                const a = document.createElement('a');
                a.className = 'dropdown-item';
                a.href = '#';
                a.textContent = dbName;
                a.addEventListener('click', function () {
                    companyButton.textContent = dbName;
                    onTransactionCompanySelected(dbName);
                });

                const li = document.createElement('li');
                li.appendChild(a);
                companyMenu.appendChild(li);

                // Preselect first company
                if (index === 0) {
                    companyButton.textContent = dbName;
                    onTransactionCompanySelected(dbName);
                }
            });
        },
        error: function () {
            companyMenu.innerHTML = '<li><span class="dropdown-item-text text-danger">Failed to load</span></li>';
        }
    });
}

// ==============================================================
// TRANSACTION COMPANY SELECTED
// ==============================================================
function onTransactionCompanySelected(selectedName) {
    const companyBtn = document.getElementById('transactionCompanyDropdownButton');
    companyBtn.textContent = selectedName;

    // Persist the selected database to the server
    $.ajax({
        url: '/GlobalState/SetDatabase',
        method: 'POST',
        data: { databaseName: selectedName },
        success: function () {
            console.log("✅ Database set:", selectedName);
        },
        error: function () {
            console.error("❌ Failed to set selected database.");
        }
    });

    // Load accounts for this company
    loadTransactionAccountsDropdown(selectedName);
}

// ==============================================================
// LOAD TRANSACTION ACCOUNTS DROPDOWN
// ==============================================================
function loadTransactionAccountsDropdown(dbFileName) {
    const accountMenu = document.getElementById('transactionAccountDropdownMenu');
    const accountButton = document.getElementById('transactionAccountDropdownButton');

    accountMenu.innerHTML = '<li><span class="dropdown-item-text text-muted">Loading...</span></li>';
    accountButton.textContent = 'Loading...';
    accountButton.setAttribute('data-id', '');

    $.ajax({
        url: `/Account/GetAccounts?dbFileName=${encodeURIComponent(dbFileName)}`,
        type: 'GET',
        success: function (accounts) {
            accountMenu.innerHTML = '';

            if (!Array.isArray(accounts) || accounts.length === 0) {
                accountMenu.innerHTML = '<li><span class="dropdown-item-text text-muted">No accounts found</span></li>';
                accountButton.textContent = 'none';
                accountButton.setAttribute('data-id', 'NONE');
                return;
            }

            accounts.forEach((account, index) => {
                const name = account.name || account.Name;
                const accountID = account.accountID || account.AccountID;

                const a = document.createElement('a');
                a.className = 'dropdown-item';
                a.href = '#';
                a.textContent = name;
                a.setAttribute('data-id', accountID);
                a.addEventListener('click', function () {
                    accountButton.textContent = name;
                    accountButton.setAttribute('data-id', accountID);
                    $.post('/GlobalState/SetAccount', { accountID: accountID });
                });

                const li = document.createElement('li');
                li.appendChild(a);
                accountMenu.appendChild(li);

                if (index === 0) {
                    accountButton.textContent = name;
                    accountButton.setAttribute('data-id', accountID);
                    $.post('/GlobalState/SetAccount', { accountID: accountID });
                }
            });
        },
        error: function () {
            accountMenu.innerHTML = '<li><span class="dropdown-item-text text-danger">Failed to load accounts</span></li>';
            accountButton.textContent = 'Error';
            accountButton.setAttribute('data-id', 'ERROR');
        }
    });
}

// ===========================================================
// CONFIRM SELECTION (ALLOW "none")
// ===========================================================
function confirmTransactionSelection() {
    const companyButton = document.getElementById('transactionCompanyDropdownButton');
    const accountButton = document.getElementById('transactionAccountDropdownButton');

    const selectedCompany = companyButton ? companyButton.textContent.trim() : '';
    const selectedAccount = accountButton ? accountButton.getAttribute('data-id') : 'none';

    // ✅ Save Company
    if (selectedCompany) {
        $.ajax({
            url: '/GlobalState/SetDatabase',
            type: 'POST',
            data: { databaseName: selectedCompany },
            success: function () {
                console.log("✅ Company saved:", selectedCompany);
            },
            error: function () {
                console.error("❌ Failed to save company.");
            }
        });
    }

    // ✅ Save Account
    if (selectedAccount) {
        $.ajax({
            url: '/GlobalState/SetAccount',
            type: 'POST',
            data: { accountID: selectedAccount },
            success: function () {
                console.log("✅ Account saved:", selectedAccount);
            },
            error: function () {
                console.error("❌ Failed to save account.");
            }
        });
    }

    // ✅ Call your main app logic
    runApp();
}

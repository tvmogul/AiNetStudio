// ===================================================================
// ============== DROPDOWNS FOR COMPANIES AND ACCOUNTS ===============
// ===================================================================

window.currentCompanyAccountModal = null; // ✅ Global JS variable

// ==============================================================
// SHOW COMPANYACCOUNT MODAL
// ==============================================================
function showCompanyAccountModal() {
    const modalHtml = document.getElementById("globalCompanyAccountModalWrapper").innerHTML;
    const container = document.createElement("div");
    container.innerHTML = modalHtml;
    document.body.appendChild(container);

    const modalElement = container.querySelector("#globalCompanyAccountModal");
    window.currentCompanyAccountModal = new bootstrap.Modal(modalElement, {
        backdrop: 'static',
        keyboard: false
    });

    window.currentCompanyAccountModal.show();

    const companyMenu = container.querySelector('#globalCompanyModalDropdownMenu');
    const companyButton = container.querySelector('#globalCompanyModalDropdownButton');

    // Load companies
    $.ajax({
        url: '/Company/GetCompanies',
        type: 'GET',
        success: function (data) {
            companyMenu.innerHTML = '';

            if (!Array.isArray(data) || data.length === 0) {
                companyMenu.innerHTML = '<li><span class="dropdown-item-text text-muted">No companies found</span></li>';
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
                    onGlobalCompanySelected(dbName, container);
                });

                const li = document.createElement('li');
                li.appendChild(a);
                companyMenu.appendChild(li);

                if (index === 0) {
                    companyButton.textContent = dbName;
                    onGlobalCompanySelected(dbName, container);
                }
            });
        },
        error: function () {
            companyMenu.innerHTML = '<li><span class="dropdown-item-text text-danger">Failed to load</span></li>';
        }
    });
}

// ==============================================================
// COMPANY SELECTED
// ==============================================================
function onGlobalCompanySelected(selectedName, container) {
    const companyBtn = container.querySelector('#globalCompanyModalDropdownButton');
    companyBtn.textContent = selectedName;

    // Persist the selected database to the server
    $.ajax({
        url: '/GlobalState/SetDatabase',
        method: 'POST',
        data: { databaseName: selectedName },
        success: function () {
            console.log("Database name set to:", selectedName);
        },
        error: function () {
            console.error("Failed to set selected database.");
        }
    });

    // Load accounts for this company
    loadGlobalAccountsDropdown(selectedName, container);
}

// ==============================================================
// LOAD GLOBAL ACCOUNTS DROPDOWN
// ==============================================================
function loadGlobalAccountsDropdown(dbFileName, container) {
    const accountMenu = container.querySelector('#globalAccountModalDropdownMenu');
    const accountButton = container.querySelector('#globalAccountModalDropdownButton');

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
// CONFIRM SELECTION AND CLOSE MODAL (ALLOW "none")
// ===========================================================
function confirmSelection() {
    const companyButton = document.getElementById('globalCompanyModalDropdownButton');
    const accountButton = document.getElementById('globalAccountModalDropdownButton');

    const selectedCompany = companyButton ? companyButton.textContent.trim() : '';
    const selectedAccount = accountButton ? accountButton.getAttribute('data-id') : 'none';

    // ✅ Save Company to GlobalState
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

    // ✅ Save Account to GlobalState
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

    // ✅ Call your runApp() logic
    runApp();

    // ✅ Close modal
    if (window.currentCompanyAccountModal) {
        window.currentCompanyAccountModal.hide();
    }
}

// ===========================================================
// CANCEL SELECTION AND CLOSE MODAL WITHOUT SAVING
// ===========================================================
function cancelSelection() {
    if (window.currentCompanyAccountModal) {
        window.currentCompanyAccountModal.hide();
    }
}


document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('globalCompanyAccountModal');

    // Ensure Light Theme is applied by default
    modal.classList.add('light-theme');

    // Optional: Reset to Light Theme every time modal is shown
    modal.addEventListener('show.bs.modal', function () {
        modal.classList.remove('dark-theme');
        modal.classList.add('light-theme');
    });
});

// ✅ Function to toggle theme ONLY for the GLOBAL MODAL
function toggleGlobalModalTheme() {
    const modal = document.getElementById('globalCompanyAccountModal');
    if (modal.classList.contains('dark-theme')) {
        modal.classList.remove('dark-theme');
        modal.classList.add('light-theme');
    } else {
        modal.classList.remove('light-theme');
        modal.classList.add('dark-theme');
    }
}





// ===========================================================
// APPLY THEME TO COMPANY & ACCOUNT DROPDOWNS
// ===========================================================
//function applyThemeToDropdowns(buttonIds, menuIds, appTheme) {
//    const lightBtnClass = "btn btn-sm btn-outline-secondary dropdown-toggle fw-bold";
//    const darkBtnClass = "btn btn-sm btn-outline-light dropdown-toggle fw-bold"; // High contrast for dark

//    const lightMenuClass = "forestgreen-scrollbar";
//    const darkMenuClass = "gray-scrollbar";

//    buttonIds.forEach(id => {
//        const button = $("#" + id);
//        if (button.length) {
//            const className = appTheme === "light" ? lightBtnClass : darkBtnClass;
//            button.attr("class", className);
//        }
//    });

//    menuIds.forEach(id => {
//        const menu = $("#" + id);
//        if (menu.length) {
//            menu.removeClass(lightMenuClass + " " + darkMenuClass);
//            menu.addClass(appTheme === "light" ? lightMenuClass : darkMenuClass);
//        }
//    });
//}

// ===========================================================
// AUTO APPLY WHEN THEME CHANGES
// ===========================================================
// Pass IDs for Company and Account dropdowns
//const companyAccountButtonIds = ["CompanyAccountModal_DropdownButton", "CompanyAccountModal_DropdownButton"];
//const companyAccountMenuIds = ["CompanyAccountModal_DropdownMenu", "CompanyAccountModal_DropdownMenu"];

// // ✅ Track the current theme globally
//window.currentTheme = "dark"; // Default, or load from user preference

 //function updateTheme(newTheme) {
 //    window.currentTheme = newTheme;
 //    applyThemeToDropdowns(companyAccountButtonIds, companyAccountMenuIds, newTheme);
 //}

// ===========================================================
// LISTEN FOR THEME TOGGLE EVENT
// ===========================================================
// Example toggle: If you have a theme switch button
//$("#themeToggle").on("click", function () {
//    const newTheme = window.currentTheme === "light" ? "dark" : "light";
//    updateTheme(newTheme);
//    console.log("Theme switched to:", newTheme);
//});

// ✅ Initial apply
//$(document).ready(function () {
//    applyThemeToDropdowns(companyAccountButtonIds, companyAccountMenuIds, window.currentTheme);
//});
// ✅ validateCompanyData
function validateCompanyData(data) {
    const errorsCompany = [];

    // Enforce required fields
    if (!data.companyName || data.companyName.trim() === "") {
        errorsCompany.push("Company Name is required.");
    }

    return errorsCompany;
}

// ✅ validateAccountData
function validateAccountData(data) {
    const errorsAccount = [];

    // Required: Account name must not be blank
    if (!data.accountName || data.accountName.trim() === '') {
        errorsAccount.push("Account name is required.");
    }

    return errorsAccount;
}






//COMPANY
//if (!data.databaseName || data.databaseName.trim() === "") {
//    errorsCompany.push("Database Name is required.");
//}

//if (!data.businessType || data.businessType.trim() === "") {
//    errorsCompany.push("Business Type is required.");
//}

//if (!data.address || data.address.trim() === "") {
//    errorsCompany.push("Address is required.");
//}

//if (!data.city || data.city.trim() === "") {
//    errorsCompany.push("City is required.");
//}

//if (!data.state || data.state.trim() === "") {
//    errorsCompany.push("State is required.");
//} else if (data.state.trim().length !== 2) {
//    errorsCompany.push("State must be a 2-letter code.");
//}

//if (!data.zipCode || data.zipCode.trim() === "") {
//    errorsCompany.push("ZIP Code is required.");
//} else {
//    const zip = String(data.zipCode).replace(/\D/g, "");
//    if (zip.length !== 5 && zip.length !== 9) {
//        errorsCompany.push("ZIP Code appears invalid.");
//    }
//}

//if (!data.phone || data.phone.trim() === "") {
//    errorsCompany.push("Phone number is required.");
//} else {
//    const phone = String(data.phone).replace(/\D/g, "");
//    if (phone.length < 7 || phone.length > 15) {
//        errorsCompany.push("Phone number appears invalid.");
//    }
//}

// Required: Email must be present and valid
//if (!data.email || data.email.trim() === '') {
//    errorsCompany.push("Email address is required.");
//} else {
//    const email = data.email.trim();
//    const atPos = email.indexOf("@");
//    const dotPos = email.lastIndexOf(".");
//    if (atPos < 1 || dotPos <= atPos + 1 || dotPos === email.length - 1) {
//        errorsCompany.push("Email address appears invalid.");
//    }
//}

// Website URL format check
//if (!data.website || data.website.trim() === '') {
//    errorsCompany.push("Website URL is required.");
//} else {
//    const url = data.website.trim();
//    if (!(url.startsWith("http://") || url.startsWith("https://"))) {
//        errorsCompany.push("Website URL must start with http:// or https://");
//    }
//}

// Date format check (YYYY-MM-DD)
//if (!data.dateOfIncorporation || data.dateOfIncorporation.trim() === '') {
//    errorsCompany.push("Date of Incorporation is required.");
//} else {
//    const dateStr = data.dateOfIncorporation.trim();
//    const parsedDate = Date.parse(dateStr);
//    if (isNaN(parsedDate) || !/^\d{4}-\d{2}-\d{2}$/.test(dateStr)) {
//        errorsCompany.push("Date of Incorporation must be a valid date in YYYY-MM-DD format.");
//    }
//}








//ACCOUNT
// Required: Account type must not be blank
//if (!data.accountType || data.accountType.trim() === '') {
//    errorsAccount.push("Account type is required.");
//}

// Required: Account subtype must not be blank
//if (!data.accountSubType || data.accountSubType.trim() === '') {
//    errorsAccount.push("Account subtype is required.");
//}

// Required: Bank ID must be present
//if (!data.bankID || data.bankID.trim() === '') {
//    errorsAccount.push("Bank ID is required. Please select a bank.");
//}

// Required: Account number must not be blank and must follow format
//if (!data.accountNumber || data.accountNumber.trim() === '') {
//    errorsAccount.push("Account number is required.");
//} else if (!/^[a-zA-Z0-9\-]{3,30}$/.test(data.accountNumber.trim())) {
//    errorsAccount.push("Account number should be 3-30 characters, letters/numbers/hyphens only.");
//}

// Required: Description must not be blank and max length
//if (!data.description || data.description.trim() === '') {
//    errorsAccount.push("Description is required.");
//} else if (data.description.trim().length > 100) {
//    errorsAccount.push("Description should not exceed 100 characters.");
//}

// Required: Account notes must not be blank and max length
//if (!data.accountNotes || data.accountNotes.trim() === '') {
//    errorsAccount.push("Account notes are required.");
//} else if (data.accountNotes.trim().length > 250) {
//    errorsAccount.push("Account notes should not exceed 250 characters.");
//}

// Required: Opening balance must be a valid number (allow 0)
//if (data.openingBalance === null || data.openingBalance === undefined || data.openingBalance.toString().trim() === '') {
//    errorsAccount.push("Opening balance is required.");
//} else {
//    const balance = data.openingBalance.toString().trim();
//    if (isNaN(balance)) {
//        errorsAccount.push("Opening balance must be a valid number.");
//    }
//}
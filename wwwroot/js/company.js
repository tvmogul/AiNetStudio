// ✅ cancelCompany
document.getElementById("cancelCompany").addEventListener("click", function () {
    //alert("Cancelled!");

    // ✅ CLEAR ALL previous red borders on all inputs, textareas, buttons
    document.querySelectorAll('.error').forEach(el => el.classList.remove('error'));

    document.getElementById("editCompany").value = "Edit";
    bNewCompany = false;
    bEnable = false;
    EnableDisable();
    const currentDb = getCurrentDatabase(); // ✅ Get current selected database
    getCompanyData(currentDb);
    var sss = "";
});


// ✅ EnableDisable
function EnableDisable() {

    const setDisabled = (id, state) => {
        const el = document.getElementById(id);
        if (el) el.disabled = state;
        else {
            alert(id);
        }
    };

    //setDisabled("companyName", !bNewCompany);
    if (bNewCompany || document.getElementById("companyName").value.trim() === "") {
        // Do not disable if it's a new company or if the field is blank
        setDisabled("companyName", false);
    } else {
        setDisabled("companyName", true);
    }

    setDisabled("address", !bEnable);
    setDisabled("address2", !bEnable);
    setDisabled("city", !bEnable);
    setDisabled("state", !bEnable);
    setDisabled("zipCode", !bEnable);
/*    setDisabled("country", !bEnable);*/
    setDisabled("phone", !bEnable);
    setDisabled("fax", !bEnable);
    setDisabled("email", !bEnable);
    setDisabled("taxIDNumber", !bEnable);
    setDisabled("dateOfIncorporation", !bEnable);

    const btn = document.getElementById("businessTypeDropdownButton");
    btn.disabled = !bEnable;
    btn.classList.toggle("disabled", !bEnable);

    const btn2 = document.getElementById("countryDropdownButton");
    btn2.disabled = !bEnable;
    btn2.classList.toggle("disabled", !bEnable);

    setDisabled("website", !bEnable);
    setDisabled("industry", !bEnable);
    setDisabled("contact", !bEnable);
    // setDisabled("billingAddress", !bEnable);
    // setDisabled("shippingAddress", !bEnable);
    //setDisabled("selected-image-path", !bEnable);
    //logo: $('#selected-image-path').val()
    // setDisabled("paymentTerms", !bEnable);
    // setDisabled("creditLimit", !bEnable);
    // setDisabled("accountManager", !bEnable);
    // setDisabled("companySize", !bEnable);
    setDisabled("companyStatus", !bEnable);
    setDisabled("notes", !bEnable);
}
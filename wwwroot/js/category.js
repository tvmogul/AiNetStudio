
function selectCategoryType(value) {
        // Set hidden input value
        document.getElementById('categoryType').value = value;

    // Update button text
    document.getElementById('categoryTypeDropdownButton').textContent = value;
}
function getSelectedCategoryType() {
    return document.getElementById('categoryType').value;
}

function selectGroupType(value) {
    document.getElementById('groupType').value = value;

    // Update button text
    document.getElementById('groupTypeDropdownButton').textContent = value;
}
function getSelectedGroupType() {
    return document.getElementById('groupType').value;
}






//function SaveCategory() {
//    var _categoryID = "";

//    const checkbox = document.getElementById('taxRelated');
//    const isChecked = checkbox ? checkbox.checked : false;

//    var categoryData = {
//        CategoryID: _categoryID,
//        Name: $('#categoryName').val(),
//        CategoryType: getSelectedCategoryType(),
//        GroupType: getSelectedGroupType(),
//        IsTaxRelated: isChecked,
//        TaxLineMapping: $('#taxLineMapping').val(),
//        IsUser: true  // ✅ FIXED: Changed from integer 1 to boolean true
//    };

//    var _method = bNewCategory ? "CreateNewCategory" : "UpdateCategory";
//    if (!bNewCategory) {
//        _categoryID = selectedCategoryID;
//        categoryData.CategoryID = _categoryID;
//    }

//    $.ajax({
//        url: `/Category/${_method}`,
//        type: 'POST',
//        data: JSON.stringify(categoryData),
//        contentType: 'application/json',
//        success: function (response) {
//            alert("Category saved successfully!");
//            location.reload();
//        },
//        error: function (xhr, status, error) {
//            prompt("Copy", "Error: " + error + "\r\nStatus: " + status + "\r\nResponse: " + xhr.responseText);
//        }
//    });

//    bNewCategory = false;
//    bEnable = false;
//    EnableDisable();
//}



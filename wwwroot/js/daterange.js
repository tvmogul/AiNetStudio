// ✅ formatDate
function formatDate(date) {
    return date.toISOString().split('T')[0]; // yyyy-MM-dd
}

// ✅ setThisYear
function setThisYear() {
    const now = new Date();
    const start = new Date(now.getFullYear(), 0, 1);
    const end = new Date(now.getFullYear(), 11, 31);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setLastYear
function setLastYear() {
    const now = new Date();
    const start = new Date(now.getFullYear() - 1, 0, 1);
    const end = new Date(now.getFullYear() - 1, 11, 31);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisMonth
function setThisMonth() {
    const now = new Date();
    const start = new Date(now.getFullYear(), now.getMonth(), 1);
    const end = new Date(now.getFullYear(), now.getMonth() + 1, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setLastMonth
function setLastMonth() {
    const now = new Date();
    const start = new Date(now.getFullYear(), now.getMonth() - 1, 1);
    const end = new Date(now.getFullYear(), now.getMonth(), 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisJanuary
function setThisJanuary() {
    const now = new Date();
    const year = now.getFullYear();
    const start = new Date(year, 0, 1);
    const end = new Date(year, 1, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisFebruary
function setThisFebruary() {
    const now = new Date();
    const year = now.getFullYear();
    const start = new Date(year, 1, 1);
    const end = new Date(year, 2, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisMarch
function setThisMarch() {
    const year = new Date().getFullYear();
    const start = new Date(year, 2, 1);
    const end = new Date(year, 3, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisApril
function setThisApril() {
    const year = new Date().getFullYear();
    const start = new Date(year, 3, 1);
    const end = new Date(year, 4, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisMay
function setThisMay() {
    const year = new Date().getFullYear();
    const start = new Date(year, 4, 1);
    const end = new Date(year, 5, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisJune
function setThisJune() {
    const year = new Date().getFullYear();
    const start = new Date(year, 5, 1);
    const end = new Date(year, 6, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisJuly
function setThisJuly() {
    const year = new Date().getFullYear();
    const start = new Date(year, 6, 1);
    const end = new Date(year, 7, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisAugust
function setThisAugust() {
    const year = new Date().getFullYear();
    const start = new Date(year, 7, 1);
    const end = new Date(year, 8, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisSeptember
function setThisSeptember() {
    const year = new Date().getFullYear();
    const start = new Date(year, 8, 1);
    const end = new Date(year, 9, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisOctober
function setThisOctober() {
    const year = new Date().getFullYear();
    const start = new Date(year, 9, 1);
    const end = new Date(year, 10, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisNovember
function setThisNovember() {
    const year = new Date().getFullYear();
    const start = new Date(year, 10, 1);
    const end = new Date(year, 11, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}

// ✅ setThisDecember
function setThisDecember() {
    const year = new Date().getFullYear();
    const start = new Date(year, 11, 1);
    const end = new Date(year, 12, 0);
    document.getElementById("dtDateFrom").value = formatDate(start);
    document.getElementById("dtDateTo").value = formatDate(end);
}
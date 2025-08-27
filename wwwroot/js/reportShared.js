$(document).ready(function () {
    $('table[data-report-table="true"]').each(function () {
        $(this).DataTable();
    });
});

function exportToPdf(title1, title, dateRange, tableId) {
    const original = document.getElementById(tableId).cloneNode(true);

    // Build printable HTML content with correct titles and table HTML inserted
    const content =
        `<div id="printHeader" style="text-align: center; margin-bottom: 20px;">
        <h3>${title1}</h3>
        <h1>${title}</h1>
        <p>${dateRange}</p>
    </div>` +
        `<table class="table table-bordered table-striped w-100">${original.innerHTML}</table>`;

    const printWindow = window.open('', '', 'width=900,height=700');
    printWindow.document.write(
        `<html><head><title>${title}</title>` +
        `<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css">` +
        `</head><body>${content}</body></html>`
    );
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}


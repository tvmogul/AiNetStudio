let selectedEmails = [];

// Handle Find Contacts
//$('#btnFindContacts').on('click', function () {
//    if (!$('#permissionCheckbox').is(':checked')) {
//        alert("Please grant permission to scan contacts.");
//        return;
//    }

//    $.ajax({
//        url: '/ViralOffer/GetContacts',
//        type: 'GET',
//        dataType: 'json',
//        success: function (data) {
//            const tbody = $('#contactsTable tbody');
//            tbody.empty();

//            if (Array.isArray(data)) {
//                data.forEach(contact => {
//                    tbody.append(`
//                    <tr>
//                        <td><input type="checkbox" class="contact-checkbox" value="${contact.Email}" data-name="${contact.Name}"></td>
//                        <td>${contact.Name}</td>
//                        <td>${contact.Email}</td>
//                    </tr>
//                `);
//                });
//            }

//            $('#contactsSection').show();
//        },
//        error: function () {
//            alert("Failed to load contacts.");
//        }
//    });



//});



//document.addEventListener('DOMContentLoaded', function () {
//    const btnSend = document.getElementById('btnSendInvites');
//    const tbody = document.querySelector('#contactsTable tbody');

//    function loadContacts() {
//        $.ajax({
//            url: '/ViralOffer/GetContacts?t=' + new Date().getTime(), // cache-buster
//            type: 'GET',
//            dataType: 'json',
//            success: function (data) {
//                console.log("Reloaded & shuffled:", data.map(c => c.Name).join(", "));
//                tbody.innerHTML = "";
//                data.forEach(contact => {
//                    tbody.insertAdjacentHTML('beforeend', `
//                        <tr>
//                            <td><input type="checkbox" class="contact-checkbox" value="${contact.Email}" data-name="${contact.Name}"></td>
//                            <td>${contact.Name}</td>
//                            <td>${contact.Email}</td>
//                        </tr>
//                    `);
//                });
//                btnSend.disabled = true; // Reset send button
//            },
//            error: function () {
//                alert("Failed to load contacts.");
//            }
//        });
//    }

//    // ✅ When clicking the button that opens the modal, reload page before any call
//    document.getElementById('btnOpenContactsModal').addEventListener('click', function () {
//        location.reload(); // ✅ Full page reload happens immediately
//    });

//    // Enable button when 5+ selected
//    document.addEventListener('change', function () {
//        const selected = document.querySelectorAll('.contact-checkbox:checked').length;
//        btnSend.disabled = selected < 5;
//    });

//    // Handle Send Invitations
//    btnSend.addEventListener('click', function () {
//        const userEmail = document.getElementById('userEmail').value.trim();
//        if (!userEmail) {
//            alert('Please enter your email address.');
//            return;
//        }

//        const selectedRows = document.querySelectorAll('.contact-checkbox:checked');
//        if (selectedRows.length < 5) {
//            alert('Please select at least 5 sites.');
//            return;
//        }

//        const message = document.getElementById('inviteMessage').innerText.trim();
//        const subject = encodeURIComponent("AiNetProfit Press Release");

//        selectedRows.forEach(row => {
//            const email = row.value;
//            const body = encodeURIComponent(message + "\n\nSent by: " + userEmail);
//            window.open(`mailto:${email}?subject=${subject}&body=${body}`, '_blank');
//        });
//    });
//});

//document.addEventListener('DOMContentLoaded', function () {
//    const btnSend = document.getElementById('btnSendInvites');
//    const tbody = document.querySelector('#contactsTable tbody');

//    function loadContacts() {
//        $.ajax({
//            url: '/ViralOffer/GetContacts?t=' + new Date().getTime(), // cache-buster
//            type: 'GET',
//            dataType: 'json',
//            success: function (data) {
//                console.log("Reloaded & shuffled:", data.map(c => c.Name).join(", "));
//                tbody.innerHTML = "";
//                data.forEach(contact => {
//                    tbody.insertAdjacentHTML('beforeend', `
//                        <tr>
//                            <td><input type="checkbox" class="contact-checkbox" value="${contact.Email}" data-name="${contact.Name}"></td>
//                            <td>${contact.Name}</td>
//                            <td>${contact.Email}</td>
//                        </tr>
//                    `);
//                });
//                btnSend.disabled = true; // Reset send button
//            },
//            error: function () {
//                alert("Failed to load contacts.");
//            }
//        });
//    }

//    // ✅ Load contacts when page loads (like after clicking Welcome/Index link)
//    loadContacts();

//    // ✅ When clicking modal button, do what the working link does: redirect to Welcome
//    document.getElementById('btnOpenContactsModal').addEventListener('click', function () {
//        window.location.href = '/Welcome/Index';
//    });

//    // Enable button when 5+ selected
//    document.addEventListener('change', function () {
//        const selected = document.querySelectorAll('.contact-checkbox:checked').length;
//        btnSend.disabled = selected < 5;
//    });

//    // Handle Send Invitations
//    btnSend.addEventListener('click', function () {
//        const userEmail = document.getElementById('userEmail').value.trim();
//        if (!userEmail) {
//            alert('Please enter your email address.');
//            return;
//        }

//        const selectedRows = document.querySelectorAll('.contact-checkbox:checked');
//        if (selectedRows.length < 5) {
//            alert('Please select at least 5 sites.');
//            return;
//        }

//        const message = document.getElementById('inviteMessage').innerText.trim();
//        const subject = encodeURIComponent("AiNetProfit Press Release");

//        selectedRows.forEach(row => {
//            const email = row.value;
//            const body = encodeURIComponent(message + "\n\nSent by: " + userEmail);
//            window.open(`mailto:${email}?subject=${subject}&body=${body}`, '_blank');
//        });
//    });
//});



document.addEventListener('DOMContentLoaded', function () {
    const btnSend = document.getElementById('btnSendInvites');
    const tbody = document.querySelector('#contactsTable tbody');

    function loadContacts() {
        $.ajax({
            url: '/ViralOffer/GetContacts?t=' + new Date().getTime(),
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("Reloaded & shuffled:", data.map(c => c.Name).join(", "));
                tbody.innerHTML = "";
                data.forEach(contact => {
                    tbody.insertAdjacentHTML('beforeend', `
                        <tr>
                            <td><input type="checkbox" class="contact-checkbox" value="${contact.Email}" data-name="${contact.Name}"></td>
                            <td>${contact.Name}</td>
                            <td>${contact.Email}</td>
                        </tr>
                    `);
                });
                btnSend.disabled = true;
            },
            error: function () {
                alert("Failed to load contacts.");
            }
        });
    }

    // ✅ If page has ?openModal=true, open modal after full page load
    const params = new URLSearchParams(window.location.search);
    if (params.get('openModal') === 'true') {
        const modalElement = document.getElementById('contactsModal');
        const modal = new bootstrap.Modal(modalElement, {
            backdrop: 'static', // Prevent accidental close
            keyboard: false
        });
        modal.show();
        loadContacts();
        // Remove query param so it doesn't reopen on refresh
        window.history.replaceState({}, document.title, window.location.pathname);
    }

    // ✅ Special Offer button: NO modal logic here, just redirect
    const btnViral = document.getElementById('btnViral');
    if (btnViral) {
        btnViral.addEventListener('click', function (e) {
            e.preventDefault(); // Stop default anchor behavior
            window.location.href = '/Welcome/Index?openModal=true'; // Reload Welcome page
        });
    }

    // Enable button when 5+ selected
    document.addEventListener('change', function () {
        const selected = document.querySelectorAll('.contact-checkbox:checked').length;
        btnSend.disabled = selected < 5;
    });

    // Handle Send Invitations
    btnSend.addEventListener('click', function () {
        const userEmail = document.getElementById('userEmail').value.trim();
        if (!userEmail) {
            alert('Please enter your email address.');
            return;
        }

        const selectedRows = document.querySelectorAll('.contact-checkbox:checked');
        if (selectedRows.length < 5) {
            alert('Please select at least 5 sites.');
            return;
        }

        const message = document.getElementById('inviteMessage').innerText.trim();
        const subject = encodeURIComponent("AiNetProfit Press Release");

        selectedRows.forEach(row => {
            const email = row.value;
            const body = encodeURIComponent(message + "\n\nSent by: " + userEmail);
            window.open(`mailto:${email}?subject=${subject}&body=${body}`, '_blank');
        });
    });
}); document.addEventListener('DOMContentLoaded', function () {
    const btnSend = document.getElementById('btnSendInvites');
    const tbody = document.querySelector('#contactsTable tbody');

    function loadContacts() {
        $.ajax({
            url: '/ViralOffer/GetContacts?t=' + new Date().getTime(),
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                console.log("Reloaded & shuffled:", data.map(c => c.Name).join(", "));
                tbody.innerHTML = "";
                data.forEach(contact => {
                    tbody.insertAdjacentHTML('beforeend', `
                        <tr>
                            <td><input type="checkbox" class="contact-checkbox" value="${contact.Email}" data-name="${contact.Name}"></td>
                            <td>${contact.Name}</td>
                            <td>${contact.Email}</td>
                        </tr>
                    `);
                });
                btnSend.disabled = true;
            },
            error: function () {
                alert("Failed to load contacts.");
            }
        });
    }

    // ✅ If page has ?openModal=true, open modal after load
    const params = new URLSearchParams(window.location.search);
    if (params.get('openModal') === 'true') {
        const modalElement = document.getElementById('contactsModal');
        const modal = new bootstrap.Modal(modalElement, {
            backdrop: 'static',
            keyboard: false
        });
        modal.show();
        loadContacts();
        window.history.replaceState({}, document.title, window.location.pathname);
    }

    // ✅ Click Special Offer button: redirect manually (no modal until reload)
    const btnViral = document.getElementById('btnViral');
    if (btnViral) {
        btnViral.addEventListener('click', function () {
            window.location.href = '/Welcome/Index?openModal=true';
        });
    }

    // Enable button when 5+ selected
    document.addEventListener('change', function () {
        const selected = document.querySelectorAll('.contact-checkbox:checked').length;
        btnSend.disabled = selected < 5;
    });

    // Handle Send Invitations
    btnSend.addEventListener('click', function () {
        const userEmail = document.getElementById('userEmail').value.trim();
        if (!userEmail) {
            alert('Please enter your email address.');
            return;
        }

        const selectedRows = document.querySelectorAll('.contact-checkbox:checked');
        if (selectedRows.length < 5) {
            alert('Please select at least 5 sites.');
            return;
        }

        const message = document.getElementById('inviteMessage').innerText.trim();
        const subject = encodeURIComponent("AiNetProfit Press Release");

        selectedRows.forEach(row => {
            const email = row.value;
            const body = encodeURIComponent(message + "\n\nSent by: " + userEmail);
            window.open(`mailto:${email}?subject=${subject}&body=${body}`, '_blank');
        });
    });
});














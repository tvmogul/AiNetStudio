// ✅ Generic dropdown loader
function loadDropdown(menuId, buttonId, type, setDefault = true, callback = null) {
    const menu = document.getElementById(menuId);
    const button = document.getElementById(buttonId);

    menu.innerHTML = '<li><a class="dropdown-item disabled">Loading...</a></li>';

    console.log(`Fetching dropdown for type: ${type}`);

    fetch(`/MaxPay/GetDropdown?type=${type.toLowerCase()}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            console.log(`Received ${data.length} items for ${type}`);
            if (!Array.isArray(data) || data.length === 0) {
                alert(`No items received for dropdown: ${type}`);
                return;
            }

            menu.innerHTML = '';
            let firstItem = null;

            data.forEach(item => {
                const li = document.createElement('li');
                const a = document.createElement('a');
                const code = item.Code.toUpperCase();
                a.className = 'dropdown-item';
                a.href = '#';
                a.setAttribute('data-value', code);
                a.innerHTML = `<span class="d-inline-block w-25">${code}</span><span class="d-inline-block">${item.Label}</span>`;
                li.appendChild(a);
                menu.appendChild(li);
                if (!firstItem) firstItem = a;
            });

            if (setDefault && firstItem) {
                button.textContent = firstItem.getAttribute('data-value');
                firstItem.classList.add('active');
                if (callback) callback(firstItem.getAttribute('data-value'));
            } else {
                button.textContent = '';
            }
        })
        .catch(err => {
            console.error(`Failed to load dropdown: ${type}`, err);
            alert(`Dropdown failed to load: ${type} (${err.message})`);
        });
}

// ✅ Bootstrap toggler patch — force show/hide after items injected
function setupDropdown(menuId, buttonId) {
    const menu = document.getElementById(menuId);
    const button = document.getElementById(buttonId);
    menu.addEventListener('click', function (e) {
        const target = e.target.closest('a.dropdown-item');
        if (target) {
            e.preventDefault();
            button.textContent = target.getAttribute('data-value');
            this.querySelectorAll('.dropdown-item').forEach(i => i.classList.remove('active'));
            target.classList.add('active');
        }
    });
}

// ✅ mediatypeMenu
document.addEventListener('DOMContentLoaded', function () {
    loadDropdown('mediatypeDropdownMenu', 'mediatypeDropdownButton', 'mediatype');
    setupDropdown('mediatypeDropdownMenu', 'mediatypeDropdownButton');
});

// ✅ regionMenu
document.addEventListener('DOMContentLoaded', function () {
    // ✅ Initialize region button with empty value
    const regionButton = document.getElementById('regionDropdownButton');
    regionButton.textContent = '';
    setupDropdown('regionDropdownMenu', 'regionDropdownButton');
});

// ✅ countryMenu + region trigger
document.addEventListener('DOMContentLoaded', function () {
    loadDropdown('countryDropdownMenu', 'countryDropdownButton', 'country', true, function (countryCode) {
        loadDropdown('regionDropdownMenu', 'regionDropdownButton', countryCode);
    });

    const countryMenu = document.getElementById('countryDropdownMenu');
    const countryButton = document.getElementById('countryDropdownButton');

    countryMenu.addEventListener('click', function (e) {
        const target = e.target.closest('a.dropdown-item');
        if (target) {
            e.preventDefault();
            const value = target.getAttribute('data-value');
            countryButton.textContent = value;
            countryMenu.querySelectorAll('.dropdown-item').forEach(i => i.classList.remove('active'));
            target.classList.add('active');
            loadDropdown('regionDropdownMenu', 'regionDropdownButton', value);
        }
    });
});

document.getElementById('GetStations').addEventListener('click', function () {
    const mediaType = document.getElementById('mediatypeDropdownButton').textContent.trim();
    const country = document.getElementById('countryDropdownButton').textContent.trim();
    const region = document.getElementById('regionDropdownButton').textContent.trim();
    const testFlag = document.getElementById('testStations').checked;
    const rankedFlag = document.getElementById('rankedStations').checked;
    const callSign = document.querySelector('input[placeholder="Call Sign"]').value.trim();
    const pullRatio = parseFloat(document.getElementById('pullratio').value) || 0;

    $.ajax({
        url: '/MaxPay/GetStations',
        method: 'GET',
        data: {
            mediaType,
            country,
            region,
            testFlag,
            rankedFlag,
            callSign,
            pullRatio
        },
        success: function (data) {
            console.log('Stations:', data);

            if (!data) {
                //alert('❌ No data received from server.');
                return;
            }

            if (data.error) {
                //alert(`❌ Server error: ${data.error}\nRows read: ${data.rowsRead}`);
                return;
            }

            if (!Array.isArray(data)) {
                //alert('⚠️ Server response format unexpected.');
                return;
            }

            //alert(`✅ Received ${data.length} stations.`);

            // ✅ Ensure table is initialized
            if (!$.fn.dataTable.isDataTable('#stationsTable')) {
                alert('❌ table1 is not initialized yet');
                return;
            }

            // ✅ Clear and add new rows
            table1.clear();
            data.forEach(function (station) {
                const row = table1.row.add({
                    StationId: station.StationId,
                    Latitude: station.Latitude,
                    Longitude: station.Longitude,
                    Callsign: station.Callsign,
                    MediaType: station.MediaType,
                    Aff1: station.Aff1,
                    City: station.City,
                    Region: station.Region,
                    MinMGross: station.MinMGross,
                    MaxMGross: station.MaxMGross,
                    MaxPay: station.MaxPay,
                    RankBuys: station.RankBuys,
                    RankOrders: station.RankOrders
                }).draw(false).node();

                // Attach full station object to the row (serialized)
                $(row).data('station', station);
            });

            //table1.draw();
        },
        error: function (xhr, status, error) {
            console.error('Error fetching stations:', error);
            alert('Failed to retrieve stations.');
        }
    });
});

function loadDemographics() {
    const table = $('#stationsTable').DataTable();
    const selectedRow = table.row('.selected');

    if (!selectedRow.any()) {
        alert("Please select a row first.");
        return;
    }

    const data = selectedRow.data();
    const latitude = data.Latitude || data.latitude;
    const longitude = data.Longitude || data.longitude;

    if (latitude === undefined || longitude === undefined) {
        alert("Latitude or Longitude is missing.");
        return;
    }

    console.log("Selected Latitude:", latitude, "Longitude:", longitude);

    // ✅ AJAX call to MaxPay/GetRaceData
    $.ajax({
        url: `/MaxPay/GetRaceData`,
        method: 'GET',
        data: { latitude: latitude, longitude: longitude },
        success: function (response) {
            console.log("Race Data:", response);

            // ✅ Populate your UI
            $('#demographicsBlack').text(response.BlackPercent ? response.BlackPercent + '%' : '—');
            $('#demographicsWhite').text(response.WhitePercent ? response.WhitePercent + '%' : '—');
            $('#demographicsAsian').text(response.AsianPercent ? response.AsianPercent + '%' : '—');
            $('#demographicsIncome').text(response.MedianIncome ? `$${Number(response.MedianIncome).toLocaleString()}` : '—');
        },
        error: function (xhr, status, error) {
            console.error("Error fetching demographics:", error);
            alert("Error retrieving demographic data.");
        }
    });
}





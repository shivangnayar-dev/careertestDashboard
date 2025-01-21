/*
Template Name: Auxx - Admin & Dashboard Template
Author: Themesdesign
Version: 1.0.0
Website: https://themesdesign.in/
Contact: Themesdesign@gmail.com
File: dashboard ecommerce init Js File
*/

$(document).ready(function () {
    // rgb to hex convert
    function rgbToHex(rgb) {
        // Extract RGB values using regular expressions
        const rgbValues = rgb.match(/\d+/g);

        if (rgbValues.length === 3) {
            var [r, g, b] = rgbValues.map(Number);
        }
        // Ensure the values are within the valid range (0-255)
        r = Math.max(0, Math.min(255, r));
        g = Math.max(0, Math.min(255, g));
        b = Math.max(0, Math.min(255, b));

        // Convert each component to its hexadecimal representation
        const rHex = r.toString(16).padStart(2, '0');
        const gHex = g.toString(16).padStart(2, '0');
        const bHex = b.toString(16).padStart(2, '0');

        // Combine the hexadecimal values with the "#" prefix
        const hexColor = `#${rHex}${gHex}${bHex}`;

        return hexColor.toUpperCase(); // Convert to uppercase for consistency
    }

    // common function to get charts colors from class
    function getChartColorsArray(chartId) {
        const chartElement = document.getElementById(chartId);
        if (chartElement) {
            const colors = chartElement.dataset.chartColors;
            if (colors) {
                const parsedColors = JSON.parse(colors);
                const mappedColors = parsedColors.map((value) => {
                    const newValue = value.replace(/\s/g, "");
                    if (!newValue.includes("#")) {
                        const element = document.querySelector(newValue);
                        if (element) {
                            const styles = window.getComputedStyle(element);
                            const backgroundColor = styles.backgroundColor;
                            return backgroundColor || newValue;
                        } else {
                            const divElement = document.createElement('div');
                            divElement.className = newValue;
                            document.body.appendChild(divElement);

                            const styles = window.getComputedStyle(divElement);
                            const backgroundColor = styles.backgroundColor.includes("#") ? styles.backgroundColor : rgbToHex(styles.backgroundColor);
                            return backgroundColor || newValue;
                        }
                    } else {
                        return newValue;
                    }
                });
                return mappedColors;
            } else {
                console.warn(`chart-colors attribute not found on: ${chartId}`);
            }
        }
    }

    //Sales Analytic
    function initializeTestPerdayChart() {
        $.ajax({
            url: '/Dashboard/GetTestPerdayMonthwiseData', // Controller route
            method: 'GET',
            success: function (response) {
                const categories = [];
                const newuserData = [];
                const registerData = [];
                const testData = [];
                const completedData = [];

                // Parse the response to prepare data for the chart
                response.forEach(item => {
                    categories.push(item.day); // Days as categories
                    newuserData.push(item.newuser);
                    registerData.push(item.register);
                    testData.push(item.test);
                    completedData.push(item.completed);
                });

                // Define the chart options
                var options = {
                    series: [
                        {
                            name: 'New Users',
                            data: newuserData
                        },
                        {
                            name: 'Registrations',
                            data: registerData
                        },
                        {
                            name: 'Tests Taken',
                            data: testData
                        },
                        {
                            name: 'Completed Tests',
                            data: completedData
                        }
                    ],
                    chart: {
                        type: 'line',
                        height: 350,
                        toolbar: {
                            show: false
                        }
                    },
                    xaxis: {
                        categories: categories // Days as x-axis
                    },
                    colors: getChartColorsArray('salesAnalytic'), // Use colors defined in AUXX
                    stroke: {
                        curve: 'smooth',
                        width: 2
                    },
                    grid: {
                        show: true
                    },
                    legend: {
                        position: 'bottom'
                    }
                };

                // Render the chart
                var chart = new ApexCharts(document.querySelector("#salesAnalytic"), options);
                chart.render();
            },
            error: function (xhr, status, error) {
                console.error("Error fetching sales analytics:", error);
            }
        });
    }
    // Initialize the chart
    initializeTestPerdayChart();


    // Function to fetch data and initialize the chart
    function initializeTestStageChart() {
        $.ajax({
            url: '/Dashboard/GetTestStageData', // The route to the controller method
            method: 'GET',
            success: function (response) {
                // Define options for the ApexChart dynamically
                var options = {
                    series: response.map(item => item.series),
                    labels: ['Register', 'Test', 'Completed'],
                    chart: {
                        height: 200,
                        type: 'donut'
                    },
                    plotOptions: {
                        pie: {
                            startAngle: -90,
                            endAngle: 90,
                            offsetY: 5,
                            donut: {
                                size: '80%'
                            }
                        }
                    },
                    dataLabels: {
                        enabled: false,
                    },
                    grid: {
                        padding: {
                            bottom: -80
                        }
                    },
                    colors: getChartColorsArray("clientSatisfaction"),
                    legend: {
                        position: 'bottom'
                    }
                };

                // Initialize ApexChart
                var chart = new ApexCharts(
                    document.querySelector("#clientSatisfaction"), // Target div
                    options
                );

                // Render the chart
                chart.render();
            },
            error: function (xhr, status, error) {
            }
        });
    }
    // Call the function to initialize the chart
    initializeTestStageChart();

    //Client Test attempts Chart
    function initializeTestAttemptsChart() {
        $.ajax({
            url: '/Dashboard/GetTestAttemptsData', // The route to the controller method
            method: 'GET',
            success: function (response) {

                // Define options for the ApexChart dynamically
                var options = {
                    series: response.map(item => item.series),
                    labels: ['1', '2+'],
                    chart: {
                        height: 200,
                        type: 'donut'
                    },
                    plotOptions: {
                        pie: {
                            startAngle: -90,
                            endAngle: 90,
                            offsetY: 5,
                            donut: {
                                size: '80%'
                            }
                        }
                    },
                    dataLabels: {
                        enabled: false,
                    },
                    grid: {
                        padding: {
                            bottom: -80
                        }
                    },
                    colors: getChartColorsArray("storeStatusChart"),
                    legend: {
                        position: 'bottom'
                    }
                };
                // Initialize ApexChart
                var chart = new ApexCharts(
                    document.querySelector("#storeStatusChart"), // Target div
                    options
                );
                // Render the chart
                chart.render();
            },
            error: function (xhr, status, error) {
            }
        });
    }
    // Call the function to initialize the chart
    initializeTestAttemptsChart();


    // Test Summary
    // Function to load and render table data
    function loadTableData() {
        $.ajax({
            url: '/Dashboard/GetTestSummarytData', // API route
            method: 'GET',
            success: function (response) {
                if (response.success) {
                    //      const tableBody = $('#data-table tbody'); // Target table body
                    //    tableBody.empty(); // Clear previous data
                    $('#datadetail-table').hide();
                    $('#exportButton').hide();
                    $('#showAllButton').hide();
                    $('#testdetailhead').hide();
                    $('#columndropdownid').hide();

                    const mainTableBody = $('#data-table tbody');
                    const popupTableBody = $('#popup-table tbody');

                    mainTableBody.empty(); // Clear main table
                    popupTableBody.empty(); // Clear popup table

                    // Separate first 5 records for main table
                    const mainRecords = response.data.slice(0, 5);
                    const allRecords = response.data; // All records for the popup

                    // alert(mainRecords.data.count());
                    //if (mainRecords.data.rows > 0) {
                    $('#showAllButton').show();
                    // Populate main table
                    mainRecords.forEach(item => {
                        mainTableBody.append(`
                    <tr>
                        <td class="clickable-testcode" data-testcode="${item.testcode}">${item.testcode}</td>
                        <td>${item.total_current_month_count}</td>
                        <td>${item.today_count}</td>
                        <td>${item.test_expire}</td>
                    </tr>`);
                    });

                    // Populate popup table with all records
                    allRecords.forEach(item => {
                        popupTableBody.append(`
                    <tr>
                        <td class="clickable-testcode" data-testcode="${item.testcode}">${item.testcode}</td>
                        <td>${item.total_current_month_count}</td>
                        <td>${item.today_count}</td>
                        <td>${item.test_expire}</td>
                    </tr>`);
                    });
                    //      }
                    // Handle "Show All" button click
                    $('#showAllButton').click(function () {
                        $('#popup-modal').modal('show');
                    });

                    // Handle Testcode click in popup table
                    $('#popup-table').on('click', '.clickable-testcode', function () {
                        const testcode = $(this).data('testcode');
                        loadDetailsForTestcode(testcode);

                        // Close the modal
                        const modal = bootstrap.Modal.getInstance(document.getElementById('popup-modal')); // Get modal instance
                        modal.hide();
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching table data:', error);
            }
        });
    }
    // Call the function to load table data
    loadTableData();

    function loadDetailsForTestcode(testcode) {
        // Fetch additional details
        $.ajax({
            url: `/Dashboard/GetTestSummarytDetailData?testcode=${testcode}`, // API endpoint to fetch details
            method: 'GET',
            success: function (response) {
                $('#datadetail-table').show();
                $('#testdetailhead').show();

                const tableBody = $('#datadetail-table tbody'); // Target table body
                tableBody.empty(); // Clear previous data

                if (Array.isArray(response.details) && response.details.length > 0) {
                    $('#exportButton').show();
                    $('#columndropdownid').show();

                    response.details.forEach(item => {
                        // Construct the row HTML
                        const rowHtml = `
                            <tr>
                                <td>${item.candidateid || 'N/A'}</td>
                                <td>${item.testcode || 'N/A'}</td>
                                <td>${item.starttime || 'N/A'}</td>
                                <td>${item.endtime || 'N/A'}</td>
                                <td>${item.name || 'N/A'}</td>
                                <td>${item.mobileno || 'N/A'}</td>
                                <td>${item.emailaddress || 'N/A'}</td>
                                <td>
                                    ${(item.reporturl && item.reporturl.trim() !== '') ?
                                                    `<a href="${item.reporturl}" target="_blank">View Report</a>` : 'No Report'}
                                </td>
                                <td>
                                    ${(item.mobileno && item.mobileno != 0) ?
                                                    `<a href="https://wa.me/${item.mobileno.replace(/\s+/g, '')}" target="_blank">Chat</a>` : 'N/A'}
                                </td>
                            </tr>`;

                        // Append the row
                        tableBody.append(rowHtml);
                    });
                }


                // Store full data for export
                $('#exportButton').data('exportData', response);

                // Initialize DataTable if not already initialized
                let dataTable = $('#datadetail-table').DataTable({
                    destroy: true, // Ensure the table is reinitialized
                    columnDefs: [
                        { targets: 8, orderable: false }, // Disable sorting for the WhatsApp Chat column
                    ],
                });

                // Attach column visibility toggle logic to dropdown checkboxes
                $('input.toggle-vis').off('change').on('change', function (e) {
                    e.preventDefault();
                    const column = dataTable.column($(this).attr('data-column'));
                    column.visible(!column.visible());
                });

            },
            error: function () {
                alert("Eror");
                //$('#detailsSection .card-body').html('<p class="text-danger">An error occurred while fetching details.</p>');
            }
        });
    }

    //$('#data-table tbody').on('click', 'tr', function () {
    //    const testcode = $(this).data('testcode');
    //    loadDetailsForTestcode(testcode);
    //});

    // Event delegation for dynamically added rows
    $('#data-table').on('click', '.clickable-testcode', function () {
        const testcode = $(this).data('testcode');
        loadDetailsForTestcode(testcode);
    });


    //Export to excel

    document.getElementById('exportButton').addEventListener('click', function () {
        //// Get the table element
        const exportData = $('#exportButton').data('exportData');

        //const headers = [
        //    'Candidate Id', 'Testcode', 'Start Datetime', 'End Datetime',
        //    'Name', 'Phone', 'Email', 'Link to report', 'Organization', 'Gender', 'BenchMarkOrganisation', 'top5Motivations1', 'top5Motivations2', 'top5Motivations3',
        //    'Age', 'Location', 'Country', 'Qualification', 'selectedspecializations1', 'selectedspecializations2', 'selectedspecializations3', 'selectedindustries1',
        //    'selectedindustries2', 'selectedindustries3', 'suggestedjobrole1', 'suggestedjobrole2', 'suggestedjobrole3', 'mathStats', 'science', 'Govjobs', 'armedforcesfobs', 'corestream'
        //];
        const headers = Object.keys(exportData.details[0]);

        const rows = [headers.filter(header => header !== 'cgstagid')]; // Remove 'CGStagId' from headers

        if (Array.isArray(exportData.details) && exportData.details.length > 0) {
            exportData.details.forEach(item => {
                const row = headers
                    .filter(header => header !== 'cgstagid') // Exclude 'CGStagId' here as well
                    .map(header => item[header] || ''); // Fetch value for each remaining header
                rows.push(row);
            });
        } else {
            console.error('exportData is not valid:', exportData);
            alert('No data available to export.');
            return; // Stop processing
        }

        try {
            // Export to Excel
            const ws = XLSX.utils.aoa_to_sheet(rows);
            const wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, 'DataTable');
            XLSX.writeFile(wb, 'datatable_export.xlsx');
            alert('Export successful!');
        } catch (error) {
            console.error('Error exporting data:', error);
            alert('An error occurred during export.');
        }
    });



    //end of export

    // start of block data
    // Function to fetch and display counts
    function updateTestUserCounts() {
        $.ajax({
            url: '/Dashboard/GetTestCountBlockData', // API endpoint
            method: 'GET',
            success: function (response) {
                // Construct the display text
                // const displayText = `This Month: ${response[0]}, Today: ${response[1]}`;

                // Update the HTML content of the target div
                $('#testUserMonthNumber').text(response[0]);
                $('#testUserDayNumber').text(response[1]);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching test user counts:", error);
                $('#testUserNumber').text("Error loading test user counts");
            }
        });
    }
    // Call the function on page load
    updateTestUserCounts();

    function updateCompletedUserCounts() {
        $.ajax({
            url: '/Dashboard/GetCompletedCountBlockData', // API endpoint
            method: 'GET',
            success: function (response) {

                // Update the HTML content of the target div
                $('#completedUserMonthNumber').text(response[0]);
                $('#completedUserDayNumber').text(response[1]);
            },
            error: function (xhr, status, error) {
                //console.error("Error fetching test user counts:", error);
                $('#completedUserDayNumber').text("Error loading test user counts");
            }
        });
    }
    // Call the function on page load
    updateCompletedUserCounts();
    function updateNewUserCounts() {
        $.ajax({
            url: '/Dashboard/GetNewUsersCountBlockData', // API endpoint
            method: 'GET',
            success: function (response) {

                // Update the HTML content of the target div
                $('#newUserMonthNumber').text(response[0]);
                $('#newUserDayNumber').text(response[1]);
            },
            error: function (xhr, status, error) {
                //console.error("Error fetching test user counts:", error);
                $('#newUserDayNumber').text("Error loading test user counts");
            }
        });
    }
    // Call the function on page load
    updateNewUserCounts();
    function updateRepeatLoginsCounts() {
        $.ajax({
            url: '/Dashboard/GetRepeatLoginCountBlockData', // API endpoint
            method: 'GET',
            success: function (response) {

                // Update the HTML content of the target div
                $('#repeatLoginsMonthNumber').text(response[0]);
                $('#repeatLoginsDayNumber').text(response[1]);
            },
            error: function (xhr, status, error) {
                //console.error("Error fetching test user counts:", error);
                $('#repeatLoginsDayNumber').text("Error loading login user counts");
            }
        });
    }
    // Call the function on page load
    updateRepeatLoginsCounts();

    //end of block data


    //sigout/logout from current session
    document.getElementById('signOutLink').addEventListener('click', function (event) {
        event.preventDefault();

        if (confirm('Are you sure you want to sign out?')) {
            window.location.href = '/Auth/LogoutCover';
        }
    });

    //Radar – Multiple Series
    var options = {
        series: [{
            name: 'Visit',
            data: [80, 50, 30, 40, 100, 20],
        }, {
            name: 'Return',
            data: [20, 30, 40, 80, 20, 80],
        }, {
            name: 'Sales',
            data: [44, 76, 78, 13, 43, 10],
        }],
        chart: {
            height: 350,
            type: 'radar',
            dropShadow: {
                enabled: true,
                blur: 1,
                left: 1,
                top: 1
            }
        },
        stroke: {
            width: 2
        },
        colors: getChartColorsArray("radarMultipleSeries"),
        fill: {
            opacity: 0.1
        },
        markers: {
            size: 0
        },
        xaxis: {
            categories: ['2019', '2020', '2021', '2022', '2023', '2024']
        }
    };

    var chart = new ApexCharts(document.querySelector("#radarMultipleSeries"), options);
    chart.render();

    //Audience Chart
    var options = {
        series: [{
            name: 'Male',
            data: [44, 55, 41, 67, 22, 43, 26]
        }, {
            name: 'Female',
            data: [13, 23, 20, 8, 13, 27, 41]
        }],
        chart: {
            type: 'bar',
            height: 390,
            stacked: true,
            toolbar: {
                show: false
            },
            zoom: {
                enabled: true
            }
        },
        plotOptions: {
            bar: {
                horizontal: false,
                borderRadius: 6,
                columnWidth: '44%',
                dataLabels: {
                    total: {
                        enabled: true,
                        style: {
                            fontSize: '13px',
                            fontWeight: 600
                        }
                    }
                }
            },
        },
        xaxis: {
            type: 'datetime',
            categories: ['01/01/2023 GMT', '01/02/2023 GMT', '01/03/2023 GMT', '01/04/2023 GMT',
                '01/05/2023 GMT', '01/06/2023 GMT', '01/07/2023 GMT'
            ],
        },
        colors: getChartColorsArray("audienceChart"),
        legend: {
            position: 'top',
            horizontalAlign: 'right',
        },
        fill: {
            opacity: 1
        }
    };

    var chart = new ApexCharts(document.querySelector("#audienceChart"), options);
    chart.render();

});
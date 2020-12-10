$(document).ready(function () {
    getCurrentYearSaleSummaryByMonth();
    getCurrentYearDueSummaryByMonth();
});

function getCurrentYearSaleSummaryByMonth() {
    $.ajax({
        url: '/Home/GetCurrentYearSaleSummaryByMonth',
        type: 'GET',
        contentType: "application/json; charset=utf-8",  
        dataType: 'json',
        async: true,
        cache: false,      
        success: function (response) {            
            generateCurrentYearSaleSummaryByMonthChart(response);
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function getCurrentYearDueSummaryByMonth() {
    $.ajax({
        url: '/Home/GetCurrentYearDueSummaryByMonth',
        type: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        cache: false,
        success: function (response) {
            generateCurrentYearDueSummaryByMonthChart(response);
        },
        error: function (response) {
            console.log(response);
        }
    });
}

function generateCurrentYearSaleSummaryByMonthChart(response) {
    var ctx = document.getElementById('currentYearSaleByMonthchart').getContext('2d');
    var customerChart = new Chart(ctx, {
        type: 'bar',
        responsive: true,
        data: {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November','December'],
            datasets: [{
                label: 'Current Year Sale By Month',
                data: response.sellAmount,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}

function generateCurrentYearDueSummaryByMonthChart(response) {
    var ctx = document.getElementById('currentYearDueByMonthchart').getContext('2d');
    var customerChart = new Chart(ctx, {
        type: 'bar',
        responsive: true,
        data: {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            datasets: [{
                label: 'Current Year Due By Month',
                data: response.dueAmount,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}



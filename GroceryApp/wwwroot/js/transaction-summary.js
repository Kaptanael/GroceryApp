$(document).ready(function () {

    loadFullNameAutocomplete();

    loadFirstNameAutocomplete();

    loadLastNameAutocomplete();

    loadMobileAutocomplete();

    loadEmailAutocomplete();

    $('#transaction-summary-table').DataTable({
        language: {
            processing: "Loading Data...",
            zeroRecords: "No matching records found"
        },
        processing: true,
        serverSide: true,
        filter: true,
        orderMulti: false,
        autoWidth: false,
        lengthMenu: [5, 10, 15, 20],
        dom: '<"top"i>rt<"bottom"lp><"clear">',
        ajax: {
            url: "/Transactions/GetAllTransactionSummaryByFilter",
            type: "POST",
            datatype: "json",
            async: true
        },
        columns: [
            { data: 'customerId', name: 'CustomerId', orderable: true, visible: false, searchable: false },
            { data: 'fullName', name: 'FullName', orderable: true },
            { data: 'mobile', name: 'Mobile', orderable: true },
            { data: 'email', name: 'Email', orderable: true },
            { data: 'totalSellAmount', name: 'TotalSellAmount', orderable: true, className: 'text-right' },
            { data: 'totalReceiveAmount', name: 'TotalReceiveAmount', orderable: true, className: 'text-right' },
            { data: 'totalDueAmount', name: 'TotalDueAmount', orderable: true, className: 'text-right'},
            { data: 'totalAmount', name: 'TotalAmount', orderable: true, className: 'text-right' }
        ]
    });

    var dataTable = $('#transaction-summary-table').DataTable();

    $('#search').click(function (e) {
        e.preventDefault();
        dataTable.columns(1).search($('#fullName').val().trim());
        dataTable.columns(2).search($('#mobile').val().trim());
        dataTable.columns(3).search($('#email').val().trim());
        dataTable.columns(4).search($('#fromDate').val().trim());
        dataTable.columns(5).search($('#toDate').val().trim());
        dataTable.draw();
    });

    $('#clear').click(function (e) {
        e.preventDefault();
        $('#fullName').val('');
        $('#mobile').val('');
        $('#email').val('');
        $('#fromDate').val('');
        $('#toDate').val('');
        dataTable.search('').columns().search('').draw();
    });

    $("#toDate").change(function () {
        var fromDate = document.getElementById("fromDate").value;
        var toDate = document.getElementById("toDate").value;

        if (Date.parse(fromDate) >= Date.parse(toDate)) {
            document.getElementById("toDate").value = "";
            $("#deleteMessage").html(generateMessage('danger'));
            $("#message").html('To date should be greater than From date');
        }
    });
});


function loadFirstNameAutocomplete() {
    $("#firstName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Transactions/GetAllCustomerByFirstName',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.firstName,
                            value: item.firstName
                        };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $('#firstName').val(ui.item.value);
            return false;
        }
    });
}

function loadLastNameAutocomplete() {
    $("#lastName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Transactions/GetAllCustomerByLastName',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.lastName,
                            value: item.lasttName
                        };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $('#lastName').val(ui.item.value);
            return false;
        }
    });
}

function loadFullNameAutocomplete() {
    $("#fullName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Transactions/GetAllCustomerByFullName',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.fullName,
                            value: item.fullName
                        };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $('#fullName').val(ui.item.value);
            return false;
        }
    });
}

function loadMobileAutocomplete() {
    $("#mobile").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Transactions/GetAllCustomerByMobile',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.mobile,
                            value: item.mobile
                        };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $('#mobile').val(ui.item.value);
            return false;
        }
    });
}

function loadEmailAutocomplete() {
    $("#email").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Transactions/GetAllCustomerByEmail',
                type: 'GET',
                cache: false,
                data: request,
                dataType: 'json',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.email,
                            value: item.email
                        };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $('#email').val(ui.item.value);
            return false;
        }
    });
}

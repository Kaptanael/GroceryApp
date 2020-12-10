$(document).ready(function () {

    setTimeout(function () {
        $("#successMessageDiv").remove();
    }, 3000);  

    var fullName = getParameterByName('fullName'); 

    if (fullName !== null){
        $('#fullName').val(fullName);
    }

    loadFullNameAutocomplete();

    loadFirstNameAutocomplete();

    loadLastNameAutocomplete();

    loadMobileAutocomplete();

    $('#transaction-table').DataTable({
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
            url: "/Transactions/GetAllTransactionByFilter?" + $.param({ fullName: fullName }),
            type: "POST",
            datatype: "json",
            async: true
        },
        columns: [
            { data: 'transactionId', name: 'TransactionId', orderable: true, visible: false, searchable: false },
            { data: 'status', name: 'Status', orderable: false, visible: false, searchable: false },
            { data: 'firstName', name: 'FirstName', orderable: true },
            { data: 'lastName', name: 'LastName', orderable: true },
            { data: 'fullName', name: 'FullName', orderable: true },
            { data: 'mobile', name: 'Mobile', orderable: true },
            { data: 'soldAmount', name: 'SoldAmount', orderable: true },
            { data: 'receivedAmount', name: 'ReceivedAmount', orderable: true },
            {
                data: 'transactionDate', name: 'TransactionDate', orderable: true,
                render: function (value) {
                    if (value === null) return "";
                    return moment(value).format('DD/MM/YYYY');
                }
            },
            {
                data: null,
                orderable: false,
                autoWidth: true,
                render: function (data, type, row) {
                    if (row.status === true) {
                        return '<div>'
                            + '<a href="/Transactions/AddEditTransaction/' + row.transactionId + '" class="btn btn-primary btn-sm mr-1">'
                            + '<i class="glyphicon glyphicon-pencil"></i>  Edit'
                            + '</a>'
                            + '<span id="confirmDeleteSpan_' + row.transactionId + '" style="display:none">'
                            + '<span> Are you sure you want to delete?</span>'
                            + '<button type="button" class="btn btn-danger btn-sm ml-1 mr-1" onclick="deleteTransaction(' + row.transactionId + ')">Yes</button>'
                            + '<button type="button" class="btn btn-primary btn-sm mr-1" onclick="confirmDelete(' + row.transactionId + ', false)">No</button>'
                            + '</span>'
                            + '<span id="deleteSpan_' + row.transactionId + '"><button type="button" class="btn btn-danger btn-sm mr-1" onclick="confirmDelete(' + row.transactionId + ', true)">Delete</button></span>'
                            + '</div>';
                    } else {
                        return '<div class="btn btn-info">Inactive customer</div>';
                    }
                }
            }
        ]
    });

    var dataTable = $('#transaction-table').DataTable();

    $('#search').click(function (e) {
        e.preventDefault();
        dataTable.columns(1).search($('#firstName').val().trim());
        dataTable.columns(2).search($('#lastName').val().trim());
        dataTable.columns(3).search($('#fullName').val().trim());
        dataTable.columns(4).search($('#mobile').val().trim());
        dataTable.columns(5).search($('#fromDate').val().trim());
        dataTable.columns(6).search($('#toDate').val().trim());
        dataTable.draw();
    });

    $('#clear').click(function (e) {
        e.preventDefault();
        $('#firstName').val('');
        $('#lastName').val('');
        $('#fullName').val('');
        $('#mobile').val('');
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

function deleteTransaction(transactionId) {
    $.ajax({
        url: '/Transactions/DeleteTransaction/' + transactionId,
        type: "DELETE",
        async: true,
        cache: false,
        success: function (response) {
            if (response.type === 1) {
                $("#deleteMessageDiv").html(generateMessage('success'));
                $("#message").html(response.message);
                $('#transaction-table').DataTable().draw();
                setTimeout(function () {
                    $("#deleteMessage").remove();
                }, 3000);
            }
            else if (response.type === 2) {
                $("#deleteMessageDiv").html(generateMessage('info'));
                $("#message").html(response.message);
                setTimeout(function () {
                    $("#deleteMessage").remove();
                }, 3000);
            }
            else if (response.type === 3) {
                $("#deleteMessageDiv").html(generateMessage('warning'));
                $("#message").html(response.message);
                setTimeout(function () {
                    $("#deleteMessage").remove();
                }, 3000);
            }
            else if (response.type === 4) {
                $("#deleteMessageDiv").html(generateMessage('danger'));
                $("#message").html(response.message);
                setTimeout(function () {
                    $("#deleteMessage").remove();
                }, 3000);
            }
        },
        error: function (errorResponse) {
            $("#deleteMessageDiv").html(generateMessage('danger'));
            $("#message").html(errorResponse);
            setTimeout(function () {
                $("#deleteMessage").remove();
            }, 3000);
        }
    });
}

function generateMessage(className) {
    var html = '<div id="deleteMessage" class="row">'
        + '<div class="col-md-10 offset-md-1">'
        + '<div class="alert alert-' + className + ' alert-dismissible">'
        + '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'
        + '<strong id="message"></strong>'
        + '</div>'
        + '</div>'
        + '</div>';
    return html;
}

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

function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
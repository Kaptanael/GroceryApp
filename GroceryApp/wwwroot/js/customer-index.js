$(document).ready(function () {

    loadFirstNameAutocomplete();

    loadLastNameAutocomplete();

    loadFullNameAutocomplete();

    loadMobileAutocomplete();

    $('#customer-table').DataTable({
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
            url: "/Customers/GetAllCustomerByFilter",
            type: "POST",
            datatype: "json",
            async: true
        },
        columns: [
            { data: 'customerId', name: 'CustomerId', orderable: true, visible: false, searchable: false },
            { data: 'firstName', name: 'FirstName', orderable: true },
            { data: 'lastName', name: 'LastName', orderable: true },
            { data: 'fullName', name: 'FullName', orderable: true },
            { data: 'mobile', name: 'Mobile', orderable: true },
            { data: 'email', name: 'Email', orderable: true },
            {
                data: null,
                orderable: false,
                autoWidth: true,
                render: function (data, type, row) {
                    return '<div>'
                        + '<a href="/Customers/AddEditCustomer/' + row.customerId + '" class="btn btn-primary btn-sm mr-1">'
                        + '<i class="glyphicon glyphicon-pencil"></i>  Edit'
                        + '</a>'
                        + '<span id="confirmDeleteSpan_' + row.customerId + '" style="display:none">'
                        + '<span> Are you sure you want to delete?</span>'
                        + '<button type="button" class="btn btn-danger btn-sm ml-1 mr-1" onclick="deleteCustomer(' + row.customerId + ')">Yes</button>'
                        + '<button type="button" class="btn btn-primary btn-sm mr-1" onclick="confirmDelete(' + row.customerId + ', false)">No</button>'
                        + '</span>'
                        + '<span id="deleteSpan_' + row.customerId + '"><button type="button" class="btn btn-danger btn-sm mr-1" onclick="confirmDelete(' + row.customerId + ', true)">Delete</button></span>'
                        + '<a href="/Transactions/Index?pageNumber=1&pageSize=10&sortOrder=asc&fullName=' + row.fullName + '" class="btn btn-info btn-sm">'
                        + '<i class="glyphicon glyphicon-pencil"></i>  Transaction Detail'
                        + '</a>'
                        + '</div>';
                }
            }
        ]
    });

    var dataTable = $('#customer-table').DataTable();

    $('#search').click(function (e) {
        e.preventDefault();
        dataTable.columns(1).search($('#firstName').val().trim());
        dataTable.columns(2).search($('#lastName').val().trim());
        dataTable.columns(3).search($('#fullName').val().trim());
        dataTable.columns(4).search($('#mobile').val().trim());
        dataTable.draw();
    });

    $('#clear').click(function (e) {
        e.preventDefault();
        $('#firstName').val('');
        $('#lastName').val('');
        $('#fullName').val('');
        $('#mobile').val('');
        dataTable.search('').columns().search('').draw();
    });
});

function deleteCustomer(customerId) {
    $.ajax({
        url: '/Customers/DeleteCustomer/' + customerId,
        type: "DELETE",
        async: true,
        cache: false,
        success: function (response) {
            if (response.type === 1) {
                $("#deleteMessage").html(generateMessage('success'));
                $("#message").html(response.message);
                $('#customer-table').DataTable().draw();
            }
            else if (response.type === 2) {
                $("#deleteMessage").html(generateMessage('info'));
                $("#message").html(response.message);
            }
            else if (response.type === 3) {
                $("#deleteMessage").html(generateMessage('warning'));
                $("#message").html(response.message);
            }
            else if (response.type === 4) {
                $("#deleteMessage").html(generateMessage('danger'));
                $("#message").html(response.message);
            }
        },
        error: function (errorResponse) {
            $("#deleteMessage").html(generateMessage('danger'));
            $("#message").html(errorResponse);
        }
    });
}

function generateMessage(className) {
    var html = '<div class="row">'
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
                url: '/Customers/GetAllCustomerByFirstName',
                type: 'GET',
                dataType: 'json',
                data: request,
                async: true,
                cache: false,
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
                url: '/Customers/GetAllCustomerByLastName',
                type: 'GET',
                dataType: 'json',
                data: request,
                async: true,
                cache: false,
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.lastName,
                            value: item.lastName
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
                url: '/Customers/GetAllCustomerByFullName',
                type: 'GET',
                dataType: 'json',
                data: request,
                async: true,
                cache: false,
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
                url: '/Customers/GetAllCustomerByMobile',
                type: 'GET',
                dataType: 'json',
                data: request,
                async: true,
                cache: false,
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
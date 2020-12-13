//(function ($) {

//    setTimeout(function () {
//        $("#successMessageDiv").remove();
//    }, 3000);

//    function AddEditTransaction() {
//        var $this = this;

//        function initialize() {
//            $('#Description').summernote({
//                focus: false,
//                height: 150,
//                codemirror: {
//                    theme: 'united'
//                }
//            });
//        }

//        $this.init = function () {
//            initialize();
//        };
//    }

//    $(function () {
//        var self = new AddEditTransaction();
//        self.init();
//    });

//    jQuery.validator.setDefaults({
//        ignore: ":hidden, [contenteditable='true']:not([name])"
//    });    

//}(jQuery));

(function ($) {

    setTimeout(function () {
        $("#successMessageDiv").remove();
    }, 3000);

    function AddEditTransaction() {
        var $this = this;

        function initialize() {
            $('#Description').summernote({
                focus: false,
                height: 150,
                codemirror: {
                    theme: 'united'
                }
            });

            var amountTextBox = $('#Amount');
            amountTextBox.focus();
            var amount = amountTextBox.val();
            amountTextBox.val('');
            amountTextBox.val(amount);
        }

        $this.init = function () {
            initialize();
        };
    }

    $(function () {
        var self = new AddEditTransaction();
        self.init();
    });

    jQuery.validator.setDefaults({
        ignore: ":hidden, [contenteditable='true']:not([name])"
    });

    $('#clear').click(function () {
        $('#Amount').val('');
        $('#Description').summernote('reset');
        $('#TransactionDate').val('');
        $('#CustomerId').val('');
        resetValidations();
    });

}(jQuery));

function resetValidations() {
    //Removes validation from input-fields
    $('.input-validation-error').addClass('input-validation-valid');
    $('.input-validation-error').removeClass('input-validation-error');
    //Removes validation message after input-fields
    $('.field-validation-error').addClass('field-validation-valid');
    $('.field-validation-error').removeClass('field-validation-error');
    //Removes validation summary 
    $('.validation-summary-errors').addClass('validation-summary-valid');
    $('.validation-summary-errors').removeClass('validation-summary-errors');

    $('#Amount-error').remove();
    $('#Description-error').remove();
    $('#TransactionDate-error').remove();
    $('#CustomerId-error').remove();
}




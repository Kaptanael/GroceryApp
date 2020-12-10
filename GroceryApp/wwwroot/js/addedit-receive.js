(function ($) {

    setTimeout(function () {
        $("#successMessageDiv").remove();
    }, 3000);

    function AddEditReceive() {
        var $this = this;

        function initialize() {
            $('#Description').summernote({
                focus: false,
                height: 150,
                codemirror: {
                    theme: 'united'
                }
            });
        }

        $this.init = function () {
            initialize();
        };
    }

    $(function () {
        var self = new AddEditReceive();
        self.init();        
    });
    
    jQuery.validator.setDefaults({        
        ignore: ":hidden, [contenteditable='true']:not([name])"
    });    

    $('#clear').click(function () {        
        $('#Description').summernote('reset');
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

    $('#ReceivedAmount-error').remove();
    $('#Description-error').remove();
    $('#TransactionDate-error').remove();
    $('#CustomerId-error').remove();
}




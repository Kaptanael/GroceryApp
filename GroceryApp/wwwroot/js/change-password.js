$(document).ready(function () {

    setTimeout(function () {
        $("#successMessageDiv").remove();
    }, 3000);

    $('#clear').click(function () {
        resetValidations();
    });
});

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
    
    $('#OldPassword-error').remove();
    $('#NewPassword-error').remove();
    $('#ConfirmPassword-error').remove();    
}
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

}(jQuery));

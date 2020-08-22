(function ($) {
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
}(jQuery));

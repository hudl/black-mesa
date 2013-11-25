(function ($) {

    $(document).ready(function () {
        $('#login-form').submit(function (evt) {
            evt.preventDefault();
            evt.stopPropagation();
            $.post(BlackMesa.config.loginServer, { username: $('#u').val().replace('@hudl.com', ''), password: $('#p').val() }, function (data) {
                if (!data.success) {
                    toastr.error(data.message, 'Error')
                    return;
                }
                window.location = "/Deploys";
            });
        });
    });

})(jQuery);
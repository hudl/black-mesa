(function ($) {

    $(document).ready(function () {
        $('#login').on('click', function () {
            $.post(BlackMesa.config.loginServer, { username: $('#u').val(), password: $('#p').val() }, function (data) {
                if (!data.success) {
                    toastr.error(data.message, 'Error')
                    return;
                }
                window.location = "/Deploys";
            });
        });
    });

})(jQuery);
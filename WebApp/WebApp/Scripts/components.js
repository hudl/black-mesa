$(function () {
    $('#component-form #component-name').focus();

    $('.delete').on('click', function () {
        var item = $(this);
        $.ajax({
            url: '/api/v1/components/' + item.attr('id'),
            type: 'DELETE'
        }).done(function() {
            item.parents()[0].remove();
        });
    });
});
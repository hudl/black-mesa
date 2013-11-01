$(function () {
    $('#project-form #project-name').focus();

    $('.delete').on('click', function () {
        var item = $(this);
        $.ajax({
            url: '/api/v1/projects/' + item.attr('id'),
            type: 'DELETE'
        }).done(function () {
            item.parents()[0].remove();
        });
    });
});
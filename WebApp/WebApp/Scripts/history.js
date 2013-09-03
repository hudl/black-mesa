(function ($) {

    var historyItem = '<div class="history-item">Person: {Person}<br />Property: {Property}<br />Old Value: {OldValue}<br />New Value: {NewValue}<br /><a href="#" class="row-finder" data-row-id="{DeployId}">Take Me There</a></div><br />';

    $('#load-history').on('click', function () {
        var historyDiv = $('#history');
        historyDiv.show();
        $.get('api/v1/history', function (data) {
            _.each(data.items, function (history) {
                historyDiv.append(historyItem.replace("{Person}", history.person)
                                                .replace("{Property}", history.propertyChanged)
                                                .replace("{OldValue}", history.oldValue)
                                                .replace("{NewValue}", history.newValue)
                                                .replace("{DeployId}", history.deployId));
            });
        });
    });

})(jQuery);
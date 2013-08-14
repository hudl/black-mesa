(function (deployform, $, _) {

    $('.hotfix').live('click', function () {
        var hotfixMongoId = $(this).data("mongo-id");
        var hotfixes = window.grid.getData().getItemById(hotfixMongoId).hotfixes;
        if (hotfixes == undefined || hotfixes[0] == undefined) {
            toastr.error('This deploy didn\'t have associated hotfix data.');
            return;
        }
        var hotfix = hotfixes[0];
        $('#hotfix').modal({
            minHeight: 300,
            maxHeight: 800,
            minWidth: 400,
            onOpen: function (dialog) {
                dialog.overlay.fadeIn(200, function () {
                    dialog.container.slideDown(200, function () {
                        dialog.data.fadeIn(200);
                    });
                });
                var prodTicket = hotfix.prodTicket ? '<a href="http://jira/browse/PROD-' + hotfix.prodTicket + '" target="_blank">' + hotfix.prodTicket + '</a>' : "None";
                $('#prod-ticket').html(prodTicket);
                // A lot of tickets have text appended to the ticket. Break after the space so the link works.
                var ticket;
                if (hotfix.ticket) {
                    if (hotfix.ticket.indexOf(" ") != -1) {
                        hotfix.ticket = hotfix.ticket.substring(0, hotfix.ticket.indexOf(" "));
                    }
                    ticket = '<a href="http://jira/browse/' + hotfix.ticket + '" target="_blank">' + hotfix.ticket + '</a>';
                } else {
                    ticket = "None";
                }
                $('#hotfix-view-ticket').html(ticket);
                $('#bad-branch').text(hotfix.branchThatBrokeIt);
                $('#hotfix-view-notes').text(hotfix.notes);
                $('#hotfix-view-special').text(hotfix.special);
                $('#dev-team-culp').text(hotfix.assessments.developers.culpability);
                $('#dev-hudl-impact').text(hotfix.assessments.developers.hudlWideImpact);
                $('#dev-affected-users').text(hotfix.assessments.developers.affectedUserImpact);
                $('#dev-initials').text(hotfix.assessments.developers.initials);
                $('#qa-team-culp').text(hotfix.assessments.quails.culpability);
                $('#qa-hudl-impact').text(hotfix.assessments.quails.hudlWideImpact);
                $('#qa-affected-users').text(hotfix.assessments.quails.affectedUserImpact);
                $('#qa-initials').text(hotfix.assessments.quails.initials);
            }
        });
    });

})(window.hotfix, jQuery, _);
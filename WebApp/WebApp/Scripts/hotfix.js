(function (deployform, $, _) {

    $(document).ready(function () {
        BlackMesa.hotfix = (function () {

            function flattenHotfixData(postingData, structuredData) {
                if (!structuredData.hotfixes || structuredData.hotfixes.length == 0) return;

                postingData.prodTicket = structuredData.hotfixes[0].prodTicket;
                postingData.ticket = structuredData.hotfixes[0].ticket;
                postingData.qaTeamCulp = structuredData.hotfixes[0].assessments.quails.culpability;
                postingData.qaHudlImpact = structuredData.hotfixes[0].assessments.quails.hudlWideImpact;
                postingData.qaUserImpact = structuredData.hotfixes[0].assessments.quails.affectedUserImpact;
                postingData.qaInitials = structuredData.hotfixes[0].assessments.quails.initials;
                postingData.devTeamCulp = structuredData.hotfixes[0].assessments.developers.culpability;
                postingData.devHudlImpact = structuredData.hotfixes[0].assessments.developers.hudlWideImpact;
                postingData.devUserImpact = structuredData.hotfixes[0].assessments.developers.affectedUserImpact;
                postingData.devInitials = structuredData.hotfixes[0].assessments.developers.initials;
                postingData.badBranch = structuredData.hotfixes[0].branchThatBrokeIt;
                postingData.hotfixNotes = structuredData.hotfixes[0].notes;
                postingData.special = structuredData.hotfixes[0].special;
                postingData.hotfixComponent = structuredData.hotfixes[0].hotfixComponent;
                postingData.theProblem = structuredData.hotfixes[0].theProblem;
                postingData.howMissed = structuredData.hotfixes[0].howMissed;
                postingData.theFix = structuredData.hotfixes[0].theFix;
            }

            return {
                flattenHotfixData: flattenHotfixData
            }
        });
    });

    $(document).on('click', '.hotfix', function (event) {
        event.preventDefault();
        var hotfixMongoId = $(event.target).data("mongo-id");
        if (!hotfixMongoId) {
            return;
        }
        var deploy = window.grid.getData().getItemById(hotfixMongoId);
        var hotfixes = deploy.hotfixes;
        if (hotfixes == undefined || hotfixes[0] == undefined) {
            deploy.hotfixes = [];
            // The hotfix needs parent properties to be defined
            deploy.hotfixes[0] = {
                assessments: {
                    developers: {},
                    quails: {}
                }
            };
            var hotfixes = deploy.hotfixes;
        }
        var hotfix = hotfixes[0];
        $('#hotfix').modal({
            minHeight: 300,
            maxHeight: 900,
            minWidth: 1150,
            overlayClose: true,
            onOpen: function (dialog) {
                dialog.overlay.fadeIn(200, function () {
                    dialog.container.slideDown(200, function () {
                        dialog.data.fadeIn(200);
                    });
                });
                var prodTicket = hotfix.prodTicket ? '<a href="http://jira/browse/PROD-' + hotfix.prodTicket + '" target="_blank">' + hotfix.prodTicket + '</a>' : "None";
                $('#prod-ticket-link').html(prodTicket);
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
                $('#hotfix-view-ticket-link').html(ticket);
                $('#hotfix-branch-name').text(deploy.branch);
                $('#prod-ticket').val(hotfix.prodTicket);
                $('#hotfix-view-ticket').val(hotfix.ticket);
                $('#bad-branch').val(hotfix.branchThatBrokeIt);
                $('#hotfix-view-notes').val(hotfix.notes);
                $('#hotfix-view-special').val(hotfix.special);
                $('#dev-team-culp').val(hotfix.assessments.developers.culpability);
                $('#dev-hudl-impact').val(hotfix.assessments.developers.hudlWideImpact);
                $('#dev-affected-users').val(hotfix.assessments.developers.affectedUserImpact);
                $('#dev-initials').val(hotfix.assessments.developers.initials);
                $('#qa-team-culp').val(hotfix.assessments.quails.culpability);
                $('#qa-hudl-impact').val(hotfix.assessments.quails.hudlWideImpact);
                $('#qa-affected-users').val(hotfix.assessments.quails.affectedUserImpact);
                $('#qa-initials').val(hotfix.assessments.quails.initials);
                $('#hotfix-view-component').val(hotfix.hotfixComponent);
                $('#the-problem').val(hotfix.theProblem);
                $('#how-missed').val(hotfix.howMissed);
                $('#the-fix').val(hotfix.theFix);

                $('#dev-team-culp,#dev-hudl-impact,#dev-affected-users,#qa-team-culp,#qa-hudl-impact,#qa-affected-users').each(function () {
                    applyHotfixRatingColor($(this));
                });
                showBadRatingsFields();
            }
        });

        $('#prod-ticket').on('change', function () {
            var prodTicket = $('#prod-ticket').val() ? '<a href="http://jira/browse/PROD-' + $('#prod-ticket').val() + '" target="_blank">PROD-' + $('#prod-ticket').val() + '</a>' : "None";
            $('#prod-ticket-link').html(prodTicket);
        });

        $('#hotfix-view-ticket').on('change', function () {
            var prodTicket = $('#hotfix-view-ticket').val() ? '<a href="http://jira/browse/' + $('#hotfix-view-ticket').val() + '" target="_blank">' + $('#hotfix-view-ticket').val() + '</a>' : "None";
            $('#hotfix-view-ticket-link').html(prodTicket);
        });

        $('#dev-team-culp,#dev-hudl-impact,#dev-affected-users,#qa-team-culp,#qa-hudl-impact,#qa-affected-users').on('change', function () {
            applyHotfixRatingColor($(this));
            showBadRatingsFields();
        });

        function showBadRatingsFields() {
            var badRatings = false;
            $('#dev-team-culp,#dev-hudl-impact,#dev-affected-users,#qa-team-culp,#qa-hudl-impact,#qa-affected-users').each(function () {
                if ($(this).val() >= 4) {
                    badRatings = true;
                }
            });
            if (badRatings) {
                $('#bad-rating-detail').show();
            } else {
                $('#bad-rating-detail').hide();
            }
        }

        $('#hotfix-save').click(function () {
            var data = window.grid.getData().getItemById(hotfixMongoId);
            data.hotfixes[0].prodTicket = $('#prod-ticket').val();
            data.hotfixes[0].ticket = $('#hotfix-view-ticket').val();
            data.hotfixes[0].branchThatBrokeIt = $('#bad-branch').val();
            data.hotfixes[0].notes = $('#hotfix-view-notes').val();
            data.hotfixes[0].special = $('#hotfix-view-special').val();
            data.hotfixes[0].assessments.developers.culpability = $('#dev-team-culp').val();
            data.hotfixes[0].assessments.developers.hudlWideImpact = $('#dev-hudl-impact').val();
            data.hotfixes[0].assessments.developers.affectedUserImpact = $('#dev-affected-users').val();
            data.hotfixes[0].assessments.developers.initials = $('#dev-initials').val();
            data.hotfixes[0].assessments.quails.culpability = $('#qa-team-culp').val();
            data.hotfixes[0].assessments.quails.hudlWideImpact = $('#qa-hudl-impact').val();
            data.hotfixes[0].assessments.quails.affectedUserImpact = $('#qa-affected-users').val();
            data.hotfixes[0].assessments.quails.initials = $('#qa-initials').val();
            data.hotfixes[0].hotfixComponent = $('#hotfix-view-component').val();
            data.hotfixes[0].theProblem = $('#the-problem').val();
            data.hotfixes[0].howMissed = $('#how-missed').val();
            data.hotfixes[0].theFix = $('#the-fix').val();
            BlackMesa.hotfix().flattenHotfixData(data, data);
            $.post("/api/v1/deploys", data, function (response) {
                toastr.info('Updated Hotfix Data');
            });
            $.modal.close(); // closes all modals. There will only ever be 1, so this will just have to work. Only way documented to do it.
        });

        function applyHotfixRatingColor(element) {
            switch (element.val()) {
                case "3":
                    element.css("background-color", "#F7FE2E");
                    break;
                case "4":
                    element.css("background-color", "#FAAC58");
                    break;
                case "5":
                    element.css("background-color", "#FA5858");
                    break;
                default:
                    element.css("background-color", "#FFFFFF");
                    break;
            }
        }
    });

})(window.hotfix, jQuery, _);
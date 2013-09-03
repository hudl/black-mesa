(function (deployform, $, _) {

    var productTeamMembers;

    $(function () {
        $.getJSON('/api/v1/people', function (data) {
            productTeamMembers = data["accounts"];
            productTeamMembers = _.sortBy(productTeamMembers, function (user) { return user.Name; });
            var noneAccount = {
                Name: "None",
                Groups: ["all"]
            };
            productTeamMembers.unshift(noneAccount);
            // They all need ids for this :(
            $.each(productTeamMembers, function (index, value) {
                value.id = value.Name;
            });
        });
        $('#new-deploy').click(function () {
            $('#deploy-doc').modal({
                minHeight: 300,
                maxHeight: 800,
                minWidth: 800,
                overlayClose: true,
                onOpen: function (dialog) {
                    dialog.overlay.fadeIn(function () {
                        dialog.container.slideDown(function () {
                            dialog.data.fadeIn();
                        });
                    });
                    function select2Options(group, fullName) {
                        return {
                            data: { results: sortByRole(productTeamMembers, group), text: function (item) { return item.id; } },
                            formatSelection: format,
                            formatResult: formatDev,
                            tags: [],
                            width: "320px",
                            placeholder: fullName + "..."
                        }
                    };
                    $('#dev').select2(select2Options("Developers", "Developer"));
                    $('#qa').select2(select2Options("QA", "Quality Assurance"));
                    $('#design').select2(select2Options("Design", "Designer"));
                    $('#codeReview').select2(select2Options("Developers", "Code Review Done By"));
                    $('#projectManager').select2(select2Options("PM", "Project Manager"));
                    $('#devInitials').select2(select2Options("Developers", "Developer"));
                    $('#qaInitials').select2(select2Options("QA", "Quality Assurance"));

                    $('#hotfixComponent').select2({
                        width: "320px"
                    });
                    $('#pullRequestId').change(prChanged);

                    $(function () {
                        $('#bug-fix-yes').click(function () {
                            $('#time-frame-div').show();
                            $('#time-frame-div').prop('required', true);
                            $('#maint-div').hide();
                            $('#maint-div').prop('required', false);
                            $('#new-div').hide();
                            $('#new-div').prop('required', false);
                        });
                        $('#bug-fix-no').click(function () {
                            $('#time-frame-div').hide();
                            $('#how-bad-was-it-scores').hide();
                            $('#maint-div').show();
                        });
                        $('#maint-yes').click(function () {
                            $('#new-div').hide();
                            $('#type').val("Task");
                        });
                        $('#maint-no').click(function () {
                            $('#new-div').show();
                        });
                        $('#new-yes').click(function () {
                            $('#type').val("New Feature");
                        });
                        $('#new-no').click(function () {
                            $('#type').val("Enhancement");
                        });
                        $('#time-frame').change(function () {
                            var timeFrame = $('#time-frame').val();
                            if (timeFrame < 2) { // Same day or 1-3 Days.
                                $('#how-bad-was-it-scores').show();
                                $('#type').val("Hotfix");
                            } else {
                                $('#how-bad-was-it-scores').hide();
                                $('#type').val("Fix");
                            }
                        });
                        $('#basecampThread').change(function () {
                            if ($('#basecampThread').val() == '0') {
                                $('#basecamp-links').show();
                                $('#basecamp-div').hide();
                            } else {
                                $('#basecamp-links').hide();
                                $('#basecamp-div').show();
                            }
                        });
                        $('#deploy-doc-form').submit(function (evt) {
                            evt.preventDefault();
                            evt.stopPropagation();
                            var deployDocForm = $('#deploy-doc-form');
                            var complete = true;
                            var incompleteForms = new Array();
                            var i = 0;
                            deployDocForm.find('*').each(function () {
                                var el = $(this);
                                if (el.data('hudl-required') && (el.is(":visible") || el.data('hudl-remains-hidden')) && $.trim(el.val()) == '') {
                                    complete = false;
                                    incompleteForms[i++] = el.data('desc');
                                }
                            });
                            if (!complete) {
                                var str = "";
                                $.each(incompleteForms, function () {
                                    str += this + "\n";
                                });
                                var sumbitAnyway = confirm("You didn't fill out\n" + str + "\nSubmit anyway??")
                                if (sumbitAnyway) {
                                    submitDeployDoc(deployDocForm);
                                }
                            } else {
                                submitDeployDoc(deployDocForm);
                            }
                            return false;
                        });
                    });
                },
                onClose: function (dialog) {
                    dialog.container.slideUp(function () {
                        dialog.overlay.fadeOut(function () {
                            $.modal.close();
                        });
                    });
                }
            });
        });
    });

    function submitDeployDoc(deployDocForm) {
        var form = deployDocForm.serialize();
        $.post("/api/v1/deploys/new", form,
			function (data) {
			    if (data.success) {
			        var basecampPost = $('#basecampThread').val();
			        if (basecampPost > 0) {
			            var project, message;
			            _.each(BlackMesa.config.basecampThreads, function (basecampThread, index) {
			                if (basecampThread.threadId == basecampPost) {
			                    project = basecampThread.projectId;
			                    message = basecampThread.threadId;
			                }
			            });
			            if (message && project) {
			                $.post("/api/v1/basecamp",
                                {
                                    content: $('#basecamp').val(),
                                    project: project,
                                    message: message
                                },
                                function (data) {
                                    location.reload(true);
                                }
                            );
			            }
			        } else {
			            location.reload(true);
			        }
			    } else {
			        alert("I failed, I'm so sorry");
			    }
			}
		);
    }

    function sortByRole(users, role) {
        var i = 0;
        var sortedList = [];
        _.each(users, function (user) {
            if ($.inArray("all", user.Groups) > -1 || $.inArray(role, user.Groups) > -1) {
                sortedList[i++] = user;
            }
        });
        _.each(users, function (user) {
            if ($.inArray("all", user.Groups) == -1 && $.inArray(role, user.Groups) == -1) {
                sortedList[i++] = user;
            }
        });
        return sortedList;
    }

    function format(item) {
        return item.Name;
    }
    function formatDes(item) {
        return formatter(item, "Design");
    };
    function formatDev(item) {
        return formatter(item, "Developers");
    };
    function formatQa(item) {
        return formatter(item, "QA");
    };
    function formatPm(item) {
        return formatter(item, "PM");
    };
    function formatter(item, role) {
        if ($.inArray(role, item.Groups) > -1 || $.inArray("all", item.Groups) > -1) {
            return "<b>" + item.Name + "</b>";
        } else {
            return item.Name;
        }
    }

    function prChanged() {
        $('#pull-request-spinner').show();
        $.getJSON('/api/v1/github/pullRequest/' + $('#pullRequestId').val() + '/branch', function (data) {
            $.getJSON('/api/v1/github/pullRequest/' + $('#pullRequestId').val() + '/comments', function (comments) {
                if (comments != undefined && comments.length > 0) {
                    data.codeReview = comments[0].user.login;
                }
                $('#pull-request-spinner').hide();
                $('#qa').prop('disabled', false);
                $('#de').prop('disabled', false);
                $('#dev').prop('disabled', false);
                $('#branch').prop('disabled', false);
                $('#jiraLabel').prop('disabled', false);
                $('#codeReview').prop('disabled', false);
                $('#projectManager').prop('disabled', false);
                $('#notes').prop('disabled', false);
                if (data != undefined) {
                    $('#branch').val('').val(data.head.ref);
                    $('#jiraLabel').val('').val(data.head.ref);
                    $('#dev').select2("val", [fullNameForGithubName(data.user.login)]).trigger("change");
                    if (data.merged) {
                        $('#qa').select2("val", [fullNameForGithubName(data.merged_by.login)]).trigger("change");
                        $('#deploy-doc-submit').addClass('btn-primary')
                                                .removeClass('btn-danger')
                                                .prop('disabled', false);
                        $('#basecamp').val('').val('__CHANGE_TO_YOUR_NAME__ -- <b>' + data.head.ref + '</b>, by ' + fullNameForGithubName(data.user.login) + ', ');
                    } else {
                        $('#qa').select2("val", "").trigger("change");
                        $('#deploy-doc-submit').removeClass('btn-primary')
                                                .addClass('btn-danger')
                                                .prop('disabled', true);
                    }
                    $('#notes').val('').val(data.title);
                    if (data.codeReview != undefined) {
                        $('#codeReview').select2("val", [fullNameForGithubName(data.codeReview)]).trigger("change");
                    } else {
                        $('#codeReview').select2("val", "").trigger("change");
                    }
                    $.getJSON('/api/v1/jira/tickets/' + data.head.ref, function (data) {
                        if (data.total) {
                            $('#jiraTicketCount').val(data.total);
                        } else {
                            $('#jiraTicketCount').val("");
                        }
                    });
                } else {
                    $('#deploy-doc-form').each(function () {
                        this.reset();
                    });
                    $('#pullRequestId').val('').attr('placeholder', 'Invalid Pull Request ID');
                }
            });
        });
    };

    function parseName(username) {
        if (!username || !username.indexOf) return '';
        var index = username.indexOf('.');
        if (index === -1) {
            return username;
        }
        var first = username.slice(1, index),
            last = username.slice(index + 2);
        first = username.slice(0, 1).toUpperCase() + first;
        last = username.slice(index + 1, index + 2).toUpperCase() + last;
        if (last.indexOf('Mc') === 0) {
            last = 'Mc' + last.slice(2, 3).toUpperCase() + last.slice(3);
        }
        return first + ' ' + last;
    }

    function fullNameForGithubName(githubName) {
        var name;
        $.each(productTeamMembers, function (index, value) {
            if (value.GithubName == githubName) {
                name = value.Name;
                return false;
            }
        });
        return name;
    }

})(window.deployform, jQuery, _);
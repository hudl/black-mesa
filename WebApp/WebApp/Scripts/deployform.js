(function (deployform, $, _) {

    var productTeamMembers;

    $(function () {
        $.getJSON('/api/v1/github/repos', function (data) {
            var repos = [];
            _.each(data, function(repo) {
                repos.push(repo.name);
            });

            repos = _.sortBy(repos, function(repo) {
                return repo.toUpperCase();
            });

            var repoSelect = $('#repos');
            $.each(repos, function (i) {
                if (repos[i] === 'hudl') {
                    repoSelect.append('<option selected="selected">hudl</option>');
                } else {
                    repoSelect.append('<option>' + repos[i] + '</option>');
                }
            });
        });

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
                    function select2Options(group, fullName, formatter) {
                        return {
                            data: { results: sortByRole(productTeamMembers, group), text: function (item) { return item.id; } },
                            formatSelection: format,
                            formatResult: formatter,
                            tags: [],
                            width: "320px",
                            placeholder: fullName + "..."
                        }
                    };
                    $('#dev').select2(select2Options("Developers", "Developer", formatDev));
                    $('#qa').select2(select2Options("QA", "Quality Assurance", formatQa));
                    $('#design').select2(select2Options("Design", "Designer", formatDes));
                    $('#codeReview').select2(select2Options("Developers", "Code Review Done By", formatDev));
                    $('#projectManager').select2(select2Options("PM", "Project Manager", formatPm));
                    $('#devInitials').select2(select2Options("Developers", "Developer", formatDev));
                    $('#qaInitials').select2(select2Options("QA", "Quality Assurance", formatQa));

                    $('#hotfixComponent').select2({
                        width: "320px"
                    });
                    $('#repos').change(prChanged);
                    $('#pullRequestId').change(prChanged);

                    $(function () {
                        $('#bug-fix-yes').click(function () {
                            $('#hotfix-choice').show();
                            $('#maint-div').hide();
                            $('#maint-div').prop('required', false);
                            $('#new-div').hide();
                            $('#new-div').prop('required', false);
                        });
                        $('#bug-fix-no').click(function () {
                            $('#hotfix-choice').hide();
                            $('#how-bad-was-it-scores').hide();
                            $('#maint-div').show();
                        });
                        $('#hotfix-yes').click(function() {
                            $('#type').val("Hotfix");
                            $('#how-bad-was-it-scores').show();
                        });
                        $('#hotfix-no').click(function() {
                            $('#type').val("Fix");
                            $('#how-bad-was-it-scores').hide();
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
                            $('#deploy-doc-submit').hide();
                            $('#deploy-doc-submit-spinner').show();
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
                                } else {
                                    $('#deploy-doc-submit-spinner').hide();
                                    $('#deploy-doc-submit').show();
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
			    $('#deploy-doc-submit-spinner').hide();
			    $('#deploy-doc-submit').show();
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
        var pullRequestId = $('#pullRequestId');
        if (pullRequestId.val() < 1) {
            pullRequestId.val('').attr('placeholder', 'Invalid Pull Request ID');
            return;
        }
        $('#pull-request-spinner').show();
        var itemsToDisable = [
            $('#qa'),
            $('#de'),
            $('#dev'),
            $('#branch'),
            $('#jiraLabel'),
            $('#codeReview'),
            $('#projectManager'),
            $('#notes')
        ];
        _.each(itemsToDisable, function (item) {
            item.prop('disabled', true);
        });

        var baseUrl = '/api/v1/github/' + $('#repos').val() + '/pullRequest/' + pullRequestId.val();
        $.getJSON(baseUrl + '/branch', function (data) {
            $.getJSON(baseUrl + '/comments', function (comments) {
                _.each(itemsToDisable, function (item) {
                    item.prop('disabled', false);
                });
                $('#pull-request-spinner').hide();

                if (comments != undefined && comments.length > 0) {
                    data.codeReview = comments[0].user.login;
                }

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
                   pullRequestId.val('').attr('placeholder', 'Invalid Pull Request ID');
                }
            })
            .error(function () {
                $('#pull-request-spinner').hide();
                _.each(itemsToDisable, function (item) {
                    item.prop('disabled', false);
                });
                alert('Invalid PR');
            });
        }).error(function () {
            $('#pull-request-spinner').hide();
            _.each(itemsToDisable, function (item) {
                item.prop('disabled', false);
            });
            alert('Invalid PR');
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
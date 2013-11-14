(function ($) {
    $.extend(true, Slick.Editors, {
        "Jira": JiraEditor,
        "Type": TypeEditor,
        "Action": ActionEditor,
        "Component": ComponentEditor,
        "Project": ProjectEditor,
        "NoBlankTextEditor": NoBlankTextEditor,
        "QuailEditor": QuailEditor,
        "DesignerEditor": DesignerEditor,
        "DevEditor": DevEditor,
        "ProjectManagerEditor": ProjectManagerEditor,
        "HudlDateEditor": HudlDateEditor,
    })

    function JiraEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                })
                .focus()
                .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item.Branch || "";
            $input.val(defaultValue);
            $input[0].defaultValue = defaultValue;
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item.Branch = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }

    function TypeEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $('<input type="hidden" id="componentEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: ["Fix", "Hotfix", "Enhancement", "New Feature", "Task", "Rollback"],
                openOnEnter: false,
                multiple: false,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }

    function ActionEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $('<input type="hidden" id="componentEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: ["Deploy", "Script", "App", "Tool Fail", "Other"],
                openOnEnter: false,
                multiple: false,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }

    function ProjectEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;
        
        var projects = [];
        $.get('/api/v1/projects', function (data) {
            for (var d in data) {
                projects.push(data[d].name);
            }
        });

        this.init = function () {
            $input = $('<input type="hidden" id="componentEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: projects,
                openOnEnter: false,
                multiple: false,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }

    function ComponentEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;
        var components = [];
        $.get('/api/v1/components', function(data) {
            for (var d in data) {
                components.push(data[d].name);
            }
        });

        this.init = function () {
            $input = $('<input type="hidden" id="componentEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: components,
                openOnEnter: false,
                multiple: false,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }

    function NoBlankTextEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;

        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                })
                .focus()
                .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.val(defaultValue);
            $input[0].defaultValue = defaultValue;
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }
    
    function QuailEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;
        var people = [];
        var peopleTags = [];
        $.get('/api/v1/people', function (data) {
            people = data["accounts"];
            people = _.sortBy(people, function (user) { return user.Name; });
            var nonqa = [];
            peopleTags.push("None");
            _.each(people, function(user) {
                if (_.contains(user.Groups, "QA")) {
                    peopleTags.push(user.Name);
                } else {
                    nonqa.push(user.Name);
                }
            });

            _.each(nonqa, function(user) {
                peopleTags.push(user);
            });
        });

        this.init = function () {
            $input = $('<input type="hidden" id="quailEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: peopleTags,
                openOnEnter: false,
                multiple: true,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }
    
    function DesignerEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;
        var people = [];
        var peopleTags = [];
        $.get('/api/v1/people', function (data) {
            people = data["accounts"];
            people = _.sortBy(people, function (user) { return user.Name; });
            var nondesigner = [];
            peopleTags.push("None");
            _.each(people, function (user) {
                if (_.contains(user.Groups, "Design")) {
                    peopleTags.push(user.Name);
                } else {
                    nondesigner.push(user.Name);
                }
            });

            _.each(nondesigner, function (user) {
                peopleTags.push(user);
            });
        });

        this.init = function () {
            $input = $('<input type="hidden" id="quailEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: peopleTags,
                openOnEnter: false,
                multiple: true,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }
    
    function DevEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;
        var people = [];
        var peopleTags = [];
        $.get('/api/v1/people', function (data) {
            people = data["accounts"];
            people = _.sortBy(people, function (user) { return user.Name; });
            var nondev = [];
            peopleTags.push("None");
            _.each(people, function (user) {
                if (_.contains(user.Groups, "Developers")) {
                    peopleTags.push(user.Name);
                } else {
                    nondev.push(user.Name);
                }
            });

            _.each(nondev, function (user) {
                peopleTags.push(user);
            });
        });

        this.init = function () {
            $input = $('<input type="hidden" id="devEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: peopleTags,
                openOnEnter: false,
                multiple: true,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }
    
    function ProjectManagerEditor(args) {
        var $input;
        var defaultValue;
        var scope = this;
        var people = [];
        var peopleTags = [];
        $.get('/api/v1/people', function (data) {
            people = data["accounts"];
            people = _.sortBy(people, function (user) { return user.Name; });
            var nonpm = [];
            peopleTags.push("None");
            _.each(people, function (user) {
                if (_.contains(user.Groups, "PM")) {
                    peopleTags.push(user.Name);
                } else {
                    nondev.push(user.Name);
                }
            });

            _.each(nonpm, function (user) {
                peopleTags.push(user);
            });
        });

        this.init = function () {
            $input = $('<input type="hidden" id="devEditor" style="width:300px" />')
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                });
            $input.select2({
                tags: peopleTags,
                openOnEnter: false,
                multiple: true,
            })
            .focus()
            .select();
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }

    function HudlDateEditor(args) {
        this.init = function () {
            $input = $("<INPUT type=text class='editor-text' />")
                .appendTo(args.container)
                .bind("keydown.nav", function (e) {
                    if (e.keyCode === $.ui.keyCode.LEFT || e.keyCode === $.ui.keyCode.RIGHT) {
                        e.stopImmediatePropagation();
                    }
                })
                .focus()
                .select();
            toastr.warning('The format of this must be exactly<br />YYYY-MM-DDTHH:MM:SS.SSSZ<br />with the "T" and "Z" always in those spots.<br />Also the "updated" text might look weird for this.', 'Be careful');
        };

        this.destroy = function () {
            $input.remove();
        };

        this.focus = function () {
            $input.focus();
        };

        this.getValue = function () {
            return $input.val();
        };

        this.setValue = function (val) {
            $input.val(val);
        };

        this.loadValue = function (item) {
            defaultValue = item[args.column.field] || "";
            $input.val(defaultValue);
            $input[0].defaultValue = defaultValue;
            $input.select();
        };

        this.serializeValue = function () {
            return $input.val();
        };

        this.applyValue = function (item, state) {
            item[args.column.field] = state;
        };

        this.isValueChanged = function () {
            return (!($input.val() == "" && defaultValue == null)) && ($input.val() != defaultValue);
        };

        this.validate = function () {
            if (args.column.validator) {
                var validationResults = args.column.validator($input.val());
                if (!validationResults.valid) {
                    return validationResults;
                }
            }

            var valid = $input.val().length > 0;
            if (!valid) warnFieldIsEmpty();

            return {
                valid: valid,
                msg: null
            };
        };

        this.init();
    }

    function warnFieldIsEmpty() {
        toastr.error('Field is empty, you can\'t save it unless it has a value. Put "None" or "N/A" if that\'s the case. Or hit "Esc" to leave the old value.');
    }
})(jQuery);
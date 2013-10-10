(function (blackmesa, $) {

    $(function () {
        toastr.options = {
            "debug": false,
            "fadeIn": 300,
            "fadeOut": 500,
            "timeOut": 2000,
            "extendedTimeOut": 1000
        }
        $('#load-all').click(function () {
            $('#row-loading-spinner').show();
            loadData(true);
        });
        loadData(false);
    });

    _.each(BlackMesa.config.basecampThreads, function (basecampThread, index) {
        $('#basecamp-links').append('<a href="' + basecampThread.url + '" target="_blank">' + basecampThread.name + '</a><br />');
        $('#basecampThread').append($("<option></option>")
                             .attr("value", basecampThread.threadId)
                             .text(basecampThread.name));
    });

    $('#hotfix-thread').append('<a href="' + BlackMesa.config.hotfixThread + '" target="_blank">Post Here As Well!</a>');

    Slick.Formatters = _.extend(Slick.Formatters || {}, {
        DateTimeFormatter: function(row, cell, value, columnDef, dataContext) {
            return $.format.date(new Date(value), columnDef.dateFormat);
        },
        JiraFormatter: function(row, cell, value, columnDef, dataContext) {
            var jiraUrl = columnDef.jiraBaseUrl + encodeURIComponent('labels = ' + value);
            return '<a href="' + jiraUrl + '" target="_blank"><i class="icon-share-alt"></i></a>';
        },
        PullRequestFormatter: function(row, cell, value, columnDef, dataContext) {
            if (!value) {
                return '';
            }
            return '<a href="' + columnDef.githubBaseUrl + value.toString() + '" target="_blank">' + value.toString() + '<i class="icon-share-alt"></i></a>';
        },
        TypeFormatter: function (row, cell, value, columnDef, dataContext) {
            if (value == "Hotfix") {
                return '<a href="#" class="hotfix" data-mongo-id="' + dataContext.id + '">' + value + '<i class="icon-share-alt"></i></a>';
            }
            return value;
        },
    });
    
    function loadData(loadAll) {
        $.getJSON('/api/v1/deploys' + (loadAll ? '/all' : '')).then(function (data) {
            if (loadAll) {
                $('#load-all').text("Loaded all rows in Mongo");
                $('#row-loading-spinner').hide();
            }

            $.each(data.items, function (index, item) {
                if (!item.people) return;
                if (item.people["quails"])
                    item.qa = item.people["quails"].join(',');
                if (item.people["developers"])
                    item.dev = item.people["developers"].join(',');
                if (item.people["designers"])
                    item.design = item.people["designers"].join(',');
                if (item.people["projectManagers"])
                    item.pm = item.people["projectManagers"].join(',');
                if (item.people["codeReviewers"])
                    item.codeReview = item.people["codeReviewers"].join(',');
            });

            // If we have them all loaded we really need to add the year
            var dateFormatter = "M/d/yyyy HH:mm";

            var columns = [
                { id: 'Day', name: 'Day', field: 'day', width: 35 },
                { id: 'DeployTime', name: 'Time', field: 'deployTime', width: 90, editor: Slick.Editors.HudlDateEditor, formatter: Slick.Formatters.DateTimeFormatter, dateFormat: dateFormatter, sortable: true },
                { id: 'Action', name: 'Action', field: 'action', width: 50, editor: Slick.Editors.Action, sortable: true },
                { id: 'Component', name: 'Comp.', field: 'component', width: 60, editor: Slick.Editors.Component, sortable: true },
                { id: 'Type', name: 'Type', field: 'type', width: 90, formatter: Slick.Formatters.TypeFormatter, editor: Slick.Editors.Type, sortable: true },
                { id: 'Project', name: 'Proj.', field: 'project', width: 90, editor: Slick.Editors.Project, sortable: true },
                { id: 'Branch', name: 'Branch', field: 'branch', width: 270, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'PullRequestId', name: 'PR', field: 'pullRequestId', width: 50, formatter: Slick.Formatters.PullRequestFormatter, githubBaseUrl: blackmesa.config.gitHubPullRequestBaseUrl, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'JiraLabel', name: 'Jira', field: 'branch', width: 33, formatter: Slick.Formatters.JiraFormatter, jiraBaseUrl: blackmesa.config.jiraSearchByLabelBaseUrl, editor: Slick.Editors.Jira, sortable: true },
                { id: 'Quails', name: 'QA', field: 'qa', width: 100, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'Designers', name: 'DES', field: 'design', width: 100, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'Developers', name: 'DEV', field: 'dev', width: 100, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'CodeReviewers', name: 'CR', field: 'codeReview', width: 100, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'ProjectManagers', name: 'PM', field: 'pm', width: 100, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'Notes', name: 'Notes', field: 'notes', width: 450, editor: Slick.Editors.NoBlankTextEditor, sortable: true },
                { id: 'Delete', name: 'Delete', width: 50, formatter: buttonFormatter }
            ];
            
            function buttonFormatter(row, cell, value, columnDef, dataContext) {
                var button = "<input class='btn btn-danger del' type='button' id='" + dataContext.id + "' />";
                return button;
            }

            $('.del').live('click', function() {
                var id = $(this).attr('id');

                $.ajax({
                    url: 'api/v1/deploys/' + id,
                    type: 'DELETE'
                });
                dataView.deleteItem(id);
                grid.invalidate();
            });

            var dataView = new Slick.Data.DataView();

            var grid = new Slick.Grid('#deploys', dataView, columns, {
                editable: true,
                autoEdit: false,
                asyncEditorLoading: false,
                enableColumnReorder: false,
                enableCellNavigation: true,
                multiColumnSort: true,
                syncColumnCellResize: true,
            });

            grid.onSort.subscribe(function (e, args) {
                var cols = args.sortCols;

                dataView.sort(function (dataRow1, dataRow2) {
                    for (var i = 0, l = cols.length; i < l; i++) {
                        var field = cols[i].sortCol.field;
                        var sign = cols[i].sortAsc ? 1 : -1;
                        var value1 = dataRow1[field], value2 = dataRow2[field];
                        var result = (value1 == value2 ? 0 : (value1 > value2 ? 1 : -1)) * sign;
                        if (result != 0) {
                            return result;
                        }
                    }
                    return 0;
                });
                grid.invalidate();
                grid.render();
            });

            grid.setSelectionModel(new Slick.CellSelectionModel());

            dataView.onRowCountChanged.subscribe(function (e, args) {
                grid.updateRowCount();
                grid.render();
            });
            dataView.onRowsChanged.subscribe(function (e, args) {
                grid.invalidateRows(args.rows);
                grid.render();
            });
            grid.onSelectedRowsChanged.subscribe(function(e, args) {
                grid.render();
            });

            var oldValue;
            grid.onBeforeEditCell.subscribe(function (e, args) {
                oldValue = {
                    value: $(grid.getCellNode(args.row, args.cell)).text(),
                    type: grid.getColumns()[args.cell].id,
                    id: args.item.id
                }
            });
            grid.onCellChange.subscribe(function (e, args) {
                BlackMesa.hotfix().flattenHotfixData(args.item, grid.getData().getItemById(args.item.id));
                $.post("/api/v1/deploys", args.item, function (data) {
                    var confirmedOldValue = "";
                    if (oldValue.id === args.item.id && oldValue.type == grid.getColumns()[args.cell].id) {
                        confirmedOldValue = oldValue.value;
                    }
                    var newValue = grid.getData().getItemById(args.item.id)[grid.getColumns()[args.cell].field];
                    toastr.info('Updated ' + grid.getColumns()[args.cell].id + " from " + confirmedOldValue + " to " + newValue);
                    $.post("api/v1/history", {
                        person: BlackMesa.username,
                        deployId: args.item.id,
                        propertyChanged: grid.getColumns()[args.cell].id,
                        newValue: newValue,
                        oldValue: confirmedOldValue
                    });
                });
            });
            grid.registerPlugin(new Slick.AutoTooltips());

            dataView.beginUpdate();
            dataView.setItems(data.items, 'id');
            dataView.endUpdate();

            var selecteditemid = /\/([0-9a-f]+)/.exec(window.location.href);
            if (selecteditemid !== null && selecteditemid.length > 0) {
                var selectedrowindex = dataView.getIdxById(selecteditemid[1]);
                grid.setSelectedRows([selectedrowindex]);
            }

            window.grid = grid;
        });
    };
})(window.BlackMesa, jQuery);
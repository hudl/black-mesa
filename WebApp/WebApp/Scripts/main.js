(function (blackmesa, $) {

    $(function () {
        toastr.options = {
            "debug": false,
            "fadeIn": 300,
            "fadeOut": 500,
            "timeOut": 2000,
            "extendedTimeOut": 1000
        }
    });

    Slick.Formatters = _.extend(Slick.Formatters || {}, {
        DateTimeFormatter: function(row, cell, value, columnDef, dataContext) {
            return $.format.date(new Date(value), columnDef.dateFormat);
        },
        JiraFormatter: function(row, cell, value, columnDef, dataContext) {
            var jiraUrl = columnDef.jiraBaseUrl + encodeURIComponent('labels = ' + value);
            return '<a href="' + jiraUrl + '" target="_blank">tix<i class="icon-share-alt"></i></a>';
        },
        PeopleFormatter: function(row, cell, value, columnDef, dataContext) {
            if (!value) {
                return '';
            }
            var people = value[columnDef.peopleKey];
            return (people.length) ? people[0] : 0;
        },
        PullRequestFormatter: function(row, cell, value, columnDef, dataContext) {
            if (!value) {
                return '';
            }
            return '<a href="' + columnDef.githubBaseUrl + value.toString() + '" target="_blank">' + value.toString() + '<i class="icon-share-alt"></i></a>';
        }
    });
    
    $(function () {
        $.getJSON('/api/v1/deploys').then(function (data) {
            var columns = [
                { id: 'Day', name: 'Day', field: 'Day', width: 35 },
                { id: 'DeployTime', name: 'Time', field: 'DeployTime', formatter: Slick.Formatters.DateTimeFormatter, dateFormat: "M/d HH:mm", width: 80 },
                { id: 'Action', name: 'Action', field: 'Action', width: 60, editor: Slick.Editors.Action },
                { id: 'Component', name: 'Comp.', field: 'Component', width: 50, editor: Slick.Editors.Component },
                { id: 'Type', name: 'Type', field: 'Type', width: 90, editor: Slick.Editors.Type },
                { id: 'Project', name: 'Proj.', field: 'Project', width: 50, editor: Slick.Editors.Project },
                { id: 'Branch', name: 'Branch', field: 'Branch', width: 150, editor: Slick.Editors.Text },
                { id: 'PullRequestId', name: 'PR', field: 'PullRequestId', width: 60, formatter: Slick.Formatters.PullRequestFormatter, githubBaseUrl: blackmesa.config.gitHubPullRequestBaseUrl, editor: Slick.Editors.Text },
                { id: 'JiraLabel', name: 'Jira', field: 'Branch', width: 50, formatter: Slick.Formatters.JiraFormatter, jiraBaseUrl: blackmesa.config.jiraSearchByLabelBaseUrl, editor: Slick.Editors.Jira },
                { id: 'Quails', name: 'QA', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'Quails', width: 70, editor: Slick.Editors.People },
                { id: 'Designers', name: 'DES', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'Designers', width: 70, editor: Slick.Editors.People },
                { id: 'Developers', name: 'DEV', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'Developers', width: 70, editor: Slick.Editors.People },
                { id: 'CodeReviewers', name: 'CR', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'CodeReviewers', width: 70, editor: Slick.Editors.People },
                { id: 'Notes', name: 'Notes', field: 'Notes', width: 249, editor: Slick.Editors.Text }
            ];
            var dataView = new Slick.Data.DataView();

            var grid = new Slick.Grid('#deploys', dataView, columns, {
                editable: true,
                autoEdit: false,
                asyncEditorLoading: false,
                enableColumnReorder: false,
                enableCellNavigation: true
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
            grid.onCellChange.subscribe(function (e, args) {
                $.post("/api/v1/deploys", args.item, function (data) {
                    toastr.info('Updated ' + grid.getColumns()[args.cell].field + " to " + $(grid.getCellNode(args.row, args.cell)).text());
                });
            });
            
            dataView.beginUpdate();
            dataView.setItems(data.Items, 'Id');
            dataView.endUpdate();

            var selecteditemid = /\/([0-9a-f]+)/.exec(window.location.href);
            if (selecteditemid !== null && selecteditemid.length > 0) {
                var selectedrowindex = dataview.getidxbyid(selecteditemid[1]);
                grid.setselectedrows([selectedrowindex]);
            }

            window.grid = grid;
        });
    });
})(window.BlackMesa, jQuery);
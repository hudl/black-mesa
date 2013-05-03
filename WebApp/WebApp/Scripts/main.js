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
            loadData(true);
        });
        loadData(false);
    });

    Slick.Formatters = _.extend(Slick.Formatters || {}, {
        DateTimeFormatter: function(row, cell, value, columnDef, dataContext) {
            return $.format.date(new Date(value), columnDef.dateFormat);
        },
        JiraFormatter: function(row, cell, value, columnDef, dataContext) {
            var jiraUrl = columnDef.jiraBaseUrl + encodeURIComponent('labels = ' + value);
            return '<a href="' + jiraUrl + '" target="_blank">tix<i class="icon-share-alt"></i></a>';
        },
        PullRequestFormatter: function(row, cell, value, columnDef, dataContext) {
            if (!value) {
                return '';
            }
            return '<a href="' + columnDef.githubBaseUrl + value.toString() + '" target="_blank">' + value.toString() + '<i class="icon-share-alt"></i></a>';
        },
    });
    
    function loadData(loadAll) {
        $.getJSON('/api/v1/deploys' + (loadAll ? '/all' : '')).then(function (data) {
            if (loadAll) {
                $('#load-all').text("You suck, but I did it for you, but just this once.");
            }

            $.each(data.items, function (index, item) {
                item.qa = item.people["quails"].join(','),
                item.dev = item.people["developers"].join(','),
                item.design = item.people["designers"].join(','),
                item.pm = item.people["projectManagers"].join(','),
                item.codeReview = item.people["codeReviewers"].join(',')
            });

            // If we have them all loaded we really need to add the year
            var dateFormatter = loadAll ? "M/d/yy HH:mm" : "M/d HH:mm";

            var columns = [
                { id: 'Day', name: 'Day', field: 'day', width: 35 },
                { id: 'DeployTime', name: 'Time', field: 'deployTime', formatter: Slick.Formatters.DateTimeFormatter, dateFormat: dateFormatter, width: 80 },
                { id: 'Action', name: 'Action', field: 'action', width: 60, editor: Slick.Editors.Action },
                { id: 'Component', name: 'Comp.', field: 'component', width: 60, editor: Slick.Editors.Component },
                { id: 'Type', name: 'Type', field: 'type', width: 100, editor: Slick.Editors.Type },
                { id: 'Project', name: 'Proj.', field: 'project', width: 100, editor: Slick.Editors.Project },
                { id: 'Branch', name: 'Branch', field: 'branch', width: 150, editor: Slick.Editors.Text },
                { id: 'PullRequestId', name: 'PR', field: 'pullRequestId', width: 60, formatter: Slick.Formatters.PullRequestFormatter, githubBaseUrl: blackmesa.config.gitHubPullRequestBaseUrl, editor: Slick.Editors.Text },
                { id: 'JiraLabel', name: 'Jira', field: 'branch', width: 50, formatter: Slick.Formatters.JiraFormatter, jiraBaseUrl: blackmesa.config.jiraSearchByLabelBaseUrl, editor: Slick.Editors.Jira },
                { id: 'Quails', name: 'QA', field: 'qa', width: 100, editor: Slick.Editors.Text },
                { id: 'Designers', name: 'DES', field: 'design', width: 100, editor: Slick.Editors.Text },
                { id: 'Developers', name: 'DEV', field: 'dev', width: 100, editor: Slick.Editors.Text },
                { id: 'CodeReviewers', name: 'CR', field: 'codeReview', width: 100, editor: Slick.Editors.Text },
                { id: 'ProjectManagers', name: 'PM', field: 'pm', width: 100, editor: Slick.Editors.Text },
                { id: 'Notes', name: 'Notes', field: 'notes', width: 259, editor: Slick.Editors.Text }
            ];
            var dataView = new Slick.Data.DataView();

            var grid = new Slick.Grid('#deploys', dataView, columns, {
                editable: true,
                autoEdit: false,
                asyncEditorLoading: false,
                enableColumnReorder: false,
                enableCellNavigation: true,
                forceFitColumns: true
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
                    toastr.info('Updated ' + grid.getColumns()[args.cell].id + " to " + $(grid.getCellNode(args.row, args.cell)).text());
                });
            });
            grid.registerPlugin(new Slick.AutoTooltips());

            dataView.beginUpdate();
            dataView.setItems(data.items, 'id');
            dataView.endUpdate();

            var selecteditemid = /\/([0-9a-f]+)/.exec(window.location.href);
            if (selecteditemid !== null && selecteditemid.length > 0) {
                var selectedrowindex = dataview.getidxbyid(selecteditemid[1]);
                grid.setselectedrows([selectedrowindex]);
            }

            window.grid = grid;
        });
    };
})(window.BlackMesa, jQuery);
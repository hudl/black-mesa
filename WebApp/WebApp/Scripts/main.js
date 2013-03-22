(function (blackmesa, $) {
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
                { id: 'Action', name: 'Action', field: 'Action', width: 60 },
                { id: 'Component', name: 'Comp.', field: 'Component', width: 50 },
                { id: 'Type', name: 'Type', field: 'Type', width: 80 },
                { id: 'Project', name: 'Proj.', field: 'Project', width: 50 },
                { id: 'Branch', name: 'Branch', field: 'Branch', width: 150 },
                { id: 'PullRequestId', name: 'PR', field: 'PullRequestId', width: 40, formatter: Slick.Formatters.PullRequestFormatter, githubBaseUrl: blackmesa.config.gitHubPullRequestBaseUrl },
                { id: 'JiraLabel', name: 'Jira', field: 'JiraLabel', width: 40, formatter: Slick.Formatters.JiraFormatter, jiraBaseUrl: blackmesa.config.jiraBaseUrl },
                { id: 'QualityAnalysts', name: 'QA', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'QualityAnalysts', width: 80 },
                { id: 'Designers', name: 'DES', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'Designers', width: 80 },
                { id: 'Developers', name: 'DEV', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'Developers', width: 80 },
                { id: 'CodeReviewers', name: 'CR', field: 'People', formatter: Slick.Formatters.PeopleFormatter, peopleKey: 'CodeReviewers', width: 80 },
                { id: 'Notes', name: 'Notes', field: 'Notes', width: 260 }
            ];
            var dataView = new Slick.Data.DataView();
            var grid = new Slick.Grid('#deploys', dataView, columns, {
                enableColumnReorder: false,
                enableCellNavigation: true
            });
            grid.setSelectionModel(new Slick.RowSelectionModel());
            
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
            
            dataView.beginUpdate();
            dataView.setItems(data.Items, 'Id');
            dataView.endUpdate();

            var selectedItemId = /\/([0-9a-f]+)/.exec(window.location.href);
            if (selectedItemId !== null && selectedItemId.length > 0) {
                var selectedRowIndex = dataView.getIdxById(selectedItemId[1]);
                grid.setSelectedRows([selectedRowIndex]);
            }

            window.grid = grid;
        });
    });
})(window.BlackMesa, jQuery);
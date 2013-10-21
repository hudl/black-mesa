(function ($) {

    var historyItem = '<div class="history-item">Person: {Person}<br />Property: {Property}<br />Old Value: {OldValue}<br />New Value: {NewValue}<br /><a href="#" class="row-finder" data-row-id="{DeployId}">Take Me There</a></div><br />';

    Slick.Formatters = _.extend(Slick.Formatters || {}, {
        DateTimeFormatter: function (row, cell, value, columnDef, dataContext) {
            return $.format.date(new Date(value), columnDef.dateFormat);
        }
    });

    $(document).on('ready', function () {
        var historyDiv = $('#history');
        historyDiv.show();
        $('#load-all').click(function() {
            $('#row-loading-spinner').show();
            loadData(true);
        });
        loadData(false);

        function loadData(loadAll) {
            $.get('api/v1/history' + (loadAll ? '/all' : '')).then(function (data) {
                if (loadAll) {
                    $('#load-all').text('Loaded all rows in Mongo');
                    $('#row-loading-spinner').hide();
                }

                var dateFormatter = "M/d/yyyy HH:mm";

                var columns = [
                    { id: 'DeployTime', name: 'Time', field: 'dateTime', width: 90, formatter: Slick.Formatters.DateTimeFormatter, dateFormat: dateFormatter, sortable: true },
                    { id: 'Person', name: 'Person', field: 'person', width: 190, sortable: true },
                    { id: 'Property', name: 'Property', field: 'propertyChanged', width: 150, sortable: true },
                    { id: 'OldValue', name: 'Old Value.', field: 'oldValue', width: 160, sortable: true },
                    { id: 'NewValue', name: 'New Value', field: 'newValue', width: 190, sortable: true },
                    { id: 'Link', name: 'Link', field: 'deployId', width: 190, sortable: true },
                ];

                var dataView = new Slick.Data.DataView();

                var grid = new Slick.Grid('#history', dataView, columns, {
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
                grid.onSelectedRowsChanged.subscribe(function (e, args) {
                    grid.render();
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




                _.each(data.items, function (history) {
                    historyDiv.append(historyItem.replace("{Person}", history.person)
                                                    .replace("{Property}", history.propertyChanged)
                                                    .replace("{OldValue}", history.oldValue)
                                                    .replace("{NewValue}", history.newValue)
                                                    .replace("{DeployId}", history.deployId));
                });
            });
        }
    });

})(jQuery);
$.fn.createTable = function (options) {
    var me = $(this);
    var table = '<table class="table table-bordered table-condensed">';
    var th = '<tr>';
    options.columns.forEach(function (value, index) {
        var hidden = value['hidden'] ? 'none' : 'table-cell';
        th += '<th style="display:' +hidden           
            + '">' + value['title'] + '</th>';   
    });
    th += '</tr>';
    table += th;
    options.data.forEach(function (value) {
       var tr = '<tr>';
        options.columns.forEach(function (column) {
            var hidden = column['hidden'] ? 'none' : 'table-cell';
            tr += '<td  style="display:' + hidden +
               '">' + value[column['field']] + '</td>';
        });
        tr += '</tr>';
        table += tr;
    });
    table += '</table>';
    console.log(table);
    me.append(table);
};

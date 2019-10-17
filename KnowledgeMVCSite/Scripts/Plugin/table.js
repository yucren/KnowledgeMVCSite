$.fn.createTable = function (options) {
    var me = $(this);
    var a = 10;
    var table = '<table class="table table-bordered table-condensed table-hover">';    
    var th = '<tr class="popover-title">';
  
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
    if (options.caption !== undefined) {
        var caption = '<caption class="caption">' + options.caption + '</caption>';
        table += caption;
    }    
    table += '</table>';
    var pagination = '<ul class="pagination pagination-lg">';
    if (options.pagination !== undefined) {
        
        pagination += '<li onclick="action.prepage()"><a href="#" >&laquo;</a></li>';
        pagination += '<li onclick="action.nextpage()"><a href="#" >&raquo;</a></li>';
        table += pagination;
    }
   
   
    console.log(table);
    me.append(table);
};

var action = {
    prepage: function () {
        alert("上一页" );
    },
    nextpage: function () {
        alert("下一页");
    }
};
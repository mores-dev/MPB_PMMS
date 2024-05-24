$(document).ready(function () {
    $('#btn_page_first').button({
        icons: { primary: 'ui-icon-arrowthickstop-1-w' },
        label: '第一頁',
        text: false
    }).bind('click', function (event) { event.stopPropagation(); changePage(pagelist_go_page_first, 0) });
    $('#btn_page_prev').button({
        icons: { primary: 'ui-icon-arrowthick-1-w' },
        label: '上一頁',
        text: false
    }).bind('click', function (event) { event.stopPropagation(); changePage(pagelist_go_page_prev, 0) });
    $('#btn_page_next').button({
        icons: { primary: 'ui-icon-arrowthick-1-e' },
        label: '下一頁',
        text: false
    }).bind('click', function (event) { event.stopPropagation(); changePage(pagelist_go_page_next, 0) });
    $('#btn_page_last').button({
        icons: { primary: 'ui-icon-arrowthickstop-1-e' },
        label: '最末頁',
        text: false
    }).bind('click', function (event) { event.stopPropagation(); changePage(pagelist_go_page_last, 1) });
    $('#ToPage').filter('input:text').bind('keyup', function (event) { if (event.which == 13) { event.stopPropagation(); changePage(this.value, 0); } });
    $('#ToPage').filter('select').bind('change', function (event) { changePage(this.value, 0); });
});

function changePage(ToPage, isLastPage) {//分頁用FUNCTION
    $.vs_dialogStart({ dialogMassage: '請稍候...' });
    $('#ToPage').val(ToPage);
    $('#isLastPage').val(isLastPage);
    $('form:first').vs_doSubmit({ actionPage: queryPage, targetWin: '_self', postMethod: 'post' });
}

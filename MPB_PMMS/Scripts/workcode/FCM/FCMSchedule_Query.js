﻿var HtColSet;
var Ht;
var CompanyName = null;
$(document).ready(function () {
    //$('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btQuery").on("click", function (event) {
        $.vs_dialogStart({ dialogMassage: "請稍候..." });
        $('form:first').vs_doSubmit({ actionPage: queryPage, targetWin: "_self", postMethod: "post" });
    });

    $("#btAdd").on("click", function (event) { doAct(addPage, "A"); });

    if ($("#isAdmin").val() == "1") { CompanyName = "航商名稱"; }

    // ===== Chosen Example =====
    $(".chosen-select").chosen({
        allow_single_deselect: true,            //可清除選取
        disable_search_threshold: 10,           //拿掉搜尋列
        search_contains: true,                   //不需從頭相同
        no_results_text: "查無結果!"           //顯示查無資料訊息
    });

    //===== handsontable Example =====
    //HtColSet  header: null,  隱藏欄位  (Hidden)
    HtColSet = [
        //{
        //    header: "檢視",
        //    width: 30,
        //    data: "BQ",
        //    type: "btn",
        //    click_function: function (event) { doAct(modifyPage, "V"); },
        //},
        {
            header: CompanyName,
            data: "C_NAME",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 40 },
        },
        {
            header: "航班代號",
            data: "SC_CODE",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "航班名稱",
            data: "SC_NAME",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 40 },
        },
        {
            header: "航班時間",
            data: "SC_TIME",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "套用航線",
            data: "R_NAME",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "預估航行時間(分)",
            data: "TravelTime",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "狀態",
            data: "SC_STATUS",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "管理",
            width: 70,
            data: "BM",
            type: "btn",
            click_function: function (event) { doAct(modifyPage, "M"); },
        },
        //{
        //    header: "刪除",
        //    width: 70,
        //    data: "BK",
        //    type: "btn",
        //    click_function: function (event) { doAct(modifyPage, "D"); },
        //},
        {
            header: null,
            data: "SC_ID",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
            submit: 1
        },
        {
            header: null,
            data: "C_ID",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
            submit: 1
        },
    ];
    
    $("#tbResultsFCMSchedule").handsontable({     //set handsontable attribute
        data: HtData,
        contextMenu: false,                 //快捷清單(滑鼠右鍵)
        columnSorting: false,                //欄位排序
        colHeaders: $.map(HtColSet, function (t, i) { return t.header; }),
        maxRows: 20,                        //最大新增行數
        //manualColumnResize: true,          //可動態調整欄位大小
        manualRowResize: false,             //可動態調整行距大小
        persistentState: true,             //持久化狀態(true=記錄被異動過的值)
        minSpareRows: 0,                    //設定備用欄位
        className: "table_format htCenter ",  //htCenter  htLeft  htRight
        columns: $.grep(HtColSet, function (t, i) { return t.header; }),
        cells: function (row, col, prop) { return { readOnly: true }; },
        fillHandle: false,
        renderAllRows: true,
        afterRender: function () {
        }
    });

    Ht = $("#tbResultsFCMSchedule").handsontable('getInstance');
    //auto resize handsontable column width
    $.syncWin_WH.resizeFunctions.push({ func: handsontableResize, params: [$("#tbResultsFCMSchedule"), HtColSet] });//handsontableResize($("#tbResultsAuthUser"), HtColSet)
    $("body").removeClass("noVisibility");

});

function doAct(pPage, pMod) {
    $('#currentUrl').val(self.location.href);
    $('#mode').val(pMod);
    if (pMod == "V" || pMod == "M" || pMod == "A" || pMod == "D" || pMod == "C") {
        if (pMod != "A") {
            $('#mod_key').val(get_ht_submit_string("single", Ht, HtColSet));
        }
        $('form:first').vs_doSubmit({ actionPage: pPage, targetWin: "_self", postMethod: "post" });
    } else if (!$('.btn_chk,.btn_opt').hasClass("ht_checked_row")) {
        alert("請先選取要處理的資料!");
        return false;
    } else {
        //多筆列印
        $('#mod_key').val(get_ht_submit_string("multi", Ht, HtColSet));
        $('form:first').vs_doSubmit({ actionPage: pPage, targetWin: "_blank", postMethod: "post" });
    }
}
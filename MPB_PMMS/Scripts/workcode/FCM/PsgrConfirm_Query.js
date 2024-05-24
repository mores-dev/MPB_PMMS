var HtColSet;
var Ht;
$(document).ready(function () {
    //$('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btQuery").on("click", function (event) {
        $.vs_dialogStart({ dialogMassage: "請稍候..." });
        $('form:first').vs_doSubmit({ actionPage: queryPage, targetWin: "_self", postMethod: "post" });
    });

    $("#btMod").on("click", function (event) { if (doCheck()) { doAct(modifyPage, "M"); }});

    // ===== Chosen Example =====
    $(".chosen-select").chosen({
        allow_single_deselect: false,            //可清除選取
        disable_search_threshold: 10,           //拿掉搜尋列
        search_contains: true,                   //不需從頭相同
        no_results_text: "查無結果!"           //顯示查無資料訊息
    });

    //===== handsontable Example =====
    //HtColSet  header: null,  隱藏欄位  (Hidden)
    HtColSet = [
        {
            header: '航商名稱',
            data: "CName",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 40 },
        },
        {
            header: "航班名稱",
            data: "RName",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "船名",
            data: "VesselName",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 40 },
        },
        {
            header: "乘客姓名",
            data: "PsgrName",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "證件別",
            data: "IdType",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "證件號碼",
            data: "IdNo",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "出生日期",
            data: "Birth",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: null,
            data: "C_ID",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
            submit: 1,
        },
        {
            header: null,
            data: "Station",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
            submit: 1,
        },
        {
            header: null,
            data: "VoyageTime",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
            submit: 1,
        },
        {
            header: null,
            data: "VesselId",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
            submit: 1,
        },
    ];
    
    $("#tbResultsPsgrConfirm").handsontable({     //set handsontable attribute
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

    Ht = $("#tbResultsPsgrConfirm").handsontable('getInstance');
    //auto resize handsontable column width
    $.syncWin_WH.resizeFunctions.push({ func: handsontableResize, params: [$("#tbResultsPsgrConfirm"), HtColSet] });//handsontableResize($("#tbResultsAuthUser"), HtColSet)
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

function doCheck() {
    if (Ht.countRows() > 0) {
        Ht.selectCell(0, 1);
        return true;
    } else {
        alert("請查詢航班資料!");
        return false;
    }
}
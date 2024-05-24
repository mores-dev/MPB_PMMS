var HtColSet;
var Ht;
$(document).ready(function () {
    //$('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btQuery").on("click", function (event) {
        $("#Upload").val("1");
        $.vs_dialogStart({ dialogMassage: "請稍候..." });
        $('form:first').vs_doSubmit({ actionPage: queryPage, targetWin: "_self", postMethod: "post" });
    });

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
            header: "登錄人數",
            data: "PassengerCnt",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "上傳雲端平台狀態",
            data: "UploadSts",
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
    ];
    
    $("#tbResultsPsgrUpload").handsontable({     //set handsontable attribute
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

    Ht = $("#tbResultsPsgrUpload").handsontable('getInstance');
    //auto resize handsontable column width
    $.syncWin_WH.resizeFunctions.push({ func: handsontableResize, params: [$("#tbResultsPsgrUpload"), HtColSet] });//handsontableResize($("#tbResultsAuthUser"), HtColSet)
    $("body").removeClass("noVisibility");

});

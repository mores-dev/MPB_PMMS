var HtColSet;
var Ht;
$(document).ready(function () {
    $('.date-picker').datepicker();
    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);

    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btQuery").on("click", function (event) {
        if (doCheck())
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
            header: "操作時間",
            data: "LogDt",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: null,
            data: "LogSys",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "使用者",
            data: "Account",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "類別",
            data: "LogType",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "程式名稱",
            data: "ProgName",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "IP",
            data: "Ip",
            type: 'text',
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
    ];

    $("#tbResultsAuthRecord").handsontable({     //set handsontable attribute
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

    Ht = $("#tbResultsAuthRecord").handsontable('getInstance');
    //auto resize handsontable column width
    $.syncWin_WH.resizeFunctions.push({ func: handsontableResize, params: [$("#tbResultsAuthRecord"), HtColSet] });//handsontableResize($("#tbResultsAuthRecord"), HtColSet)
    //$("body").removeClass("noVisibility");

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
    if ($.active > 0) {
        alert("背景資料正在處理,請稍後");
        return false;
    } else {
        $('[class*=vs_formValidate]').vs_formValidate();
        //自訂規則
        //$("#ID").vs_showDivAlert({ append: true, alertMassage: "MESSAGE" });//自訂alert
        //自訂規則
        if ($("#LogOn").val().length > 0 && $("#LogOff").val().length > 0) {
            if (Date.parse($("#LogOn").val()).valueOf() > Date.parse($("#LogOff").val()).valueOf()) {
                //判斷開始時間是否小於結束時間
                $("#LogOff").vs_showDivAlert({ alertMassage: "迄日不可大於起日" });//自訂alert
                $("#LogOn").vs_showDivAlert({ alertMassage: "迄日不可大於起日" });//自訂alert
            } else {
                var sdt = new Date($("#LogOn").val());
                var edt = new Date($("#LogOff").val());
                if (sdt.dateDiff("d", edt) > 31) {
                    //判斷日期區間是否大於31天
                    $("#LogOff").vs_showDivAlert({ alertMassage: "日期區間不可大於31天" });//自訂alert
                    $("#LogOn").vs_showDivAlert({ alertMassage: "日期區間不可大於31天" });//自訂alert
                }
            }
        }
        else {
            if ($("#LogOn").val().length == 0) $("#LogOn").vs_showDivAlert({ alertMassage: "查詢日期不可為空" });
            if ($("#LogOff").val().length == 0) $("#LogOff").vs_showDivAlert({ alertMassage: "查詢日期不可為空" });
        }


        //自訂規則
        if ($.vs_showDivAlert({ resetZindex: true, order: "desc" }) > 0) {//重設z-index回傳alertDiv總數量
            $("div.divAlert:first").vs_findDivAlertParent().vs_scroll_Focus({ showDivAlert: true });
            return false;
        }

    }
    return true;
}
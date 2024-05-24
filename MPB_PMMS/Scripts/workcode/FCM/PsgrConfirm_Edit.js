var HtColSetGrid1;
var HtSchemaGrid1;
var HtGrid1;

$(document).ready(function () {
    $('.date-picker').datepicker();
    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);

    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btnBack").on("click", function (event) { $.vs_toHref({ targetFrame: window.self, pageURL: $('#currentUrl').val() }); });

    $("#btnConfirm").on("click", doConfirm);

    // ===== Chosen Example =====
    $(".chosen-select").chosen({
        allow_single_deselect: false,            //可清除選取
        disable_search_threshold: 10,           //拿掉搜尋列
        search_contains: true,                   //不需從頭相同
        no_results_text: "查無結果!"           //顯示查無資料訊息
    });

    //===== handsontable  =====
    //HtColSet
    HtColSetGrid1 = [
        {
            header: "乘客姓名",
            data: "PsgrName",
            width: 200,
            type: "text",
            readOnly: true,
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "出生年月日",
            width: 200,
            data: "Birth",
            type: "text",
            readOnly: true,
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "證件別",
            width: 100,
            data: "IdType",
            type: "text",
            readOnly: true,
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "證件號碼",
            width: 150,
            data: "IdNo",
            type: 'text',
            readOnly: true,
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
        {
            header: "刪除",
            width: 50,
            data: "BK",
            type: "btn",
            click_function: function (event) { doDelete(); },
        },
        {
            header: null,
            data: "IdNoEncode",
            type: 'text',
            readOnly: true,
            className: "htCenter",
            vs_initialForm: { M: 10 },
        },
    ]

    $("#tbResults_PsgrConfirmGrid1").handsontable({     //set handsontable attribute
        data: HtDataGrid1,
        dataSchema: HtSchemaGrid1,
        preventOverflow: 'horizontal',
        contextMenu: false,                 //快捷清單(滑鼠右鍵)
        columnSorting: false,                //欄位排序
        colHeaders: $.map(HtColSetGrid1, function (t, i) { return t.header; }),
        minRows: 0,                        //最少顯示行數
        rowHeaders: true,                   //顯示列抬頭
        manualColumnResize: false,          //可動態調整欄位大小
        manualRowResize: false,             //可動態調整行距大小
        persistentState: false,             //持久化狀態(true=記錄被異動過的值)
        minSpareRows: 0,                    //設定備用欄位
        className: "table_format htCenter",
        columns: $.grep(HtColSetGrid1, function (t, i) { return t.header; }),
        fillHandle: false,
        renderAllRows: true,
        afterRender: function () {
        }
    });
    HtGrid1 = $("#tbResults_PsgrConfirmGrid1").handsontable('getInstance');
    $.syncWin_WH.resizeFunctions.push({ func: handsontableResize, params: [$("#tbResults_PsgrConfirmGrid1"), HtColSetGrid1] });

    $("body").removeClass("noVisibility");
});


function doConfirm(event) {
    $('#btnConfirm').vs_initialForm({ D: 1 });
    if (doCheck()) {
        doSave();
    }
    $('#btnConfirm').delay(1000, function () { $(this).vs_initialForm({ D: 0 }); });
}

function doCheck() {
    if ($.active > 0) {
        alert("背景資料正在處理,請稍後");
        return false;
    }
    if (isDeleteMode) {
        if (!confirm("是否確定刪除資料？")) {
            return false;
        }
    } else {
        $('[class*=vs_formValidate]').vs_formValidate();
        //自訂規則
        var idNo = $("#IdNo").val();
        if (idNo) {
            for (var i = 0; i < HtGrid1.countRows(); i++) {
                if (idNo == HtGrid1.getDataAtRowProp(i, "IdNo")) {
                    alert("證件號碼重複，請重新輸入!");
                    return false;
                }
            }
        }
        //$("#ID").vs_showDivAlert({ append: true, alertMassage: "MESSAGE" });//自訂alert
        //自訂規則
        if ($.vs_showDivAlert({ resetZindex: true, order: "desc" }) > 0) {//重設z-index回傳alertDiv總數量
            $("div.divAlert:first").vs_findDivAlertParent().vs_scroll_Focus({ showDivAlert: true });
            return false;
        }
    }
    return true;
}

function doSave() {
    var sData = {};
    sData.C_ID = $("#C_ID").val();
    sData.Station = $("#Station").val();
    sData.VoyageTime = $("#VoyageTime").val();
    sData.VesselId = $("#VesselId").val();
    sData.PsgrName = $("#PsgrName").val();
    sData.IdType = $("#IdType").val();
    sData.IdNo = $("#IdNo").val();
    sData.Birth = $("#Birth").val();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: sData,
        url: 'AjaxSaveData',
        timeout: 3000,
        success: function (response) {
            if (response.TotalRow == "1") {//insert success
                //add data into grid
                HtGrid1.alter("insert_row", 0);
                HtGrid1.setDataAtCell(0, 0, sData.PsgrName);
                HtGrid1.setDataAtCell(0, 1, sData.Birth);
                HtGrid1.setDataAtCell(0, 2, $("#IdType :selected").text());
                HtGrid1.setDataAtCell(0, 3, response.IdNo);
                HtGrid1.setDataAtRowProp(0, "IdNoEncode", response.IdNoEncode);

                HtGrid1.render();
                clearInput();
            } else { // insert failed
                if (response.ErrMsg)
                    alert(response.ErrMsg);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (textStatus == "timeout") {
                textStatus += "=" + this.timeout;
            }
            $('#PsgrName').vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + ")" });
            $('#PsgrName').delay(9, function () { $(this).trigger("focus"); });
        }
    });
}

function doDelete() {
    if (!confirm("是否確定刪除資料？")) {
        return false;
    }

    var sData = {};
    sData.C_ID = $("#C_ID").val();
    sData.Station = $("#Station").val();
    sData.VoyageTime = $("#VoyageTime").val();
    sData.VesselId = $("#VesselId").val();
    var rIdx = HtGrid1.getSelected()[0];
    sData.IdNo = HtGrid1.getDataAtRowProp(rIdx, "IdNo");
    sData.IdNoEncode = HtGrid1.getDataAtRowProp(rIdx, "IdNoEncode");

    $.ajax({
        type: 'POST',
        dataType: 'json',
        data: sData,
        url: 'AjaxDeleteData',
        timeout: 3000,
        success: function (response) {
            if (response.TotalRow == "1") {// success
                //delete grid
                HtGrid1.alter("remove_row", rIdx, 1);
                HtGrid1.render();
            } else { // delete failed
                if (response.ErrMsg)
                    alert(response.ErrMsg);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (textStatus == "timeout") {
                textStatus += "=" + this.timeout;
            }
            $('#PsgrName').vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + ")" });
            $('#PsgrName').delay(9, function () { $(this).trigger("focus"); });
        }
    });
}

function clearInput() {
    $("#PsgrName").val("");
    $("#Birth").val("");
    $("#IdNo").val("");
}
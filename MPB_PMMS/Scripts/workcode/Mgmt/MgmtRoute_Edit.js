var HtColSetGrid1;
var HtSchemaGrid1;
var HtGrid1;

$(document).ready(function () {
    //$('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btnBack").on("click", function (event) { $.vs_toHref({ targetFrame: window.self, pageURL: $('#currentUrl').val() }); });
    $("#R_CODE").on("blur", function (event) { check_key_duplicate(); });

    if (isViewMode) {
        $(':input:not(#btnBack)').vs_initialForm({ D: 1 });
    }
    if (isDeleteMode) {
        $(':input:not(#btnBack, #btnConfirm)').vs_initialForm({ D: 1 });
    }

    $("#btnConfirm").on("click", doConfirm);
    if (isEditMode) {
        var temp = $("#R_CODE").val();
        $("#chk_Value").val(temp);
    } else {
    }

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
            header: "新增",
            width: 70,
            data: "BA",
            type: "btn",
        },
        {
            header: "場站",
            width: '250%',
            data: "ST_ID",
            type: "select",
            className: "htLeft",
            //vs_initialForm: { M: 20 },
            submit: 1
        },
        {
            header: "刪除",
            width: 70,
            data: "BD",
            type: "btn"
        },
    ]

    $("#tbResults_MgmtRouteGrid1").handsontable({     //set handsontable attribute
        data: HtDataGrid1,
        dataSchema: HtSchemaGrid1,
        contextMenu: false,                 //快捷清單(滑鼠右鍵)
        columnSorting: false,                //欄位排序
        colHeaders: $.map(HtColSetGrid1, function (t, i) { return t.header; }),
        minRows: 1,                        //最少顯示行數
        rowHeaders: true,                   //顯示列抬頭
        manualColumnResize: false,          //可動態調整欄位大小
        manualRowResize: false,             //可動態調整行距大小
        persistentState: true,             //持久化狀態(true=記錄被異動過的值)
        minSpareRows: 0,                    //設定備用欄位
        className: "table_format htCenter",
        columns: $.grep(HtColSetGrid1, function (t, i) { return t.header; }),
        readOnly: isViewMode || isDeleteMode,
        fillHandle: false,
        renderAllRows: true,
        afterRender: function () {
        }
    });
    HtGrid1 = $("#tbResults_MgmtRouteGrid1").handsontable('getInstance');
    $.syncWin_WH.resizeFunctions.push({ func: handsontableResize, params: [$("#tbResults_MgmtRouteGrid1"), HtColSetGrid1] });

    $("body").removeClass("noVisibility");
});

function check_key_duplicate() {
    if ($("#mode").val() != 'A' && $("#mode").val() != 'M') { return false; }
    var R_CODE = $("#R_CODE");
    var R_CODE_value = R_CODE.val();
    if (R_CODE_value == null) { R_CODE_value = ""; }
    if ($("#mode").val() == "M" && ($("#chk_Value").val() == R_CODE_value)) { return false; }
    if (R_CODE_value != "") {   //all keys not null
        $.ajax({
            url: "AjaxCheckKey",
            data: {
                "R_CODE": R_CODE_value
            },
            beforeSend: function (xhr) {
                //for CSRF
                xhr.setRequestHeader("RequestVerificationToken",
                    tokenVal);
            },
            success: function (responseData, textStatus, xhr) {//responseData may equal to xhr.responseText
                //alert(responseData);
                var xmlhttpResponseObj = null;
                //try { xmlhttpResponseObj = $.parseJSON(responseData); } catch (ignore) { }
                try { xmlhttpResponseObj = JSON.parse(responseData) } catch (ignore) { }
                if (xmlhttpResponseObj != null && xmlhttpResponseObj.ToTalRow > 0) {//query成功
                    var Data = JSON.parse(xmlhttpResponseObj.Data);
                    //var Data = $.parseJSON(xmlhttpResponseObj.Data);
                    if (Data[0].KeyCount <= 0) {
                        //可以新增
                    } else {
                        R_CODE.vs_showDivAlert({ alertMassage: "" + R_CODE.attr("title") + "(" + R_CODE_value + ")已存在<br>,不能使用!" });
                        R_CODE.val("" + R_CODE.prop("defaultValue"));
                        R_CODE.delay(9, function () { $(this).trigger("focus"); });
                    }
                } else {
                    R_CODE.vs_showDivAlert({ alertMassage: "資料有誤(ToTalRow=0),不能確定" + R_CODE.attr("title") + "(" + R_CODE_value + ")是否已存在!" });
                    R_CODE.val("" + R_CODE.prop("defaultValue"));
                    R_CODE.delay(9, function () { $(this).trigger("focus"); });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    textStatus += "=" + this.timeout;
                }
                R_CODE.vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + "),不能確定" + R_CODE.attr("title") + "(" + R_CODE_value + ")是否已存在!" });
                R_CODE.val("" + R_CODE.prop("defaultValue"));
                R_CODE.delay(9, function () { $(this).trigger("focus"); });
            },
            complete: function (xhr, textStatus) {//after success and error
                //alert("完成");//alert($.active);
            }
        });
    }
}

function doConfirm(event) {
    $('#mode').val(mode);
    if (isViewMode) {
        return false;
    }
    $('#btnConfirm').vs_initialForm({ D: 1 });
    if (doCheck()) {
        doSave();
    }
    $('#btnConfirm').delay(1500, function () { $(this).vs_initialForm({ D: 0 }); });
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
    $('#resultGrid1').val(get_ht_submit_string("save", HtGrid1, HtColSetGrid1));//多筆
    $.vs_dialogStart({ dialogMassage: "執行中" });
    $('form:first').vs_doSubmit({ actionPage: "MgmtRoute_Save", targetWin: "iframeHide", postMethod: "post" });
}

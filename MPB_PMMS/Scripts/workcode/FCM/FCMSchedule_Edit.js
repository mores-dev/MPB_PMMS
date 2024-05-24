
$(document).ready(function () {
    //$('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btnBack").on("click", function (event) { $.vs_toHref({ targetFrame: window.self, pageURL: $('#currentUrl').val() }); });
    $("#SC_CODE").on("blur", function (event) { check_key_duplicate(); });

    if (isViewMode) {
        $(':input:not(#btnBack)').vs_initialForm({ D: 1 });
    }
    if (isDeleteMode) {
        $(':input:not(#btnBack, #btnConfirm)').vs_initialForm({ D: 1 });
    }

    $("#C_ID_Selector").on("change", function () { C_ID_Selector(); });

    $("#btnConfirm").on("click", doConfirm);
    if (isEditMode) {
        $("#C_ID_Selector").vs_initialForm({ D: 1 });
        var temp = $("#SC_CODE").val();
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

});

function C_ID_Selector() {
    var C_ID_Selector = $("#C_ID_Selector").val();
    $("#C_ID").val(C_ID_Selector);
    check_key_duplicate();
}

function check_key_duplicate() {
    if ($("#mode").val() != 'A' && $("#mode").val() != 'M') { return false; }
    var C_ID = $("#C_ID");
    var C_ID_value = C_ID.val();
    if (C_ID_value == null) { C_ID_value = ""; }

    var SC_CODE = $("#SC_CODE");
    var SC_CODE_value = SC_CODE.val();
    if (SC_CODE_value == null) { SC_CODE_value = ""; }
    if ($("#mode").val() == "M" && ($("#chk_Value").val() == SC_CODE_value)) { return false; }
    if (SC_CODE_value != "" && C_ID_value != "") {   //all keys not null
        $.ajax({
            url: "AjaxCheckKey",
            data: {
                "C_ID": C_ID_value,
                "SC_CODE": SC_CODE_value
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
                        SC_CODE.vs_showDivAlert({ alertMassage: "" + SC_CODE.attr("title") + "(" + SC_CODE_value + ")已存在<br>,不能使用!" });
                        SC_CODE.val("" + SC_CODE.prop("defaultValue"));
                        SC_CODE.delay(9, function () { $(this).trigger("focus"); });
                    }
                } else {
                    SC_CODE.vs_showDivAlert({ alertMassage: "資料有誤(ToTalRow=0),不能確定" + SC_CODE.attr("title") + "(" + SC_CODE_value + ")是否已存在!" });
                    SC_CODE.val("" + SC_CODE.prop("defaultValue"));
                    SC_CODE.delay(9, function () { $(this).trigger("focus"); });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    textStatus += "=" + this.timeout;
                }
                SC_CODE.vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + "),不能確定" + SC_CODE.attr("title") + "(" + SC_CODE_value + ")是否已存在!" });
                SC_CODE.val("" + SC_CODE.prop("defaultValue"));
                SC_CODE.delay(9, function () { $(this).trigger("focus"); });
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
        var SC_TIME_Value = $("#SC_TIME").val();
        var msg = "";
        if (SC_TIME_Value != null) {
            if (SC_TIME_Value.length != 4)
                msg = "請輸入正確的時間格式(HHmm)";
            else if (SC_TIME_Value.substring(0, 2) > 23)
                msg = "請輸入正確的時間格式(HHmm)";
            else if (SC_TIME_Value.substring(2, 4) > 59)
                msg = "請輸入正確的時間格式(HHmm)";

            if (msg != "")
                $("#SC_TIME").vs_showDivAlert({ append: false, alertMassage: msg });//自訂alert
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
    $.vs_dialogStart({ dialogMassage: "執行中" });
    $('form:first').vs_doSubmit({ actionPage: "FCMSchedule_Save", targetWin: "iframeHide", postMethod: "post" });
}

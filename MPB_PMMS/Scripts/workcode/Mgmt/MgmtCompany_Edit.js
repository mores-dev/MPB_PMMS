
$(document).ready(function () {
    //$('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btnBack").on("click", function (event) { $.vs_toHref({ targetFrame: window.self, pageURL: $('#currentUrl').val() }); });
    $("#C_CODE").on("blur", function (event) { check_key_duplicate(); });

    if (isViewMode) {
        $(':input:not(#btnBack)').vs_initialForm({ D: 1 });
    }
    if (isDeleteMode) {
        $(':input:not(#btnBack, #btnConfirm)').vs_initialForm({ D: 1 });
    }

    $("#btnConfirm").on("click", doConfirm);
    if (isEditMode) {
        var temp = $("#C_CODE").val();
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

function check_key_duplicate() {
    if ($("#mode").val() != 'A' && $("#mode").val() != 'M') { return false; }
    var C_CODE = $("#C_CODE");
    var C_CODE_value = C_CODE.val();
    if (C_CODE_value == null) { C_CODE_value = ""; }
    if ($("#mode").val() == "M" && ($("#chk_Value").val() == C_CODE_value)) { return false; }
    if (C_CODE_value != "") {   //all keys not null
        $.ajax({
            url: "AjaxCheckKey",
            data: {
                "C_CODE": C_CODE_value
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
                        C_CODE.vs_showDivAlert({ alertMassage: "" + C_CODE.attr("title") + "(" + C_CODE_value + ")已存在<br>,不能使用!" });
                        C_CODE.val("" + C_CODE.prop("defaultValue"));
                        C_CODE.delay(9, function () { $(this).trigger("focus"); });
                    }
                } else {
                    C_CODE.vs_showDivAlert({ alertMassage: "資料有誤(ToTalRow=0),不能確定" + C_CODE.attr("title") + "(" + C_CODE_value + ")是否已存在!" });
                    C_CODE.val("" + C_CODE.prop("defaultValue"));
                    C_CODE.delay(9, function () { $(this).trigger("focus"); });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    textStatus += "=" + this.timeout;
                }
                C_CODE.vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + "),不能確定" + C_CODE.attr("title") + "(" + C_CODE_value + ")是否已存在!" });
                C_CODE.val("" + C_CODE.prop("defaultValue"));
                C_CODE.delay(9, function () { $(this).trigger("focus"); });
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
    $.vs_dialogStart({ dialogMassage: "執行中" });
    $('form:first').vs_doSubmit({ actionPage: "MgmtCompany_Save", targetWin: "iframeHide", postMethod: "post" });
}

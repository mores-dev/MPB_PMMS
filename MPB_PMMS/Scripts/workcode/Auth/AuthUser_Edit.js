var HtColSetGrid1;
var HtSchemaGrid1;
var HtGrid1;

$(document).ready(function () {
    $('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btnBack").on("click", function (event) { $.vs_toHref({ targetFrame: window.self, pageURL: $('#currentUrl').val() }); });

    if (isViewMode) {
        $(':input:not(#btnBack)').vs_initialForm({ D: 1 });
    }
    if (isDeleteMode) {
        $(':input:not(#btnBack, #btnConfirm)').vs_initialForm({ D: 1 });
    }

    $("#btnConfirm").on("click", doConfirm);
    if (isEditMode) {
        $("#UserId").vs_initialForm({ D: 1 });
        //$("#UserType").vs_initialForm({ D: 1 });
        $("#RoleId").vs_initialForm({ D: 1 });
        $("#C_ID").vs_initialForm({ D: 1 });
    } else {
        $("#UserId").on("blur", function (event) { check_key_duplicate(); });
    }

    //$("#UserType").on("change", function () { showObject(); });
    $("#RoleId").on("change", function () { showObject(); });

    // ===== Chosen Example =====
    $(".chosen-select").chosen({
        allow_single_deselect: false,            //可清除選取
        disable_search_threshold: 10,           //拿掉搜尋列
        search_contains: true,                   //不需從頭相同
        no_results_text: "查無結果!"           //顯示查無資料訊息
    });

    showObject();

    $("body").removeClass("noVisibility");
});

function showObject() {
    var roleId = $("#RoleId").val();
    //var userType = $("#UserType").val();
    //if (userType == "B") {
    if (roleId.indexOf("1") == 0) {
        $("#divDEPT").css("display", "");
        $("#divCompany").css("display", "none");
        $("#C_ID").val('');
    } else if (roleId.indexOf("2") == 0) {
        $("#divDEPT").css("display", "none");
        $("#DEPT").val('');
        $("#divCompany").css("display", "");
    } else {
        $("#divDEPT").css("display", "none");
        $("#DEPT").val('');
        $("#divCompany").css("display", "none");
        $("#C_ID").val('');
    }
}

function check_key_duplicate() {
    if ($("#mode").val() != 'A') { return false; }
    var UserId = $("#UserId");
    var UserId_value = UserId.val();
    if (UserId_value == null) { UserId_value = ""; }
    if (UserId_value != "") {   //all keys not null
        $.ajax({
            url: "AjaxCheckKey",
            data: {
                "UserId": UserId_value
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
                        UserId.vs_showDivAlert({ alertMassage: "" + UserId.attr("title") + "(" + UserId_value + ")已存在<br>,不能使用!" });
                        UserId.val("" + UserId.prop("defaultValue"));
                        UserId.delay(9, function () { $(this).trigger("focus"); });
                    }
                } else {
                    UserId.vs_showDivAlert({ alertMassage: "資料有誤(ToTalRow=0),不能確定" + UserId.attr("title") + "(" + UserId_value + ")是否已存在!" });
                    UserId.val("" + UserId.prop("defaultValue"));
                    UserId.delay(9, function () { $(this).trigger("focus"); });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    textStatus += "=" + this.timeout;
                }
                UserId.vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + "),不能確定" + UserId.attr("title") + "(" + UserId_value + ")是否已存在!" });
                UserId.val("" + UserId.prop("defaultValue"));
                UserId.delay(9, function () { $(this).trigger("focus"); });
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
        //自訂規則 --新增帳號 或 修改密碼時進行檢查
        var pdVal = $("#Pd").val();
        var msg = "";
        if ($("#mode").val() == "A" || $("#mode").val() == "M" && pdVal != "") {
            if (pdVal.length > 0 && (pdVal.length < 8 || !(pdVal.match(/[a-zA-Z]/) != null && pdVal.match(/[0-9]/) != null)))
                msg += "密碼需為英文、數字混合8位數(含)以上<br>";
            else if (pdVal.length > 0 && (pdVal.length > 16 || !(pdVal.match(/[a-zA-Z]/) != null && pdVal.match(/[0-9]/) != null)))
                msg += "密碼需為英文、數字混合16位數以下<br>";

            if (msg != "")
                $("#Pd").vs_showDivAlert({ alertMassage: msg });
        }
        //var userType = $("#UserType").val();
        var roleId = $("#RoleId").val();
        var C_ID = $("#C_ID").val();
        var ErrMsg = "";
        if (roleId.indexOf("2") == "0" && (C_ID == null || C_ID == ""))
            ErrMsg = "不可為空";

        if (ErrMsg != "")
            $("#C_ID").vs_showDivAlert({ alertMassage: ErrMsg });
        if (msg != "" || ErrMsg !== "")
            return false;
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
    $('form:first').vs_doSubmit({ actionPage: "AuthUser_Save", targetWin: "iframeHide", postMethod: "post" });
}

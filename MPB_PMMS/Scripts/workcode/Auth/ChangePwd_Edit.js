
$(document).ready(function () {
    $('.date-picker').datepicker();
    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btnConfirm").on("click", doConfirm);
});

function doConfirm(event) {
    if ($("#NewPd").val() == "") {
        alert("新密碼不可為空白!")
        return
    }
    if ($("#ConfirmPd").val() == "") {
        alert("密碼確認不可為空白!")
        return
    }
    if ($("#NewPd").val() != $("#ConfirmPd").val()) {
        alert("新密碼與密碼確認不相符!")
        return
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
    $('[class*=vs_formValidate]').vs_formValidate();
    //自訂規則
    if ($.vs_showDivAlert({ resetZindex: true, order: "desc" }) > 0) {//重設z-index回傳alertDiv總數量
        $("div.divAlert:first").vs_findDivAlertParent().vs_scroll_Focus({ showDivAlert: true });
        return false;
    }
    return true;
}

function doSave() {
    $.vs_dialogStart({ dialogMassage: "執行中" });
    $('form:first').vs_doSubmit({ actionPage: "ChangePwd_Save", postMethod: "post" });
}

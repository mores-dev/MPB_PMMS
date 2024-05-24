
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
    $("#btnChangePPP").on("click", doConfirm_PPP);

    if (isEditMode) {
        $("#GaAAA").vs_initialForm({ D: 1 });
    } else {
        $("#GaAAA").on("blur", function (event) { check_key_duplicate(); });
    }
    $("#Email").on("blur", function (event) { if ($("#Email").val()) validData($("#Email"), "Email") });
    $("#UniId").on("blur", function (event) { if ($("#UniId").val()) validData($("#UniId"), "UniId") });

    $phoneType = $('#PhoneType');

    $phoneType.change(function () {
        if ($phoneType.val() == 'T') {
            $('div.areaNumArea').show();
            $('div.areaNumDiff').show();
            $('div.ExtArea').show();
            $('div.ExtSharp').show();
            $('#Phone').css('width', '50%');
            //$('#Phone').val('');
            //$('#AreaNumber').val('');
            //$('#Ext').val('');
        } else {
            $('div.areaNumArea').hide();
            $('div.areaNumDiff').hide();
            $('div.ExtArea').hide();
            $('div.ExtSharp').hide();
            $('#Phone').css('width', '100%');
            //$('#Phone').val('');
            //$('#AreaNumber').val('');
            //$('#Ext').val('');
        }
    });

    $phoneType.trigger("change");

    // ===== Chosen Example =====
    $(".chosen-select").chosen({
        allow_single_deselect: false,            //可清除選取
        disable_search_threshold: 10,           //拿掉搜尋列
        search_contains: true,                   //不需從頭相同
        no_results_text: "查無結果!"           //顯示查無資料訊息
    });

});

function check_key_duplicate() {
    if ($("#mode").val() != 'A') { return false; }
    var Account = $("#GaAAA");
    var Account_value = Account.val();
    if (Account_value == null) { Account_value = ""; }
    if (Account_value != "") {   //all keys not null
        $.ajax({
            url: "AjaxCheckKey",
            data: {
                "GaAAA": Account_value
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
                        Account.vs_showDivAlert({ alertMassage: "" + Account.attr("title") + "(" + Account_value + ")已存在<br>,不能使用!" });
                        Account.val("" + Account.prop("defaultValue"));
                        Account.delay(9, function () { $(this).trigger("focus"); });
                    }
                } else {
                    Account.vs_showDivAlert({ alertMassage: "資料有誤(ToTalRow=0),不能確定" + Account.attr("title") + "(" + Account_value + ")是否已存在!" });
                    Account.val("" + Account.prop("defaultValue"));
                    Account.delay(9, function () { $(this).trigger("focus"); });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    textStatus += "=" + this.timeout;
                }
                Account.vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + "),不能確定" + Account.attr("title") + "(" + Account_value + ")是否已存在!" });
                Account.val("" + Account.prop("defaultValue"));
                Account.delay(9, function () { $(this).trigger("focus"); });
            },
            complete: function (xhr, textStatus) {//after success and error
                //alert("完成");//alert($.active);
            }
        });
    }
}

function doConfirm_PPP(event) {
    $('#mode').val(mode);
    $('#flag_PPP').val("PPP");
    if (isViewMode) {
        return false;
    }
    $('#btnChangePPP').vs_initialForm({ D: 1 });
    if (doCheck()) {
        doSave();
    }
    $('#btnChangePPP').delay(1500, function () { $(this).vs_initialForm({ D: 0 }); });
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
        var b = true;
        var pwdVal = $("#GA_PPP").val();
        var msg = "";
        if ($("#mode").val() == "A" || $("#mode").val() == "M" && pwdVal != "") {
            if ($("#mode").val() == "A")
                $("#GA_PPP").vs_formValidate({ NE: 1 });

            if (pwdVal.length > 0 && (pwdVal.length < 8)) {
                msg += "密碼不足8碼<br>";
            } else if (pwdVal.length > 0 && (pwdVal.length > 16)) {
                msg += "密碼超過16碼<br>";
            } else if (pwdVal.length > 0 && (!(pwdVal.match(/^(?=.*\d)(?=.*[a-zA-Z])(?=.*\W).{8,16}$/) != null))) {
                msg += "密碼需含特殊符號、數字與英文區分大小寫<br>";
            }

            //if (pwdVal.length > 0 && (pwdVal.length < 8 || !(pwdVal.match(/[a-zA-Z]/) != null && pwdVal.match(/[0-9]/) != null)))
            //    msg += "密碼需為英文、數字混合8位數(含)以上<br>";
            //else if (pwdVal.length > 0 && (pwdVal.length > 16 || !(pwdVal.match(/[a-zA-Z]/) != null && pwdVal.match(/[0-9]/) != null)))
            //    msg += "密碼需為英文、數字混合16位數以下<br>";

            if (msg != "") {
                $("#GA_PPP").vs_showDivAlert({ alertMassage: msg });
                b = false;
            }
        }
        if ($('#GA_PPP').val() != $('#GA_PPPC').val()) {
            $('#GA_PPP').vs_showDivAlert({ alertMassage: "新密碼與新密碼確認內容不同 <br>" });
            $('#GA_PPP').val("" + $('#GA_PPP').prop("defaultValue"));
            $('#GA_PPPC').val("" + $('#GA_PPPC').prop("defaultValue"));
            b = false;
        }
        if ($('#PhoneType').val() == 'M') {
            if ($('#Phone').val().length != 10) {
                $('#Phone').vs_showDivAlert({ alertMassage: "手機號碼長度錯誤 <br>" });
                $('#Phone').val("");
                b = false;
            }
        } else {
            if (!$('#AreaNumber').val()) {
                $('#AreaNumber').vs_showDivAlert({ alertMassage: "請輸入電話號碼區碼 <br>" });
                b = false;
            }
            if ($('#Phone').val().length > 8) {
                $('#Phone').vs_showDivAlert({ alertMassage: "電話號碼長度錯誤 <br>" });
                $('#Phone').val("");
                b = false;
            }
        }
        if ($('#UniId').val().length) {
            if ($('#UniId').val().length != 8) {
                $('#UniId').vs_showDivAlert({ alertMassage: "統一編號長度錯誤 <br>" });
                b = false;

            } else {
                let regStr = /[0-9]{8}/;
                if (!regStr.test($('#UniId').val())) {
                    $('#UniId').vs_showDivAlert({ alertMassage: "統一編號內容有誤 <br>" });
                    b = false;
                }
            }

        }
        if (!b)
            return false;

        if ($.vs_showDivAlert({ resetZindex: true, order: "desc" }) > 0) {//重設z-index回傳alertDiv總數量
            $("div.divAlert:first").vs_findDivAlertParent().vs_scroll_Focus({ showDivAlert: true });
            return false;
        }
    }
    return true;
}

function doSave() {
    $.vs_dialogStart({ dialogMassage: "執行中" });
    $('form:first').vs_doSubmit({ actionPage: "SysMbrMgmt_Save", targetWin: "iframeHide", postMethod: "post" });
}

function validData(obj, objName) {
    //if ($("#mode").val() != 'A') { return false; }
    
    var obj_value = obj.val();
    if (obj_value == null) { obj_value = ""; }
    if (obj_value != "") {   //all keys not null
        $.ajax({
            url: "AjaxCheckValid",
            data: {
                "ValidData": obj_value,
                "ValidType": objName
            },
            beforeSend: function (xhr) {
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
                        obj.vs_showDivAlert({ alertMassage: "" + obj.attr("title") + "格式錯誤，請修正!" });
                        obj.val("" + obj.prop("defaultValue"));
                        obj.delay(9, function () { $(this).trigger("focus"); });
                    }
                } else {
                    obj.vs_showDivAlert({ alertMassage: "資料有誤(ToTalRow=0),不能確定" + obj.attr("title") + "(" + obj_value + ")格式是否正確!" });
                    obj.val("" + obj.prop("defaultValue"));
                    obj.delay(9, function () { $(this).trigger("focus"); });
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    textStatus += "=" + this.timeout;
                }
                obj.vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + "),不能確定" + obj.attr("title") + "(" + obj_value + ")格式是否正確!" });
                obj.val("" + obj.prop("defaultValue"));
                obj.delay(9, function () { $(this).trigger("focus"); });
            },
            complete: function (xhr, textStatus) {//after success and error
                //alert("完成");//alert($.active);
            }
        });
    }

}
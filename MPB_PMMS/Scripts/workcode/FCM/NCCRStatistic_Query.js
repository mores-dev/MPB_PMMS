var HtColSet;
var Ht;
$(document).ready(function () {
    $('.date-picker').datepicker();
    $.datepicker.setDefaults($.datepicker.regional["zh-TW"]);

    $('[class*=vs_initialForm]').vs_initialForm();
    $.vs_setSelectDefaultValue();

    $("#btQuery").on("click", function (event) {
        if (doCheck()) {
            //$('form:first').vs_doSubmit({ actionPage: modifyPage, targetWin: "_blank", postMethod: "post" });
            $('form:first').vs_doSubmit({ actionPage: queryPage, targetWin: "_self", postMethod: "post" });
        }
    });

    // ===== Chosen Example =====
    $(".chosen-select").chosen({
        allow_single_deselect: true,            //可清除選取
        disable_search_threshold: 10,           //拿掉搜尋列
        search_contains: true,                   //不需從頭相同
        no_results_text: "查無結果!"           //顯示查無資料訊息
    });

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
        if ($.vs_showDivAlert({ resetZindex: true, order: "desc" }) > 0) {//重設z-index回傳alertDiv總數量
            $("div.divAlert:first").vs_findDivAlertParent().vs_scroll_Focus({ showDivAlert: true });
            return false;
        }

    }
    return true;
}
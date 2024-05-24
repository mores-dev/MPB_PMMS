(function ($) {
    if (!$.browser) { return; }
    if (!$.browser.msie && ("" + window.navigator.userAgent).match(/Trident|Edge/i)) {//ie fake
        delete $.browser.mozilla;
        delete $.browser.webkit;
        delete $.browser.safari;
        delete $.browser.chrome;
        delete $.browser.opera;
        var tmp_arr, tmp_version = $.browser.version;
        if (tmp_arr = ("" + window.navigator.userAgent).match(/(?:mozilla)(?:.*? (Trident))(?:.*? rv:([\w.]+)|)/i)) {
            tmp_version = tmp_arr[2];
        } else if (tmp_arr = ("" + window.navigator.userAgent).match(/(Edge)[ \/]([\w.]+)/i)) {
            tmp_version = tmp_arr[2];
        }
        $.extend($.browser, { msie: true, version: tmp_version });
    }
    if ($.browser.webkit) {//identify chrome , safari and opera 15+
        //[window.navigator.vendor] value be "Google Inc." or "Apple Computer, Inc." or "Opera Software ASA"
        if (("" + window.navigator.vendor).match(/^Apple.*$/i)) {
            $.extend($.browser, { isSafari: true });
        } else {
            delete $.browser.safari;//Totally delete this deprecated property
            if (("" + window.navigator.vendor).match(/^Google.*$/i)) {
                $.extend($.browser, { isChrome: true, chrome: true });
            } else if (("" + window.navigator.vendor).match(/^Opera.*$/i)) {//[opera 15+] => [chromium]
                $.extend($.browser, { isOpera: true, opera: true });
            }
        }
    }
    if ($.browser.msie && ($.browser.version.split('.', 1)[0] - 0) <= 6) {
        $.extend($.browser, { isIE6: true });
    }
})(jQuery);
(function ($) {//外部 plugin
    $.extend({
        jQuerize: function (obj) {//$.jQuerize(this)//prevent $($(obj))//this===$(obj) combine [return $(this);] cause jquery chain function [end()] error
            if (typeof (obj) != "object") {
                obj = null;
            }

            var jQuerized_Obj = $(null);
            if (obj && obj.jquery) {//is jQuery Obj
                jQuerized_Obj = obj;
            } else {
                try { jQuerized_Obj = $(obj); } catch (igore) { }//jQuerize
            }

            return jQuerized_Obj;
        },
        htmlEncode: function (str) {
            var tmpObj = $('<div/>').text("" + str);
            var outputStr = "" + tmpObj.html();
            tmpObj.remove();
            return outputStr;
        },
        htmlDecode: function (str) {
            var tmpObj = $('<div/>').html("" + str);
            var outputStr = "" + tmpObj.text();
            tmpObj.remove();
            return outputStr;
        },
        syncWin_Initial: function () {
            if (!$.syncWin_WH.doc) {
                $.syncWin_WH.doc = window.document;
                var doc_obj = $.syncWin_WH.doc;
                $.syncWin_WH.docWidth = $(doc_obj).width() + $(doc_obj).scrollLeft();
                $.syncWin_WH.docHeight = $(doc_obj).height() + $(doc_obj).scrollTop();
                $.syncWin_WH.resizeFunctions = [];
                $.syncWin_WH.registerRun = false;
                $.syncWin_WH.registerRunning = false;
                $.syncWin_WH.prepareRunInterval = 53;
                $.syncWin_WH.prepareRunTimestamp = (new Date()).getTime() + $.syncWin_WH.prepareRunInterval;
            }
        },
        syncWin_WH: function (event) {
            $.syncWin_WH.prepareRunTimestamp = (new Date()).getTime() + $.syncWin_WH.prepareRunInterval;
            if ($.syncWin_WH.registerRun === false) {
                $.syncWin_WH.registerRun = true;
                $("body").delay($.syncWin_WH.prepareRunInterval, $.syncWin_Prepare);
            }
        },
        syncWin_Prepare: function () {
            var tmp_time_diff = $.syncWin_WH.prepareRunTimestamp - (new Date()).getTime();
            if (tmp_time_diff > 0) {
                $("body").delay(tmp_time_diff, $.syncWin_Prepare);
            } else {
                $.syncWin_Run();
            }
        },
        syncWin_Run: function () {
            var doc_obj = $.syncWin_WH.doc;
            $.syncWin_WH.docWidth = $(doc_obj).width() + $(doc_obj).scrollLeft();
            $.syncWin_WH.docHeight = $(doc_obj).height() + $(doc_obj).scrollTop();
            $.syncWin_WH.resizeFunctions.sort(function (elem1, elem2) {//element not visible be first
                try { return elem1.params[0].is(":visible") - elem2.params[0].is(":visible"); } catch (ignore) { }
                return 0;
            });
            $.each($.syncWin_WH.resizeFunctions, function (idx, elem) {
                if ($.isPlainObject(elem) && $.isFunction(elem.func) && $.isArray(elem.params)) {
                    elem.func.apply(null, elem.params);
                    $.syncWin_WH.registerRunning = true;
                }
            });
            $.syncWin_WH.registerRunning = false;
            $.syncWin_WH.registerRun = false;
        },
        JSONserialize: (JSON && JSON.stringify) ? JSON.stringify : function (obj) {//JSON object to String
            //https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/stringify
            //http://blogs.sitepoint.com/2009/08/19/javascript-json-serialization/
            //http://stackoverflow.com/questions/3593046/jquery-json-to-string
            //http://stackoverflow.com/questions/191881/serializing-to-json-in-jquery
            //http://github.com/douglascrockford/JSON-js
            var t = typeof (obj);
            if (t != "object" || obj === null) {
                // simple data type
                if (t == "string") {
                    obj = '"' + obj + '"';
                }
                return String(obj);
            } else {
                // recurse array or object
                var n, v, json = [], arr = (obj && obj.constructor == Array);
                for (n in obj) {
                    v = obj[n]; t = typeof (v);
                    if (t == "string") v = '"' + v + '"';
                    else if (t == "object" && v !== null) v = $.JSONserialize(v);
                    json.push((arr ? "" : '"' + n + '":') + String(v));
                }
                return (arr ? "[" : "{") + String(json) + (arr ? "]" : "}");
            }
        },
        dummy: $.noop
    });

    $.fn._base_delay = $.fn._base_delay ? $.fn._base_delay : $.fn.delay;//backup oringinal jQuery.fn.delay
    $.fn.extend({
        delay: function (time, type) {//overwrite jQuery.fn.delay

            if ($.isFunction(type)) {//http://james.padolsey.com/javascript/jquery-delay-plugin/
                // Empty function:
                jQuery.fx.step.delay = function () { };
                // Return meaningless animation, (will be added to queue)
                return this.animate({ delay: 1 }, time, type);//.animate(css properties,duration time,callback function)
            }

            return $.fn._base_delay(time, type);//oringinal jQuery.fn.delay
        },
        reverse: [].reverse,//jQuery.fn.reverse = [].reverse;//.reverse().each()
        dummy: $.noop
    });

})(jQuery);//外部 plugin

//String extend start
String.prototype.trim = function () {//trimming space from both side of the string
    return ("" + this).replace(/^\s+|\s+$/g, "");
};
String.prototype.ltrim = function () {//trimming space from left side of the string
    return ("" + this).replace(/^\s+/, "");
};
String.prototype.rtrim = function () {//trimming space from right side of the string
    return ("" + this).replace(/\s+$/, "");
};

var padStrBuilder = function (padLength, padString) {//"AAAAA"<->padStrBuilder(5,"A")
    return "" + new Array(padLength - 0 + 1).join("" + padString);
};
String.prototype.lpad = function (padLength, padString) {//("1").lpad(10,"0")
    var str = "" + this;
    if (str.length < padLength) {
        str = padStrBuilder(padLength - str.length, padString) + str;
    }
    return str;
};
String.prototype.rpad = function (padLength, padString) {//("1").rpad(10,"0")
    var str = "" + this;
    if (str.length < padLength) {
        str = str + padStrBuilder(padLength - str.length, padString);
    }
    return str;
};

String.prototype.escapejQueryfilter = function () {//meta-characters
    //https://api.jquery.com/category/selectors/
    //var strVal="value(with meta-characters)";
    //$("input[name='" + strVal.escapejQueryfilter + "']");
    return this.replace(
		/([\!\"#$%&\'()*+,.\/:;<=>?@\[\\\]\^\`\{|}~])/g,
		'\\$1'
	);
};
//String extend end

//Number extend start
//(0.05).toFixed(1) should be 0.1
//(0.35).toFixed(1) should be 0.4
if (Number.prototype.toFixed) {
    Number.prototype._toFixed = Number.prototype._toFixed ? Number.prototype._toFixed : Number.prototype.toFixed;//backup
}
Number.prototype.toFixed = function (precision) {//overwrite
    if (typeof (precision) === 'string') {
        try { precision = parseInt(precision, 10); } catch (e) { precision = 0; }
    }
    if (typeof (precision) !== 'number' || parseFloat(precision) != parseInt(precision, 10) || isNaN(precision)) {
        precision = 0;
    }
    var power = Math.pow(10, precision || 0);
    var num = (Math.round(this * power) / power).toString();
    if (precision <= 0) { return num; }

    if (num.indexOf('.') < 0) { num += '.'; }
    var padding = precision + 1 - (num.length - num.indexOf('.'));
    return num + padStrBuilder(padding, '0');
};
//Number extend end

(function ($) {//ajax Settings//http://api.jquery.com/category/ajax/low-level-interface/
    $.ajaxSetup({
//        url: "../ajax/ajaxGen_JSON.php",
        type: "POST",
        timeout: 10000,
        dataType: "text",
        cache: false,
        async: true
    });
})(jQuery);//ajax Settings

(function ($) {//未完iniFormSet
    //Jquery Selector Note:If you wish to use any of the meta-characters ( such as !"#$%&'()*+,./:;<=>?@[\]^`{|}~ ) as a literal part of a name, you must escape the character with two backslashes: \\. For example, if you have an an element with id="foo.bar", you can use the selector $("#foo\\.bar"). 
    //一般function start
    $.extend({
        vs_toHref: function (options) {
            var settings = $.extend({ targetFrame: window.self, pageURL: window.self.location.href, delayTime: 0 }, options || {});
            $("body").delay(settings.delayTime, function () { settings.targetFrame.location.href = settings.pageURL; });
        },
        vs_dialogStart: function (options) {
            var settings = $.extend({ dialogMassage: "執行中..." }, options || {});
            if ($("div.divDialog").length > 0) {
                $("div.divDialog").find('p').css("color", "#FF0000").html(settings.dialogMassage).end().dialog("open");
            } else {
                $('<div class="divDialog" title=""><p></p></div>').appendTo("body").dialog({ autoOpen: false, closeOnEscape: false, modal: true }).find('p').css("color", "#FF0000").html(settings.dialogMassage).end().dialog("open");
            }
        },
        vs_dialogClose: function (options) {
            var settings = $.extend({ dialogMassage: "......完成", delayTime: 5000 }, options || {});
            $("div.divDialog").find('p').html(settings.dialogMassage).end().stop().delay(settings.delayTime, function () { $(this).dialog("close"); });
        },
        vs_setSelectDefaultValue: function (options) {
            var settings = $.extend({}, options || {});
            $("select:not([multiple])").each(function () {
                var attr_value = $(this).attr("defaultValue");
                if (attr_value) {
                    if ("" + attr_value != "") {
                        $(this).val("" + attr_value);
                    }
                    $(this).removeAttr("defaultValue");
                }
                var curr_value = (($(this).val()) ? ("" + $(this).val()) : "");
                $(this).prop("defaultValue", curr_value);//比照<input>,設定<select-one> defaultValue
            });
        },
        vs_showDivAlert: function (options) {
            var settings = $.extend({ resetZindex: false, shiftZindex: 0, orderBy: "desc" }, options || {});
            var allSize = $("div.divAlert").length;
            if (settings.resetZindex) {
                settings.orderBy = settings.orderBy.toLowerCase();
                if (settings.orderBy == "asc") {
                    $("div.divAlert").each(function (idx) {//reset z-index
                        $(this).zIndex(1 + idx + settings.shiftZindex);
                    });
                } else {
                    $("div.divAlert").each(function (idx) {//reset z-index
                        $(this).zIndex(allSize - idx + settings.shiftZindex);
                    });
                }
            }
            return allSize;
        },
        dummy: $.noop
    });
    //一般function end

    //串列function start
    $.fn.extend({
        vs_formValidate: function (options) {
            var settings = $.extend({}, options || {});
            return $.jQuerize(this).each(function () {
                var formObj = this;
                var formObjTag = ("" + $(formObj).prop("tagName")).toLowerCase();
                if (formObjTag.match(/^(input|select|textarea|div)$/i) != null) {//div特殊處理
                    var inputType = ("" + $(formObj).prop("type")).toLowerCase();
                    if (formObjTag == "div") {
                        if ($(formObj).is("div.validate_check") && $(formObj).find("input:checkbox:first").length > 0) {
                            inputType = "validate_check";
                        } else {
                            return true;
                        }
                    }
                    var tmpJSONObject = null;
                    if ($.isEmptyObject(settings)) {
                        var tmpMatchArr = $(formObj).prop("class").match(/\x76\x73\x5F\x66\x6F\x72\x6D\x56\x61\x6C\x69\x64\x61\x74\x65\x28(\{[,\{\}0-9A-Za-z: ]*?\})\)(?= +(?!,)|$)/);
                        if (tmpMatchArr != null) {
                            var tmpJSONString = "" + tmpMatchArr[1].replace(/[ ]/g, "").replace(/([\{,])([0-9A-Z]*?)(:)/ig, "$1\"$2\"$3").replace(/(:)([0-9A-Z]*?)([,\}])/ig, "$1\"$2\"$3");
                            try { tmpJSONObject = $.parseJSON(tmpJSONString.toUpperCase()); } catch (ignore) { }
                        }
                    } else {
                        tmpJSONObject = settings;
                        if (tmpJSONObject != null && !$.isEmptyObject(tmpJSONObject) && !isNaN(tmpJSONObject.RESET) && tmpJSONObject.RESET == 1) {
                            //remove
                            var tmpMatchArr = $(formObj).prop("class").match(/\x76\x73\x5F\x66\x6F\x72\x6D\x56\x61\x6C\x69\x64\x61\x74\x65\x28(\{[,\{\}0-9A-Za-z: ]*?\})\)(?= +(?!,)|$)/);
                            var tmpHTMLString = "";
                            if (tmpMatchArr != null) {
                                tmpHTMLString = "\x76\x73\x5F\x66\x6F\x72\x6D\x56\x61\x6C\x69\x64\x61\x74\x65\x28" + tmpMatchArr[1] + ")";
                                //alert(tmpHTMLString);
                                $(formObj).removeClass("" + tmpHTMLString);
                            }
                            //remove

                            //reset
                            tmpHTMLString = "\x76\x73\x5F\x66\x6F\x72\x6D\x56\x61\x6C\x69\x64\x61\x74\x65\x28" + $.JSONserialize(tmpJSONObject).replace(/\"/g, "").replace(/(RESET:1,)|(,RESET:1)|(RESET:1)/ig, "") + ")";
                            $(formObj).addClass(tmpHTMLString);
                            //reset
                            return true;
                        }
                    }
                    if (tmpJSONObject != null && !$.isEmptyObject(tmpJSONObject)) {
                        tmpJSONObject = $.extend({ EI: null }, tmpJSONObject || {});
                        var formObjValue = "" + $(formObj).val();
                        var massage = "";
                        if (tmpJSONObject.EI == null || tmpJSONObject.EI <= 0 || formObjValue != "") {//EI:if Empty,then Ignore
                            $.each(tmpJSONObject, function (name, value) {// this==value==tmpObject[name]
                                switch (name) {
                                    case "EI":
                                        break;
                                    case "NE":
                                        if (inputType == "validate_check") {
                                            if ($(formObj).find("input:checkbox:checked").length<=0) {
                                                massage += "必須選至少一個<br>";
                                            }
                                        } else {
                                            if (!isNaN(value) && value - 0 > 0 && formObjValue == "") {
                                                massage += "不可為空<br>";
                                            }
                                        }
                                        break;
                                    case "NL":
                                        if (!isNaN(value) && value - 0 > 0 && formObjValue.length < value) {
                                            massage += "長度需要至少" + value + "位<br>";
                                        }
                                        break;
                                    case "ML":
                                        if (!isNaN(value) && value - 0 > 0 && formObjValue.length > value) {
                                            massage += "不可超過" + value + "位長度<br>";
                                        }
                                        break;
                                    case "N":
                                        if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9\.]/) != null || isNaN(formObjValue - 0))) {
                                            massage += "必須為0-9數字<br>";
                                        }
                                        break;
                                    case "NQ":
                                        if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9-]/) != null || isNaN(formObjValue - 0))) {
                                            massage += "必須為整數<br>";
                                        }
                                        break;
                                    case "DP":
                                        if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9\.]/) != null || isNaN(formObjValue - 0))) {
                                            massage += "必須為正數<br>";
                                        }
                                        break;
                                    case "DC":
                                        if (!isNaN(value) && value - 0 > 0 && (formObjValue.match(/[^0-9\.-]/) != null || isNaN(formObjValue - 0))) {
                                            massage += "必須為數字<br>";
                                        }
                                        break;
                                    case "EN":
                                        if (value.match(/^(U)$/i) != null && formObjValue.match(/[^0-9A-Z]/) != null) {//EN-U
                                            massage += "必須為大寫英數字<br>";
                                        } else if (value.match(/^(L)$/i) != null && formObjValue.match(/[^0-9a-z]/) != null) {//EN-L
                                            massage += "必須為小寫英數字<br>";
                                        } else if (!isNaN(value) && value - 0 > 0 && formObjValue.match(/[^0-9A-Za-z]/) != null) {//EN
                                            massage += "必須為英數字<br>";
                                        }
                                        break;
                                    case "DT":
                                        if (!isNaN(value) && value - 0 > 0) {
                                            if (formObjValue.match(/^[0-9]{8}$/) == null) {
                                                massage += "必須為正確日期字串<br>";
                                            } else {
                                                var tmpstrArray = formObjValue.match(/^([0-9]{4})([0-9]{2})([0-9]{2})$/);
                                                var tmpYYYY = tmpstrArray[1];
                                                var tmpMM = tmpstrArray[2];
                                                var tmpDD = tmpstrArray[3];
                                                var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);
                                                if (tmpYYYY - 0 < 1850 || tmpYYYY - 0 != tmpDate.getFullYear() || tmpMM - 1 != tmpDate.getMonth() || tmpDD - 0 != tmpDate.getDate()) {
                                                    massage += "必須為正確日期字串<br>";
                                                }
                                            }
                                        }
                                        break;
                                    case "DT1":
                                        if (!isNaN(value) && formObjValue.length > 0) {
                                            if (formObjValue.match(/^[0-9]{4}[/][0-9]{2}[/][0-9]{2}$/) == null) {
                                                massage += "必須為正確日期字串<br>";
                                            } else {
                                                var tmpstrArray = formObjValue.split("/");
                                                var tmpYYYY = tmpstrArray[0];
                                                var tmpMM = tmpstrArray[1];
                                                var tmpDD = tmpstrArray[2];
                                                var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);
                                                if (tmpYYYY - 0 < 1850 || tmpYYYY - 0 != tmpDate.getFullYear() || tmpMM - 1 != tmpDate.getMonth() || tmpDD - 0 != tmpDate.getDate()) {
                                                    massage += "必須為正確日期字串<br>";
                                                }
                                            }
                                        }
                                        break;
                                    case "YM":
                                        if (!isNaN(value) && value - 0 > 0) {
                                            if (formObjValue.match(/^[0-9]{6}$/) == null) {
                                                massage += "必須為正確年月字串<br>";
                                            } else {
                                                var tmpstrArray = formObjValue.match(/^([0-9]{4})([0-9]{2})$/);
                                                var tmpYYYY = tmpstrArray[1];
                                                var tmpMM = tmpstrArray[2];
                                                var tmpDate = new Date("" + tmpMM + "/01/" + tmpYYYY);
                                                if (tmpYYYY - 0 < 1850 || tmpYYYY - 0 != tmpDate.getFullYear() || tmpMM - 1 != tmpDate.getMonth()) {
                                                    massage += "必須為正確年月字串<br>";
                                                }
                                            }
                                        }
                                        break;
                                    case "TP":
                                        if (!isNaN(value) && value - 0 > 0) {
                                            if (formObjValue.match(/^[0-2][0-9][:][0-5][0-9][:][0-5][0-9]$/) == null) {
                                                massage += "必須為正確時分秒字串<br>";
                                            } else {
                                                var tmpstrArray = formObjValue.match(/^([0-2][0-9])[:]([0-5][0-9])[:]([0-5][0-9])$/);
                                                var tmpHOUR = tmpstrArray[1];
                                                var tmpMINUTE = tmpstrArray[2];
                                                var tmpSECOND = tmpstrArray[3];
                                                if (tmpHOUR - 0 > 23 || tmpMINUTE - 0 > 59 || tmpSECOND - 0 > 59) {
                                                    massage += "必須為正確時分秒字串<br>";
                                                }
                                            }
                                        }
                                        break;
                                    case "PW":
                                        if (!isNaN(value) && value - 0 > 0 && formObjValue.length > 0 && (formObjValue.length < 8  || !(formObjValue.match(/[a-zA-Z]/) != null && formObjValue.match(/[0-9]/) != null))) {  
                                            massage += "密碼需為英文、數字混合8位數(含)以上<br>";
                                        }
                                        else if(!isNaN(value) && value - 0 > 0 && formObjValue.length > 0 && (formObjValue.length > 16  || !(formObjValue.match(/[a-zA-Z]/) != null && formObjValue.match(/[0-9]/) != null))){
                                            massage += "密碼需為英文、數字混合16位數以下<br>";
                                        }
                                        break;
                                }
                            });
                        }
                        if (massage != "") {
							$(formObj).vs_showDivAlert({ alertMassage: massage });
                        }
                    }
                }
            });
        },
        vs_scroll_Focus: function (options) {
            var settings = $.extend({ showDivAlert: false }, options || {});
            var obj = $(this).filter(":first");
            var posLRTB = obj.vs_find_LR_TB_WH();
            var alertDiv_posLRTB = { L: 0, R: 0, T: 0, B: 0, W: 0, H: 0 };
            if (settings.showDivAlert) {
                var alertDiv = $(obj.prop("vs_divAlertObj"));
                if (alertDiv.length > 0) {
                    alertDiv_posLRTB = alertDiv.vs_find_LR_TB_WH();
                    if (alertDiv_posLRTB.H > $(window).height() - posLRTB.H) {
                        alertDiv_posLRTB.H = $(window).height() - posLRTB.H;
                    }
                    if (alertDiv_posLRTB.W > $(window).width()) {
                        alertDiv_posLRTB.W = $(window).width();
                    }
                }
            }
            var min_scrollTop_value = (posLRTB.B - 0) + (alertDiv_posLRTB.H - 0) - $(window).height();//posLRTB.B
            var min_scrollLeft_value = (posLRTB.L - 0) + (alertDiv_posLRTB.W - 0) - $(window).width();//posLRTB.L
            if (min_scrollTop_value < 0) {
                min_scrollTop_value = 0;
            }
            if (min_scrollLeft_value < 0) {
                min_scrollLeft_value = 0;
            }
            var max_scrollTop_value = (posLRTB.T - 0);
            var max_scrollLeft_value = (posLRTB.L - 0);
            var targetTop_value = Math.floor((min_scrollTop_value + max_scrollTop_value) / 2);
            var targetLeft_value = Math.floor((min_scrollLeft_value + max_scrollLeft_value) / 2);
            if ($(document).scrollTop() > max_scrollTop_value && targetTop_value < $(window).height() / 2) {
                targetTop_value = 0;
            }
            if ($(document).scrollLeft() > max_scrollLeft_value && targetLeft_value < $(window).width() / 2) {
                targetLeft_value = 0;
            }
            if ($(document).scrollTop() < min_scrollTop_value || $(document).scrollTop() > max_scrollTop_value) {
                try { $(document).scrollTop(targetTop_value); } catch (ignore) { }
            }
            if ($(document).scrollLeft() < min_scrollLeft_value || $(document).scrollLeft() > max_scrollLeft_value) {
                try { $(document).scrollLeft(targetLeft_value); } catch (ignore) { }
            }
            try { obj.trigger("focus"); } catch (ignore) { }
            return $.jQuerize(this);
        },
        vs_findDivAlert_Close: function (options) {
            var settings = $.extend({}, options || {});
            var alertDiv = null;
            if ((alertDiv = $(this).find("div.divAlert")).length > 0) {
                alertDiv.each(function () { $(this).stop().remove(); });
            }
            alertDiv = null;
            if ((alertDiv = $(this).filter("div.divAlert")).length > 0) {
                alertDiv.each(function () { $(this).stop().remove(); });
            }

            $(this).each(function () {
                alertDiv = $(this).prop("vs_divAlertObj");
                if (alertDiv) {
                    $(alertDiv).stop().remove();
                    $(this).removeProp("vs_divAlertObj");
                }
            });

            return $.jQuerize(this);
        },
        vs_findDivAlertParent: function (options) {
            var settings = $.extend({}, options || {});
            return $.jQuerize($(this).filter(":first").prop("vs_divAlertParentObj"));
        },
        vs_showDivAlert: function (options) {
            var settings = $.extend({ alertMassage: "", append: false, prepend: false, resetZindex: false, shiftZindex: 0, orderBy: "desc" }, options || {});
            var size = $(this).length;
            var alertDivSeed;
            if (!$("body").prop("vs_divAlertSeedObj")) {
                alertDivSeed = $('<div></div>').addClass("divAlert")
				.append('<div class="divAlertArrowPlan' + ($.browser.isIE6 ? ' bgOrangeLight' : '') + '"><div class="divAlertArrowLine bgRed"><!-- --></div></div>')
				.append('<div class="divAlertContent bgYallow borderColorRed txtBlack"></div>').on("click", function (event) { $(this).stop().remove(); });
                $("body").prop("vs_divAlertSeedObj", alertDivSeed.clone(true));
                alertDivSeed.remove();
            }
            alertDivSeed = $("body").prop("vs_divAlertSeedObj");
            return $.jQuerize(this).each(function (idx) {
                var linkingClassName = "divAlertId__" + $(this).prop("id") + "__";
                var alertDiv = $(this).prop("vs_divAlertObj");
                if (settings.append || settings.prepend) {
                    if (alertDiv && alertDiv.find("div.divAlertContent").length > 0) {
                        var oldMassage = alertDiv.find("div.divAlertContent").html() + "<br>";
                        if (settings.append) {
                            settings.alertMassage = oldMassage + settings.alertMassage;
                        } else if (settings.prepend) {
                            settings.alertMassage = settings.alertMassage + oldMassage;
                        }
                    }
                }
                if (alertDiv) { $(alertDiv).stop().remove(); }

                //tab判斷
                var inTab = false;
                var inActiveTab = false;
                if ($("div.ui-tabs:first").length > 0) {
                    var tabContent = $(this).parents("div.ui-tabs-panel:first");
                    var tabRoot = tabContent.parents("div.ui-tabs:first");
                    var tabContent_idx = tabRoot.children("div.ui-tabs-panel").index(tabContent);
                    var active_idx = tabRoot.tabs("option", "active");
                    if (tabRoot.length > 0) {//formObj在tab裡
                        inTab = true;
                        inActiveTab = (active_idx == tabContent_idx);
                    }
                    if (inTab && !inActiveTab && !$(this).attr("readOnly") && !$(this).attr("disabled") && $(this).prop("type") != "hidden") {
                        var tabHeader=tabRoot.children("ul.ui-tabs-nav:first").children("li").eq(tabContent_idx);
                        tabHeader.addClass("ui-state-error");//tab標頭上紅色
                        return true;
                    }
                }
                //tab判斷

                if (!$(this).is(":visible") && !$(this).hasClass("chosen-select")) {
                    return true;
                }
                if (("" + settings.alertMassage).length > 0 && ($(this).is(":visible") || $(this).hasClass("chosen-select"))) {
                    var inputType = ("" + $(this).prop("type")).toLowerCase();
                    //var posLRTB = $(this).vs_find_LR_TB_WH();// .css({top: (posLRTB.B + "px"), left: ((posLRTB.L - 0 + 3) + "px")})
                    var showOnObj = this;
                    if ($(this).hasClass("chosen-select")) {
                        showOnObj = $(this).next("div.chosen-container");
                    }
                    if ($(this).is("div.validate_check")) {
                        showOnObj = $(this).find("button.btn_checkbox:first");
                    }
                    alertDiv = alertDivSeed.clone(true).addClass(linkingClassName).find("div.divAlertContent").html("" + settings.alertMassage).end().insertAfter(showOnObj).css({ opacity: 0 }).position({ collision: "none", my: "left top", at: "left bottom", of: showOnObj }).zIndex(size - idx + settings.shiftZindex).prop("vs_divAlertParentObj", $(this)).fadeTo(300, 1, $.noop);
                    $(this).prop("vs_divAlertObj", alertDiv);

                    if (inputType.match(/^(text|textarea|password)$/) != null) {
                        $(this).one("focusout", function (event) { alertDiv.stop().remove(); });//<input>使用blur
                    } else if ($(this).hasClass("chosen-select")) {//chosen-select
                        $(this).one("change focusout", function (event) { alertDiv.stop().remove(); });//<select>使用focusout
                        $(showOnObj).one("focusout", function (event) { alertDiv.stop().remove(); });//div
                    } else if (inputType.match(/^(select-one)$/) != null) {
                        $(this).one("change focusout", function (event) { alertDiv.stop().remove(); });//<select>使用focusout
                    } else if ($(this).is("div.validate_check")) {//validate_check
                        $(this).find("button.btn_checkbox").one("click", function (event) { alertDiv.stop().remove(); });//div
                    }
                }
            }).delay(0, function () {
                $.vs_showDivAlert({ resetZindex: settings.resetZindex, shiftZindex: settings.shiftZindex, orderBy: settings.orderBy });
            });
        },
        vs_find_LR_TB_WH: function (options) {
            var settings = $.extend({}, options || {});
            var obj = $(this).filter(":first");
            var tmpLR_TB_WH = { L: 0, R: 0, T: 0, B: 0, W: 0, H: 0 };
            if (obj.outerWidth() != null) {
                tmpLR_TB_WH.W = obj.outerWidth();
            }
            if (obj.outerHeight() != null) {
                tmpLR_TB_WH.H = obj.outerHeight();
            }
            var isPrev = false;
            var isUp = false;
            while (obj.length > 0 && !obj.is(":visible")) {
                if (obj.next().length > 0) {
                    obj = obj.next();
                    isPrev = false;
                } else if (obj.prev().length > 0) {
                    obj = obj.prev();
                    isPrev = true;
                } else if (obj.parent().length > 0) {
                    obj = obj.parent();
                    isPrev = false;
                    isUp = true;
                } else {
                    isPrev = false;
                    isUp = true;
                    break;
                }
            }
            if (obj.offset() != null) {
                tmpLR_TB_WH.L = obj.offset().left;
                tmpLR_TB_WH.T = obj.offset().top;
                if (isPrev && obj.outerWidth() != null) {
                    tmpLR_TB_WH.L += obj.outerWidth() - tmpLR_TB_WH.W;
                }
                if (isUp && obj.outerHeight() != null) {
                    tmpLR_TB_WH.T += obj.outerHeight() - tmpLR_TB_WH.H;
                }
            }
            tmpLR_TB_WH.R = tmpLR_TB_WH.L + tmpLR_TB_WH.W;
            tmpLR_TB_WH.B = tmpLR_TB_WH.T + tmpLR_TB_WH.H;
            return $.each(tmpLR_TB_WH, function (idx, value) { tmpLR_TB_WH[idx] = value.toFixed(0); });
        },
        vs_doSubmit: function (options) {
            var settings = $.extend({ actionPage: "", targetWin: "_self", postMethod: "post" }, options || {});
            return $.jQuerize(this).each(function () {
                if ($(this).prop("tagName").toLowerCase() == "form") {
                    $(this).find(':input:not(button,input:button)').vs_setName();
                    settings.actionPage = ("" + settings.actionPage).replace(/\?dummy.*$/i, "") + "?dummy=" + (new Date()).getTime() + "&random=" + ("" + Math.random()).substring(2);
                    $(this).attr("action", settings.actionPage).attr("target", settings.targetWin).attr("method", settings.postMethod);

                    if ($.browser.mozilla) {//firefox sometimes skip(!!!) form.submit() call,create a new submit element then fire click event on this element
                        var tmp_form = $(this);
                        var tmp_submit = $('<input type="submit" value="temporary" style="display:none">').appendTo(tmp_form);
                        tmp_submit.trigger("click").remove();
                    } else {
                        this.submit();//just submit with no event
                    }

                }
            });
        },
        vs_setName: function (options) {
            var settings = $.extend({}, options || {});
            return $.jQuerize(this).each(function () {
                var inputType = ("" + $(this).prop("type")).toLowerCase();
                var inputId = "" + $(this).prop("id");
                var inputValue = (($(this).val()) ? ("" + $(this).val()) : "");
                if (inputId != "" && $(this).filter(':input').length > 0 && inputType.match(/^(button|radio|checkbox)$/) == null) {
                    if (inputType.match(/^(textarea|select-multiple)$/) == null) {
                        inputValue = inputValue.replace(/[\x00\f\n\r]/g, "");//clearEnter
                    }
                    if (inputType == "select-one") {
                        if ($(this).prevAll('[name=' + inputId + ']:first').length <= 0) {
                            $('input[type=hidden][name=' + inputId + ']').remove();
                            $("<input type=hidden>").attr("name", inputId).val(inputValue).insertBefore(this);
                        } else {
                            $(this).prevAll('[name=' + inputId + ']:first').val(inputValue);
                        }
                    } else if (inputType == "select-multiple") {
                        if (inputId.match(/\[\]$/) == null) {
                            try { $(this).attr("name", inputId + "[]"); } catch (ignore) { }
                        } else {
                            try { $(this).attr("name", inputId); } catch (ignore) { }
                        }
                    } else {
                        try { $(this).attr("name", inputId); } catch (ignore) { }
                    }
                }
            });
        },
        vs_initialForm: function (options) {
            var settings = $.extend({}, options || {});
            return $.jQuerize(this).each(function () {
                var formObj = this;
                var formObjTag = ("" + $(formObj).prop("tagName")).toLowerCase();
                if (formObjTag.match(/^(input|select|textarea|button)$/i) != null) {
                    var inputType = ("" + $(formObj).prop("type")).toLowerCase();
                    if (formObjTag == "button") { inputType == "button"; }
                    var tmpJSONObject = null;
                    if ($.isEmptyObject(settings)) {
                        var tmpMatchArr = $(formObj).prop("class").match(/\x76\x73\x5Fi\x6Ei\x74i\x61\x6C\x46\x6F\x72\x6D\x28(\{[,\{\}0-9A-Za-z: ]*?\})\)(?= +(?!,)|$)/);
                        if (tmpMatchArr != null) {
                            var tmpJSONString = "" + tmpMatchArr[1].replace(/[ ]/g, "").replace(/([\{,])([0-9A-Z]*?)(:)/ig, "$1\"$2\"$3").replace(/(:)([0-9A-Z]*?)([,\}])/ig, "$1\"$2\"$3");
                            try { tmpJSONObject = $.parseJSON(tmpJSONString.toUpperCase()); } catch (ignore) { }
                        }
                    } else {
                        tmpJSONObject = settings;
                    }
                    if (tmpJSONObject != null) {
                        $.each(tmpJSONObject, function (name, value) {// this==value==tmpObject[name]
                            switch (name) {
                                case "M":
                                    if (!isNaN(value) && value - 0 > 0) {
                                        if (inputType.match(/^(text|password)$/i) != null) {
                                            $(formObj).attr("maxLength", value - 0 + "");
                                        } else if (inputType.match(/^(textarea)$/i) != null) {
                                            $(formObj).off('.vs_initialForm_' + name);
                                            if (!isNaN($(formObj).prop("maxLength"))) {
                                                $(formObj).prop("maxLength", value - 0);
                                            } else {
                                                $(formObj).on('keypress.vs_initialForm_' + name, function (event) {
                                                    var chr = String.fromCharCode(event.charCode == undefined ? event.keyCode : event.charCode);
                                                    return event.ctrlKey || (chr < ' ' || $(this).val().length < value);
                                                });
                                                $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().substr(0, value)); }); });//opera not support copy paste series event on <input type=text>
                                                $(formObj).on('dragover.vs_initialForm_' + name, function (event) { event.preventDefault(); return false; });//drop may not fire when dragover fired in some browser
                                                $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().substr(0, value)); }); });//"Text" case sensitive
                                            }
                                        }
                                    }
                                    break;
                                case "S":
                                    if (inputType.match(/^(text|password)$/i) != null) {
                                        //$(formObj).attr("size",value-0);//input.size
                                        //$(formObj).css("width", (value * 0.7).toFixed(2) + "em");//input.style.width
                                        //$(formObj).css("width",(value*1.2).toFixed(2)+"ex");//input.style.width
                                    }
                                    break;
                                    /*case "W":
                                            $(formObj).css("width",value-0);
                                    break;*/
                                case "TA":
                                    if (inputType.match(/^(text|password|textarea)$/i) != null) {
                                        switch (value) {
                                            case "R":
                                                $(formObj).css("textAlign", "right");//input.style
                                                break;
                                            case "C":
                                                $(formObj).css("textAlign", "center");//input.style
                                                break;
                                            case "L":
                                                $(formObj).css("textAlign", "left");//input.style
                                                break;
                                            default:
                                                $(formObj).css("textAlign", "");//input.style
                                        }
                                    }
                                    break;
                                case "D":
                                    try {
                                        if (!isNaN(value) && value - 0 > 0) {
                                            $(formObj).filter('.date-picker').datepicker("disable").attr("disabled", false);
                                            $(formObj).filter('.month-picker').datepicker("disable").attr("disabled", false);
                                            $(formObj).filter('.year-picker').datepicker("disable").attr("disabled", false);
                                            $(formObj).filter('.roc-date-picker-show').next('.roc-date-picker-hide').datepicker("disable").attr("disabled", false);
                                            $(formObj).filter('.roc-month-picker-show').next('.roc-month-picker-hide').datepicker("disable").attr("disabled", false);
                                            $(formObj).filter('.roc-year-picker-show').next('.roc-year-picker-hide').datepicker("disable").attr("disabled", false);
                                            if (inputType.match(/^(text|password)$/i) != null) {
                                                $(formObj).attr("readOnly", true);
                                                $(formObj).addClass("bgReadOnly");
                                                $(formObj).addClass("borderReadOnly");
                                                if ($(formObj).is(":text[class*=tp_dialogOf]")){
                                                    $(formObj).next("input:button.tp_dialogButton").button("option", { disabled: true });
                                                }
                                            } else if (inputType.match(/^(textarea)$/i) != null) {
                                                $(formObj).attr("readOnly", true);
                                                $(formObj).addClass("bgReadOnly");
                                                $(formObj).addClass("borderReadOnly");
                                            } else if (inputType.match(/^(button)$/i) != null) {
                                                $(formObj).attr("disabled", true);
                                                $(formObj).addClass("ui-state-disabled");
                                            } else if (inputType.match(/^(checkbox)$/i) != null) {
                                                if ($(formObj).hasClass("multiple_check") || $(formObj).hasClass("single_check")) {
                                                    $(formObj).next("button.btn_checkbox").attr("disabled", true).addClass("ui-state-disabled")
                                                        .next("span").attr("disabled", true);
                                                }
                                                $(formObj).attr("disabled", true);
                                            } else if (inputType.match(/^(radio)$/i) != null) {
                                                $(formObj).attr("disabled", true);
                                            } else if (inputType.match(/^(hidden)$/i) != null) {
                                                //do nothing
                                            } else {
                                                $(formObj).attr("disabled", true);
                                                $(formObj).addClass("bgReadOnly");
                                            }
                                        } else {
                                            $(formObj).filter('.date-picker').datepicker("enable");
                                            $(formObj).filter('.month-picker').datepicker("enable");
                                            $(formObj).filter('.year-picker').datepicker("enable");
                                            $(formObj).filter('.roc-date-picker-show').next('.roc-date-picker-hide').datepicker("enable");
                                            $(formObj).filter('.roc-month-picker-show').next('.roc-month-picker-hide').datepicker("enable");
                                            $(formObj).filter('.roc-year-picker-show').next('.roc-year-picker-hide').datepicker("enable");
                                            if (inputType.match(/^(text|password)$/i) != null) {
                                                $(formObj).attr("readOnly", false);
                                                $(formObj).removeClass("bgReadOnly");
                                                $(formObj).removeClass("borderReadOnly");
                                                if ($(formObj).is("input:text[class*=tp_dialogOf]")){
                                                    $(formObj).next("input:button.tp_dialogButton").button("option", { disabled: false });
                                                }
                                            } else if (inputType.match(/^(textarea)$/i) != null) {
                                                $(formObj).attr("readOnly", false);
                                                $(formObj).removeClass("bgReadOnly");
                                                $(formObj).removeClass("borderReadOnly");
                                            } else if (inputType.match(/^(button)$/i) != null) {
                                                $(formObj).attr("disabled", false);
                                                $(formObj).removeClass("ui-state-disabled");
                                            } else if (inputType.match(/^(checkbox)$/i) != null) {
                                                if ($(formObj).hasClass("multiple_check") || $(formObj).hasClass("single_check")) {
                                                    $(formObj).next("button.btn_checkbox").attr("disabled", false).removeClass("ui-state-disabled")
                                                        .next("span").attr("disabled", false);
                                                }
                                                $(formObj).attr("disabled", false);
                                            } else if (inputType.match(/^(radio)$/i) != null) {
                                                $(formObj).attr("disabled", false);
                                            } else if (inputType.match(/^(hidden)$/i) != null) {
                                                //do nothing
                                            } else {
                                                $(formObj).attr("disabled", false);
                                                $(formObj).removeClass("bgReadOnly");
                                            }

                                        }
                                        if ($(formObj).hasClass("chosen-select")) {
                                            $(formObj).trigger("chosen:updated");
                                        }
                                    } catch (iii) { }
                                    break;
                                case "N":
                                    if (inputType.match(/^(text|password|textarea)$/i) != null && !isNaN(value) && value - 0 > 0) {
                                        $(formObj).off('.vs_initialForm_' + name);
                                        var chars = "1234567890";
                                        var tmpRegex = new RegExp("[^" + chars + "]", "g");
                                        $(formObj).on('keypress.vs_initialForm_' + name, function (event) {
                                            var chr = String.fromCharCode(event.which);
                                            return event.ctrlKey || (chr < ' ' || !chars || chars.indexOf(chr) > -1);
                                        });
                                        $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//opera not support copy paste series event on <input type=text>
                                        $(formObj).on('dragover.vs_initialForm_' + name, function (event) { event.preventDefault(); return false; });//drop may not fire when dragover fired in some browser
                                        $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//"Text" case sensitive
                                    }
                                    break;
                                case "NQ":
                                    if (inputType.match(/^(text|password|textarea)$/i) != null && !isNaN(value) && value - 0 > 0) {
                                        $(formObj).off('.vs_initialForm_' + name);
                                        var chars = "-1234567890";
                                        var tmpRegex = new RegExp("[^" + chars + "]", "g");
                                        $(formObj).on('keypress.vs_initialForm_' + name, function (event) {
                                            var chr = String.fromCharCode(event.which);
                                            //var chr = String.fromCharCode(event.charCode == undefined ? event.keyCode : event.charCode);
                                            return event.ctrlKey || (chr < ' ' || !chars || chars.indexOf(chr) > -1);
                                        });
                                        $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//opera not support copy paste series event on <input type=text>
                                        $(formObj).on('dragover.vs_initialForm_' + name, function (event) { event.preventDefault(); return false; });//drop may not fire when dragover fired in some browser
                                        $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//"Text" case sensitive
                                    }
                                    break;
                                case "DP":
                                    if (inputType.match(/^(text|password|textarea)$/i) != null && !isNaN(value) && value - 0 > 0) {
                                        $(formObj).off('.vs_initialForm_' + name);
                                        var chars = "1234567890.";
                                        var tmpRegex = new RegExp("[^" + chars + "]", "g");
                                        $(formObj).on('keypress.vs_initialForm_' + name, function (event) {
                                            var chr = String.fromCharCode(event.which);
                                            return event.ctrlKey || (chr < ' ' || !chars || chars.indexOf(chr) > -1);
                                        });
                                        $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//opera not support copy paste series event on <input type=text>
                                        $(formObj).on('dragover.vs_initialForm_' + name, function (event) { event.preventDefault(); return false; });//drop may not fire when dragover fired in some browser
                                        $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//"Text" case sensitive
                                    }
                                    break;
                                case "DC":
                                    if (inputType.match(/^(text|password|textarea)$/i) != null && !isNaN(value) && value - 0 > 0) {
                                        $(formObj).off('.vs_initialForm_' + name);
                                        var chars = "-1234567890.";
                                        var tmpRegex = new RegExp("[^" + chars + "]", "g");
                                        $(formObj).on('keypress.vs_initialForm_' + name, function (event) {
                                            var chr = String.fromCharCode(event.which);
                                            return event.ctrlKey || (chr < ' ' || !chars || chars.indexOf(chr) > -1);
                                        });
                                        $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//opera not support copy paste series event on <input type=text>
                                        $(formObj).on('dragover.vs_initialForm_' + name, function (event) { event.preventDefault(); return false; });//drop may not fire when dragover fired in some browser
                                        $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//"Text" case sensitive
                                    }
                                    break;
                                case "EN":
                                    if (inputType.match(/^(text|password|textarea)$/i) != null && ((!isNaN(value) && value - 0 > 0) || value.match(/^(U|L)$/i) != null)) {
                                        $(formObj).off('.vs_initialForm_' + name);
                                        var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                                        var tmpRegex = new RegExp("[^" + chars + "]", "g");
                                        $(formObj).on('keypress.vs_initialForm_' + name, function (event) {
                                            var chr = String.fromCharCode(event.which);
                                            return event.ctrlKey || (chr < ' ' || !chars || chars.indexOf(chr) > -1);
                                        });
                                        $(formObj).on('dragover.vs_initialForm_' + name, function (event) { event.preventDefault(); return false; });//drop may not fire when dragover fired in some browser
                                        switch (value) {
                                            case "U": case "u":
                                                $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().toUpperCase().replace(tmpRegex, "")); }); });//opera not support copy paste series event on <input type=text>
                                                $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().toUpperCase().replace(tmpRegex, "")); }); });//"Text" case sensitive
                                                $(formObj).on('keyup.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { if ($(this).val().match(/[a-z]/) != null) { $(this).val("" + $(this).val().toUpperCase()) } }); });//"Text" case sensitive
                                                break;
                                            case "L": case "l":
                                                $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().toLowerCase().replace(tmpRegex, "")); }); });//opera not support copy paste series event on <input type=text>
                                                $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().toLowerCase().replace(tmpRegex, "")); }); });//"Text" case sensitive
                                                $(formObj).on('keyup.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { if ($(this).val().match(/[A-Z]/) != null) { $(this).val("" + $(this).val().toLowerCase()) } }); });//"Text" case sensitive
                                                break;
                                            default:
                                                $(formObj).on('paste.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//opera not support copy paste series event on <input type=text>
                                                $(formObj).on('drop.vs_initialForm_' + name, function (event) { $(this).delay(1, function () { $(this).val("" + $(this).val().replace(tmpRegex, "")); }); });//"Text" case sensitive
                                        }
                                    }
                                    break;
                                default:
                            }
                        });
                    }
                }
            });
        },
        tp_dialogOf: function (strType) {
            return $.jQuerize(this).each(function () {
                var formObj = $(this);
                var formObjTag = ("" + formObj.prop("tagName")).toLowerCase();
                if (formObjTag.match(/^(input)$/i) != null) {
                    var dialogType = "";
                    if (typeof (strType)!="string") {
                        var tmpMatchArr = formObj.prop("class").match(/\x74\x70\x5F\x64\x69\x61\x6C\x6F\x67\x4F\x66\x28([0-9A-Za-z]*?)\)(?= +|$)/);
                        if (tmpMatchArr != null) {
                            dialogType = tmpMatchArr[1];
                        }
                    } else if (typeof (strType) == "string") {
                        dialogType = "" + strType;
                    }
                    //console.log(dialogType);
                    var horizontalPadding = 30;
                    var verticalPadding = 30;
                    var iframW = 800;
                    var iframH = 460;
                    var description = "", code_column = "", name_column = "",actionPathQuery="",ajaxPathQuery="";
                    var id = 0;
                    switch (dialogType) {
                        case "Cntr2":                       //EQCK - 用戶用電
                            if (id == 0) { id = 1; } 
                        case "Cntr3":                       //FDCS - 饋線自動化
                            if (id == 0) { id = 2; } 
                        case "Cntr4":                       //SUBS - (發)變電設備汰換
                            if (id == 0) { id = 3; } 
                        case "Cntr":                        //DCMS - 施工管理(配電)
                            description = "承攬商";
                            code_column = "CntrNo";
                            name_column = "CntrName";
                            actionPathQuery = "/COMMON/GET_CODE/GetCntr_Query/" + id;
                            ajaxPathQuery = "/COMMON/GET_CODE/AjaxGetCntrName/" + id;
                            break;
                        case "Cntc":
                            description = "契約";
                            code_column = "CntcNo";
                            name_column = "CntcName";
                            actionPathQuery = "/COMMON/GET_CODE/GetCntc_Query";
                            ajaxPathQuery = "/COMMON/GET_CODE/AjaxGetCntcName";
                            break;
                        case "CnMc":
                            description = "契約雜項";
                            code_column = "DataSeq";
                            name_column = "MiscName";
                            actionPathQuery = "/COMMON/GET_CODE/GetCntcMisc_Query";
                            ajaxPathQuery = "/COMMON/GET_CODE/AjaxGetCntcMiscName";
                            break;
                        case "User":
                            description = "員工";
                            code_column = "UserId";
                            name_column = "UserName";
                            actionPathQuery = "/COMMON/GET_CODE/GetUser_Query";
                            ajaxPathQuery = "/COMMON/GET_CODE/AjaxGetUserName";
                            break;
                        default:
                        return;
                    }
                    var tp_dialogDesc_element = formObj.nextAll("input:text.tp_dialogDesc:first");
                    var tp_dialogButton_element = $('<input type="button" class="tp_dialogButton" value="...">')
                    .insertAfter(formObj).button().on("click", function () {
                        //var $this = $(this);
                        window.dialogReturnStr = "";
                        $('#NewPage').removeClass("hidden").dialog({
                            title: "選擇" + description,
                            autoOpen: true,
                            width: iframW,
                            height: iframH,
                            modal: true,
                            resizable: true,
                            close: function () {
                                if (dialogReturnStr != "") {
                                    try { dialogReturnObj = $.parseJSON(dialogReturnStr); } catch (ignore) { }
                                    if (dialogReturnObj != null)
                                    {
                                        formObj.val(dialogReturnObj[0][code_column]).trigger("blur");
                                        //tp_dialogDesc_element.val(dialogReturnObj[0][name_column]);
                                    }
                                }
                                $('#NewPage').remove();
                                $('<iframe id="NewPage" name="NewPage" class="hidden"></iframe>').appendTo('body');
                            }
                        }).width(iframW - horizontalPadding).height(iframH - verticalPadding);
                        $('form:first').vs_doSubmit({ actionPage: actionPathQuery, targetWin: "NewPage", postMethod: "post" });
                    });
                    formObj.prop("tp_dialog_oldValue","").prop("tp_dialogDesc_element", tp_dialogDesc_element).prop("tp_dialogButton_element", tp_dialogButton_element)
                    .on("blur", function () {
                        var formObjVal = "" + formObj.val();
                        var formReguestData = {};
                        try { formReguestData = $.parseJSON("{\"" + code_column + "\":\"" + formObjVal + "\"}"); } catch (ignore) { }
                        if (formObjVal==""){
                            tp_dialogDesc_element.val("");
                            formObj.prop("tp_dialog_oldValue", "");
                        }else if (formObjVal != "" && formObjVal != formObj.prop("tp_dialog_oldValue")) {//all keys not null
                            $.ajax({
                                url: ajaxPathQuery,
                                data: formReguestData,
                                beforeSend: function (xhr) {
                                    tp_dialogDesc_element.val("");
                                    formObj.prop("tp_dialog_oldValue", "");
                                },
                                success: function (responseData, textStatus, xhr) {//responseData may equal to xhr.responseText
                                    //alert(responseData);
                                    var xmlhttpResponseObj = null;
                                    //try { xmlhttpResponseObj = $.parseJSON(responseData); } catch (ignore) { }
                                    try { xmlhttpResponseObj = JSON.parse(responseData) } catch (ignore) { }
                                    if (xmlhttpResponseObj != null) {//query成功
                                        if (xmlhttpResponseObj.ToTalRow > 0) {
                                            var Data = JSON.parse(xmlhttpResponseObj.Data);
                                            //var Data = $.parseJSON(xmlhttpResponseObj.Data);
                                            tp_dialogDesc_element.val(Data[0]["" + name_column]);//formObjName=Data[0]["" + name_column]
                                            formObj.prop("tp_dialog_oldValue", formObjVal);
                                        } else {
                                            alert("" + description + "資料不存在!");
                                        }
                                    } else {
                                        alert("資料有誤,不能確定是否已存在!");
                                    }
                                },
                                error: function (xhr, textStatus, errorThrown) {
                                    if (textStatus == "timeout") {
                                        textStatus += "=" + this.timeout;
                                    }
                                    alert("網路有誤(" + textStatus + "),不能確定是否已存在!");
                                },
                                complete: function (xhr, textStatus) {//after success and error
                                }
                            });
                        }
                    });
                }
            });
        },
        //max add file upload
        tp_fileUpload: function () {
            return $.jQuerize(this).each(function () {
                var formObj = $(this);
                var horizontalPadding = 30;
                var verticalPadding = 30;
                var iframW = 280;
                var iframH = 260;
                var actionPathQuery = "/COMMON/FileUpload/Index";
                formObj.button().on("click", function () {
                    window.dialogReturnStr = "";
                    $('#NewPage').removeClass("hidden").dialog({
                        title: "上傳",
                        autoOpen: true,
                        width: iframW,
                        height: iframH,
                        modal: true,
                        resizable: true,
                        close: function () {
                            if (dialogReturnStr != "") {
                                try { dialogReturnObj = $.parseJSON(dialogReturnStr); } catch (ignore) { }
                                if (dialogReturnObj != null) {
                                    alert(dialogReturnObj.attachmentID + "  " + dialogReturnObj.fileName);
                                }
                            }
                            $('#NewPage').remove();
                            $('<iframe id="NewPage" name="NewPage" class="hidden"></iframe>').appendTo('body');
                        }
                    }).width(iframW - horizontalPadding).height(iframH - verticalPadding);
                    $('form:first').vs_doSubmit({ actionPage: actionPathQuery, targetWin: "NewPage", postMethod: "post" });
                });
            });
        },
        dummy: $.noop
    });
    //串列function end
})(jQuery);//iniFormSet


// 欄位連動取得 Ajax 資料 -- Added by Aaron
function getAjaxData(curId, refId, triggerId, pageId, asyncSts, hideCode) {
    if (curId == null) { curId = ""; }
    if (refId == null) { refId = ""; }
    if (triggerId == null) { triggerId = ""; }
    if (pageId == null) { pageId = ""; }
    if (curId == "") { return; }
    if (asyncSts == null) asyncSts = true;
    if (!hideCode) hideCode = false;

    //alert(curId);
    var curVal = $("#" + curId).val();
    var oldVal = $("#" + curId).prop("oldValue");
    if (oldVal == null) {
        oldVal = $("#" + curId).prop("defaultValue");
        if (oldVal == null) {
            oldVal = "";
        }
        $("#" + curId).prop("oldValue", oldVal);
    }
    //if (curVal == oldVal) { return; }//no change  marked by KellyTao 2016.08.10

    $("#" + curId).prop("oldValue", curVal);
    //if (curVal != "") {//需清空連動欄位
        var Data = new Object();
        Data[curId] = curVal;
        //alert(refIds);
        if (refId != "") {
            var refIds = refId.split(";");
            $.each(refIds, function (i, item) {
                Data[item] = $("#" + item).val();
            });
        }
    $.ajax({
            type: 'POST',
            url: "AjaxGet" + pageId + curId,
            data: Data,
            async: asyncSts,
            success: function (responseData, textStatus, xhr) {
                //var responseObj = $.parseJSON(responseData);
                //var Data = $.parseJSON(responseObj.Data);
                var responseObj = JSON.parse(responseData);
                var Data = JSON.parse(responseObj.Data);
                $.each(Data, function (i, item) {
                    var elementName = $("#" + i);
                    var tagType = elementName.get(0).tagName;
                    switch (tagType) {
                        case "INPUT":
                            elementName.val(item);
                            break;
                        case "SELECT":
                            elementName.find("option").remove().end().append($("<option></option>"));
                            $.each(item, function (key, val) {
                                if (hideCode)
                                    elementName.append($("<option></option>").val(val.Code).text(val.Name));
                                else
                                    elementName.append($("<option></option>").val(val.Code).text(val.Code + " " + val.Name));
                            });
                            break;
                        default:
                            elementName.val(item);
                            break;
                    }
                    if (elementName.hasClass('chosen-select')) {
                        elementName.val('').trigger("chosen:updated");
                    }
                    
                });
                //Grid
                if (responseObj.GridIndex != null) {
                    //var GridIndex = $.parseJSON(responseObj.GridIndex);
                    //var GridAction = $.parseJSON(responseObj.GridAction);
                    var GridIndex = JSON.parse(responseObj.GridIndex);
                    var GridAction = JSON.parse(responseObj.GridAction);
                    if (GridIndex == null) { GridIndex = ""; }
                    if (GridAction == null) { GridAction = ""; }
                    if (GridIndex != "") {
                        var Grids = GridIndex.split(";");
                        $.each(Grids, function (i, item) {
                            switch (GridAction) {
                                case "Append":
                                    //remove EmptyRows
                                    if ($("#tbResults" + item).handsontable('getInstance').countEmptyRows() > 0) {
                                        $("#tbResults" + item).handsontable('getInstance').getData().length = $("#tbResults" + item).handsontable('getInstance').countRows() - $("#tbResults" + item).handsontable('getInstance').countEmptyRows();
                                    }
                                    for (var i = 0; i < $.parseJSON(responseObj[item]).length; i++) {
                                        $("#tbResults" + item).handsontable('getInstance').getData().push($.parseJSON(responseObj[item])[i]);
                                    }
                                    break;
                                default:
                                    $("#tbResults" + item).handsontable('getInstance').loadData($.parseJSON(responseObj[item]));
                                    break;
                            }
                        });
                    }
                }
                var triggerIdObj = '<%=Encoder.encodeForJS(triggerId)%>';
                if (triggerId != "")
                    $("#" + triggerIdObj).blur();
            },
            error: function (xhr, textStatus, errorThrown) {
                if (textStatus == "timeout") {
                    textStatus += "=" + this.timeout;
                }

                var curIdObj = '<%=Encoder.encodeForJS(curId)%>';
                $("#" + curIdObj).vs_showDivAlert({ alertMassage: "網路有誤(" + textStatus + "),不能確定" + $("#" + curIdObj).attr("title") + "(" + curVal + ")是否已存在!" });
                $("#" + curIdObj).val("" + $("#" + curIdObj).prop("defaultValue"));
                $("#" + curIdObj).delay(9, function () { $(this).trigger("focus"); });
            }
        });
    //}
}

function checkDateFormat(date) {//驗證日期格式yyyy/MM/dd
    if (date.match(/^[0-9]{4}[/][0-9]{2}[/][0-9]{2}$/) == null) {
        return false;
    } else {
        return true;
    }
}

//日期差異判斷
Date.prototype.dateDiff = function (interval, objDate) {
    var dtEnd = new Date(objDate);
    if (isNaN(dtEnd)) return undefined;
    switch (interval) {
        case "s": return parseInt((dtEnd - this) / 1000);
        case "n": return parseInt((dtEnd - this) / 60000);
        case "h": return parseInt((dtEnd - this) / 3600000);
        case "d": return parseInt((dtEnd - this) / 86400000);
        case "w": return parseInt((dtEnd - this) / (86400000 * 7));
        case "m": return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - this.getFullYear()) * 12) - (this.getMonth() + 1);
        case "y": return dtEnd.getFullYear() - this.getFullYear();
    }
}

$(document).ready(function () {//取消預設行為
    $(document).on("click", "button", null, function (event) { event.preventDefault(); });//修正非預期 form submit
    $(document).on("keydown", "input:not(:button)", null, function (event) { if (event.which == 13) { event.preventDefault(); } });//修正非預期 form submit
    $.browser = {};
    $.browser.mozilla = /mozilla/.test(navigator.userAgent.toLowerCase()) && !/webkit/.test(navigator.userAgent.toLowerCase());
    $.browser.webkit = /webkit/.test(navigator.userAgent.toLowerCase());
    $.browser.opera = /opera/.test(navigator.userAgent.toLowerCase());
    $.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());

    if ($.browser.msie) {
        window.status = window.status.replace(/[ ]*$/, "") + " ";//避免status bar 讀不完
        $("body").delay(1000, function () {
            window.status += " ";//window.status.replace(/[ ]*$/,"")+" ";//避免status bar 讀不完
        });
    }
    $(document).on("keydown keypress", function (event) {
        if (event.ctrlKey && String.fromCharCode(event.which).match(/[Ss]/) != null) {//Ctrl+S 存檔熱鍵
            event.preventDefault();//屏蔽非IE Ctrl+S網頁存檔熱鍵
        }
    });

    //同步$.syncWin_WH.docWidth,$.syncWin_WH.docHeight
    $.syncWin_Initial();
    $(window).on("load resize", $.syncWin_WH);

    //<input type="checkbox" class="multiple_check"><button class="btn_checkbox">選</button><span>NAME</span>
    //<input type="checkbox" class="single_check"><button class="btn_checkbox">選</button><span>NAME</span>
    var chkB = $("input:checkbox.multiple_check ,input:checkbox.single_check").addClass("hidden")
	.each(function (idx, elem) {
	    $(elem).next("span").addBack().wrapAll("<span class=\"noWrap\"></span>");
	});

    chkB.filter(":checked").after("<button checked class=\"btn_checkbox\">選</button>").next("button.btn_checkbox")
	.addClass("ui-state-highlight").button({ icons: { primary: 'ui-icon-check' }, label: "選取", text: false });

    chkB.not(":checked").after("<button class=\"btn_checkbox\">選</button>").next("button.btn_checkbox")
	.removeClass("ui-state-highlight").button({ icons: { primary: 'ui-icon-blank' }, label: "選取", text: false });

    chkB.next("button.btn_checkbox").on("click.btn_checkbox", function (event) {
        var btnItem = $(this);
        var should_check = !(btnItem.hasClass("ui-state-highlight"));

        if (should_check) {
            btnItem.button("option", { icons: { primary: 'ui-icon-check' } }).addClass("ui-state-highlight")
			.prev("input:checkbox").prop("checked", true);

            var sgl_chkItem = btnItem.prev("input:checkbox.single_check");
            if (sgl_chkItem.length > 0) {
                var grpName = sgl_chkItem.prop("name");
                grpItems = $("input:checkbox.single_check[name=" + grpName + "]").not(sgl_chkItem)
				.filter(":checked").prop("checked", false).next("button.btn_checkbox")
				.removeClass("ui-state-highlight").button({ icons: { primary: 'ui-icon-blank' } });
            }
        } else {
            btnItem.button("option", { icons: { primary: 'ui-icon-blank' } }).removeClass("ui-state-highlight")
			.prev("input:checkbox").prop("checked", false);
        }
        //alert(btnItem.prev("input:checkbox").is(":checked"));
        //alert(btnItem.parent().prop("tagName"));
    }).next("span").css({ "cursor": "pointer" }).on("click.btn_checkbox", function (event) { $(this).prev("button.btn_checkbox").trigger("click") });
    
    $('[class*=tp_dialogOf]').tp_dialogOf();
    $('[class*=tp_fileUpload]').tp_fileUpload();
});

//base on jQuery UI - v1.11.2 - 2014-10-16
(function($){//ui date picker modify//http://jqueryui.com/demos/datepicker/
if(!$.datepicker || !$.datepicker.regional){return;}
//http://api.jqueryui.com/datepicker/
$.datepicker.regional['zh-TW'] = {
	closeText: '關閉',
	prevText: '&#x3c;上月',
	nextText: '下月&#x3e;',
	currentText: '今天',
	monthNames: ['　一月','　二月','　三月','　四月','　五月','　六月',
	'　七月','　八月','　九月','　十月','十一月','十二月'],
	monthNamesShort: ['01','02','03','04','05','06',
	'07','08','09','10','11','12'],
	dayNames: ['星期日','星期一','星期二','星期三','星期四','星期五','星期六'],
	dayNamesShort: ['周日','周一','周二','周三','周四','周五','周六'],
	dayNamesMin: ['日','一','二','三','四','五','六'],
	weekHeader: '周',
	isRTL: false,
	showMonthAfterYear: true,
	yearSuffix: '年　',
	
	yearRange: 'c-10:c+5',
	dateFormat: 'yymmdd',
	changeMonth: true,
	changeYear: true,
	showOn: 'button',
	buttonImage: '/Content/images/calendar.gif',
	buttonImageOnly:true,
	buttonText:'日期選取',

	showOtherMonths:true,
	selectOtherMonths:true,
	onClose:function(dateText, inst){try{inst.input.trigger("focus");}catch(ignore_err){}},
	
	firstDay: 0,
	showAnim:''
};
$.datepicker.setDefaults($.datepicker.regional['zh-TW']);

(function($){//ui date time picker addon//http://trentrichardson.com/examples/timepicker/
if(!$.timepicker || !$.timepicker.regional){return;}
$.timepicker.regional['zh-TW'] = {
	timeOnlyTitle: '設定時間',
	timeText: '時間',
	hourText: '時',
	minuteText: '分',
	secondText: '秒',
	currentText: '現在',
	closeText: '關閉',
	showSecond: true,
	timeFormat: 'hh:mm:ss',
	ampm: false
};
$.timepicker.setDefaults($.timepicker.regional['zh-TW']);
})(jQuery);//ui date time picker addon

//$.datepicker.noWeekends Extend new function noSundays noSaturdays
$.datepicker.noSundays=function(date) {
	var day = date.getDay();
	return [(day > 0 && day < 7), ""];
};
$.datepicker.noSaturdays=function(date) {
	var day = date.getDay();
	return [(day > -1 && day < 6), ""];
};

//position fix
$.datepicker._base_checkOffset=$.datepicker._base_checkOffset ? $.datepicker._base_checkOffset : $.datepicker._checkOffset;//backup function
$.datepicker._checkOffset=function(inst, offset, isFixed) {//overwrite by new function
	var dpWidth = inst.dpDiv.outerWidth(),
		dpHeight = inst.dpDiv.outerHeight(),
		inputWidth = inst.input ? inst.input.outerWidth() : 0,
		inputHeight = inst.input ? inst.input.outerHeight() : 0,
/*//2014-12-19 losewin
		viewWidth = document.documentElement.clientWidth + (isFixed ? 0 : $(document).scrollLeft()),
		viewHeight = document.documentElement.clientHeight + (isFixed ? 0 : $(document).scrollTop());
*/
		viewWidth = $(document).width() + (isFixed ? 0 : $(document).scrollLeft()),
		viewHeight = $(document).height() + (isFixed ? 0 : $(document).scrollTop());

	offset.left -= (this._get(inst, "isRTL") ? (dpWidth - inputWidth) : 0);
	offset.left -= (isFixed && offset.left === inst.input.offset().left) ? $(document).scrollLeft() : 0;
	offset.top -= (isFixed && offset.top === (inst.input.offset().top + inputHeight)) ? $(document).scrollTop() : 0;

	// now check if datepicker is showing outside window viewport - move to a better place if so.
	offset.left -= Math.min(offset.left, (offset.left + dpWidth > viewWidth && viewWidth > dpWidth) ?
		Math.abs(offset.left + dpWidth - viewWidth) : 0);
	offset.top -= Math.min(offset.top, (offset.top + dpHeight > viewHeight && viewHeight > dpHeight) ?
		Math.abs(dpHeight + inputHeight) : 0);

	return offset;
};

//check "yyyymmdd" date "20141231" is valid//return boolean
//min:"00000101",max:"99991231"
$.datepicker.checkCEDateValid = function (dateStr, format) {
    if (typeof (format) == "string") {
        format = format.replace(/^yyyy/,"yy");
    }
    if (format == null) {
        format = "yymmdd";
    }
    if (typeof (format) != "string" && format != "yymmdd" && format != "yymm" && format != "yy") {
        return false;
    }
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        var tmpYYYY = null;
        var tmpMM = null;
        var tmpDD = null;
        if ((tmpstrArray = dateStr.match(/^([0-9]{4})([0-9]{2})([0-9]{2})$/)) != null) {//L=8
            tmpYYYY = tmpstrArray[1];
            tmpMM = tmpstrArray[2];
            tmpDD = tmpstrArray[3];
        } else if ((tmpstrArray = dateStr.match(/^([0-9]{4})([0-9]{2})$/)) != null) {//L=6
            tmpYYYY = tmpstrArray[1];
            tmpMM = tmpstrArray[2];
        } else if ((tmpstrArray = dateStr.match(/^([0-9]{4})$/)) != null) {//L=4
            tmpYYYY = tmpstrArray[1];
        }
        if (tmpYYYY != null) {
            var tmpDate = null;
            try {
                tmpDate = new Date("" + (tmpMM || "01") + "/" + (tmpDD || "01") + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
            } catch (ignore) { }
            if (isNaN(tmpDate)) {
            } else if (tmpYYYY - tmpDate.getFullYear() != 0) {
            } else if ((tmpMM || 1) - tmpDate.getMonth() != 1) {
            } else if ((tmpDD || 1) - tmpDate.getDate() != 0) {
            } else if (tmpYYYY - 0 < 100) {//datepicker min:"01000101"
            } else {
                return true;
            }
        }
    }
    return false;
};
/*$.datepicker.checkCEDateValid = function (dateStr) {
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        if ((tmpstrArray = dateStr.match(/^([0-9]{4})([0-9]{2})([0-9]{2})$/)) != null) {
            var tmpYYYY = tmpstrArray[1];
            var tmpMM = tmpstrArray[2];
            var tmpDD = tmpstrArray[3];
            var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
            if (isNaN(tmpDate)) {
            } else if (tmpYYYY - tmpDate.getFullYear() != 0) {
            } else if (tmpMM - tmpDate.getMonth() != 1) {
            } else if (tmpDD - tmpDate.getDate() != 0) {
            } else if (tmpYYYY - 0 < 100) {//datepicker min:"01000101"
            } else {
                return true;
            }
        }
    }
    return false;
};*/
$.datepicker.check8DigitDateValid = $.datepicker.checkCEDateValid;
//min(1813):"-990101",max:"9991231",roc-start(1912):"0010101"
//1911:"-01"
//1911:"-00","000"
$.datepicker.checkRocDateValid = function (dateStr, format) {
    if (format == null) {
        format = "yyymmdd";
    }
    if (typeof (format) != "string" && format != "yyymmdd" && format != "yyymm" && format != "yyy") {
        return false;
    }
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        var tmpS = null;
        var tmpYY = null;
        var tmpMM = null;
        var tmpDD = null;
        if (format == "yyymmdd" && (tmpstrArray = dateStr.match(/^(-?)([0-9]{2,3})([0-9]{2})([0-9]{2})$/)) != null) {//L=6,7
            tmpS = tmpstrArray[1];
            tmpYY = tmpstrArray[2];
            tmpMM = tmpstrArray[3];
            tmpDD = tmpstrArray[4];
        } else if (format == "yyymm" && (tmpstrArray = dateStr.match(/^(-?)([0-9]{2,3})([0-9]{2})$/)) != null) {//L=4,5
            tmpS = tmpstrArray[1];
            tmpYY = tmpstrArray[2];
            tmpMM = tmpstrArray[3];
        } else if (format == "yyy" && (tmpstrArray = dateStr.match(/^(-?)([0-9]{1,3})$/)) != null) {//L=1,2,3
            tmpS = tmpstrArray[1];
            tmpYY = tmpstrArray[2];
        }
        if (tmpYY != null) {
            var tmpYYY = tmpS + tmpYY;
            if (tmpYYY.length <= 3) {
                var delta = 1911;
                if (tmpS == "-") {
                    tmpYYY = -1 * (tmpYY - 0);
                    delta = 1912;
                }
                if (tmpYY - 0 == 0) {
                    tmpYYY = -1;
                    delta = 1912;
                }
                var tmpYYYY = tmpYYY - 0 + delta;
                var tmpDate = null;
                try {
                    tmpDate = new Date("" + (tmpMM || "01") + "/" + (tmpDD || "01") + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
                } catch (ignore) { }
                if (isNaN(tmpDate)) {
                } else if (tmpDate.getFullYear() - (tmpYYY - 0) != delta) {
                } else if ((tmpMM || 1) - tmpDate.getMonth() != 1) {
                } else if ((tmpDD || 1) - tmpDate.getDate() != 0) {
                } else if (tmpYYYY - 0 < 1813 || tmpYYYY - 0 > 2910) {
                } else {
                    return true;
                }
            }
        }
    }
    return false;
};
/*$.datepicker.checkRocDateValid = function (dateStr) {
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        if ((tmpstrArray = dateStr.match(/^(-?)([0-9]{1,3})([0-9]{2})([0-9]{2})$/)) != null) {
            var tmpS = tmpstrArray[1];
            var tmpYY = tmpstrArray[2];
            var tmpMM = tmpstrArray[3];
            var tmpDD = tmpstrArray[4];
            var tmpYYY = tmpS + tmpYY;
            if (tmpYYY.length <= 3) {
                var delta = 1911;
                if (tmpS == "-") {
                    tmpYYY = -1 * (tmpYY - 0);
                    delta = 1912;
                }
                if (tmpYY - 0 == 0) {
                    tmpYYY = -1;
                    delta = 1912;
                }
                var tmpYYYY = tmpYYY - 0 + delta;
                var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
                if (isNaN(tmpDate)) {
                } else if (tmpDate.getFullYear() - (tmpYYY - 0) != delta) {
                } else if (tmpMM - tmpDate.getMonth() != 1) {
                } else if (tmpDD - tmpDate.getDate() != 0) {
                } else {
                    return true;
                }
            }
        }
    }
    return false;
};*/

//min(1813):"-990101",max:"9991231",roc-start(1912):"0010101"
//1911:"-01"
//1911:"-00","000"
$.datepicker.rocDateToCE = function (dateStr) {
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        var tmpS = null;
        var tmpYY = null;
        var tmpMM = null;
        var tmpDD = null;
        if ((tmpstrArray = dateStr.match(/^(-?)([0-9]{2,3})([0-9]{2})([0-9]{2})$/)) != null) {//L=6,7
            tmpS = tmpstrArray[1];
            tmpYY = tmpstrArray[2];
            tmpMM = tmpstrArray[3];
            tmpDD = tmpstrArray[4];
        } else if ((tmpstrArray = dateStr.match(/^(-?)([0-9]{2,3})([0-9]{2})$/)) != null) {//L=4,5
            tmpS = tmpstrArray[1];
            tmpYY = tmpstrArray[2];
            tmpMM = tmpstrArray[3];
        } else if ((tmpstrArray = dateStr.match(/^(-?)([0-9]{1,3})$/)) != null) {//L=1,2,3
            tmpS = tmpstrArray[1];
            tmpYY = tmpstrArray[2];
        }
        if (tmpYY != null) {
            var tmpYYY = tmpS + tmpYY;
            if (tmpYYY.length <= 3) {
                var delta = 1911;
                if (tmpS == "-") {
                    tmpYYY = -1 * (tmpYY - 0);
                    delta = 1912;
                }
                if (tmpYY - 0 == 0) {
                    tmpYYY = -1;
                    delta = 1912;
                }
                var tmpYYYY = tmpYYY - 0 + delta;
                var tmpDate = null;
                try {
                    tmpDate = new Date("" + (tmpMM || "01") + "/" + (tmpDD || "01") + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
                } catch (ignore) { }
                if (isNaN(tmpDate)) {
                } else if (tmpDate.getFullYear() - (tmpYYY - 0) != delta) {
                } else if ((tmpMM || 1) - tmpDate.getMonth() != 1) {
                } else if ((tmpDD || 1) - tmpDate.getDate() != 0) {
                } else if (tmpYYYY - 0 < 1813 || tmpYYYY - 0 > 2910) {
                } else {
                    return "" + tmpYYYY + (tmpMM || "") + (tmpDD || "");
                }
            }
        }
    }
    return "";
};
$.datepicker.ceDateToRoc = function (dateStr) {
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        var tmpYYYY = null;
        var tmpMM = null;
        var tmpDD = null;
        if ((tmpstrArray = dateStr.match(/^([0-9]{4})([0-9]{2})([0-9]{2})$/)) != null) {//L=8
            tmpYYYY = tmpstrArray[1];
            tmpMM = tmpstrArray[2];
            tmpDD = tmpstrArray[3];
        } else if ((tmpstrArray = dateStr.match(/^([0-9]{4})([0-9]{2})$/)) != null) {//L=6
            tmpYYYY = tmpstrArray[1];
            tmpMM = tmpstrArray[2];
        } else if ((tmpstrArray = dateStr.match(/^([0-9]{4})$/)) != null) {//L=4
            tmpYYYY = tmpstrArray[1];
        }
        if (tmpYYYY != null) {
            var tmpDate = null;
            try {
                tmpDate = new Date("" + (tmpMM || "01") + "/" + (tmpDD || "01") + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
            } catch (ignore) { }
            if (isNaN(tmpDate)) {
            } else if (tmpYYYY - tmpDate.getFullYear() != 0) {
            } else if ((tmpMM || 1) - tmpDate.getMonth() != 1) {
            } else if ((tmpDD || 1) - tmpDate.getDate() != 0) {
            } else if (tmpYYYY - 0 < 1813 || tmpYYYY - 0 > 2910) {
            } else {
                var tmpYYY = (tmpYYYY >= 1912 ? tmpYYYY - 1911 : tmpYYYY - 1912);
                if (tmpYYY < 0 && tmpYYY > -10) {
                    tmpYYY = "-0" + (-1 * tmpYYY);
                }
                tmpYYY = "000" + tmpYYY;
                tmpYYY = tmpYYY.substring(tmpYYY.length - 3);
                return "" + tmpYYY + (tmpMM || "") + (tmpDD || "");
            }
        }
    }
    return "";
};
/*$.datepicker.rocDateToCE = function (dateStr) {
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        if ((tmpstrArray = dateStr.match(/^(-?)([0-9]{1,3})([0-9]{2})([0-9]{2})$/)) != null) {
            var tmpS = tmpstrArray[1];
            var tmpYY = tmpstrArray[2];
            var tmpMM = tmpstrArray[3];
            var tmpDD = tmpstrArray[4];
            var tmpYYY = tmpS + tmpYY;
            if (tmpYYY.length <= 3) {
                var delta = 1911;
                if (tmpS == "-") {
                    tmpYYY = -1 * (tmpYY - 0);
                    delta = 1912;
                }
                if (tmpYY - 0 == 0) {
                    tmpYYY = -1;
                    delta = 1912;
                }
                var tmpYYYY = tmpYYY - 0 + delta;
                var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
                if (isNaN(tmpDate)) {
                } else if (tmpDate.getFullYear() - (tmpYYY - 0) != delta) {
                } else if (tmpMM - tmpDate.getMonth() != 1) {
                } else if (tmpDD - tmpDate.getDate() != 0) {
                } else {
                    return "" + tmpYYYY + tmpMM + tmpDD;
                }
            }
        }
    }
    return "";
};
$.datepicker.ceDateToRoc = function (dateStr) {
    if (typeof (dateStr) === "string") {
        var tmpstrArray = null;
        if ((tmpstrArray = dateStr.match(/^([0-9]{4})([0-9]{2})([0-9]{2})$/)) != null) {
            var tmpYYYY = tmpstrArray[1];
            var tmpMM = tmpstrArray[2];
            var tmpDD = tmpstrArray[3];
            var tmpDate = new Date("" + tmpMM + "/" + tmpDD + "/" + tmpYYYY);//IE must be "mm/dd/yyyy"
            if (isNaN(tmpDate)) {
            } else if (tmpYYYY - tmpDate.getFullYear() != 0) {
            } else if (tmpMM - tmpDate.getMonth() != 1) {
            } else if (tmpDD - tmpDate.getDate() != 0) {
            } else if (tmpYYYY - 0 < 1813 || tmpYYYY - 0 > 2910) {
            } else {
                var tmpYYY = (tmpYYYY >= 1912 ? tmpYYYY - 1911 : tmpYYYY - 1912);
                if (tmpYYY < 0 && tmpYYY > -10) {
                    tmpYYY = "-0" + (-1 * tmpYYY);
                }
                tmpYYY = "000" + tmpYYY;
                tmpYYY = tmpYYY.substring(tmpYYY.length - 3);
                return "" + tmpYYY + tmpMM + tmpDD;
            }
        }
    }
    return "";
};*/
//https://jquery-ui.googlecode.com/svn/tags/1.8.24/docs/datepicker.html#theming
//monthpicker//<input type="text" id="" class="month-picker">//$('.month-picker').monthpicker();
$.datepicker._base_generateHTML=$.datepicker._base_generateHTML ? $.datepicker._base_generateHTML : $.datepicker._generateHTML;//backup function
$.datepicker._generateHTML=function(inst){//overwrite by new function
	var justYM=!!(this._get(inst,'justYM'));
	var justY = !!(this._get(inst, 'justY'));
	if (justYM) {
		return this._base_generateHTML(inst).replace(/( class=[\'\"]ui-datepicker-calendar[\'\"])/,"$1 style='display:none'");
	} else if (justY) {
	    return this._base_generateHTML(inst).replace(/( class=[\'\"]ui-datepicker-month[\'\"])/, "$1 style='visibility:hidden'").replace(/( class=[\'\"]ui-datepicker-calendar[\'\"])/, "$1 style='display:none'");
	} else {
		return this._base_generateHTML(inst);
	}
};
$.datepicker._setYearMonth=function(inst){
	var date=new Date(inst.selectedYear,inst.selectedMonth,1);
	var dateFormat=this._get(inst,'dateFormat');//dateFormat:'yymm'
	var yymm=this.formatDate(dateFormat,date);
	inst.input.val(yymm);
	
	var isRocYear=!!(this._get(inst,'isRocYear'));
	if(isRocYear){
		yymm=this.formatDate('yymm',date);
		var roc_yymm=$.datepicker.ceDateToRoc(yymm+"01").substr(0,5);
		inst.input.prop("rocMonthPickerView").val(roc_yymm).delay(1,function(){
			try{$(this).trigger("focus");}catch(ignore_err){}
		});
	}
};
$.datepicker._setYear = function (inst) {
    var date = new Date(inst.selectedYear, 0, 1);//0=Jan
    var dateFormat = this._get(inst, 'dateFormat');//dateFormat:'yy'
    var yy = this.formatDate(dateFormat, date);
    inst.input.val(yy);

    var isRocYear = !!(this._get(inst, 'isRocYear'));
    if (isRocYear) {
        yy = this.formatDate('yy', date);
        var roc_yy = $.datepicker.ceDateToRoc(yy + "0101").substr(0, 3);
        var tmpObj = inst.input.prop("rocYearPickerView").val(roc_yy);
        try { tmpObj.trigger("focus"); } catch (ignore_err) { }
    }
};

//http://stackoverflow.com/questions/2208480/jquery-ui-datepicker-to-show-month-year-only
$.fn.monthpicker=function(options){//2014-12-22 losewin
	var settings=$.extend({},options||{});
	return $.jQuerize(this).each(function(){
		$.jQuerize(this).datepicker($.extend({
			justYM:true,
			dateFormat: 'yymm',
			buttonText:'年月選取',
			showAnim:''
		},options||{},{
			beforeShow: function(input, inst){//$(this)===inst.input
				var dateFormat=inst.input.datepicker("option","dateFormat");//dateFormat:'yymm'
				
				var default_date=inst.input.datepicker("option","defaultDate");//datepicker defaultDate(Date or string or Number)
				var date=default_date;
				if(date instanceof String){
					try{date=$.datepicker.parseDate(dateFormat+"dd",date);}catch(ignore){}//server side default
				}
				if(!(date instanceof Date) || isNaN(default_date)){
					date=new Date();//client side default
				}
				if(!(default_date instanceof Date) || isNaN(default_date)){
					default_date=date;
				}
				
				var yymm=inst.input.val();//form value(String)
				var default_yymm=""+inst.input.prop("defaultValue");//form default(String)
				
				date=null;
				try{date=$.datepicker.parseDate(dateFormat+"dd",default_yymm+"01");}catch(ignore){}
				if(date==null){//form default not valid
					default_yymm=$.datepicker.formatDate(dateFormat,default_date);
					inst.input.prop("defaultValue",default_yymm);//fix form default
				}
				
				date=null;
				try{date=$.datepicker.parseDate(dateFormat+"dd",yymm+"01");}catch(ignore){}
				if(date==null){//form value not valid
					yymm=default_yymm;
					try{date=$.datepicker.parseDate(dateFormat+"dd",yymm+"01");}catch(ignore){}
				}
				
				inst.input.datepicker("option","defaultDate",date);//overwrite defaultDate hack//cause onChangeMonthYear								
				inst.input.val(yymm);//fix form value
			},
			onChangeMonthYear: function(year, month, inst){
				$.datepicker._setYearMonth(inst);
			},
			//onClose: function(dateText, inst){
			//	$.datepicker._setYearMonth(inst);
			//},
			justYM:true
		}));
	});
};
$.fn.yearpicker = function (options) {//2015-04-09 losewin
    var settings = $.extend({}, options || {});
    return $.jQuerize(this).each(function () {
        $.jQuerize(this).datepicker($.extend({
            justY: true,
            dateFormat: 'yy',
            buttonText: '年選取',
            showAnim: ''
        }, options || {}, {
            beforeShow: function (input, inst) {//$(this)===inst.input
                var dateFormat = inst.input.datepicker("option", "dateFormat");//dateFormat:'yy'

                var default_date = inst.input.datepicker("option", "defaultDate");//datepicker defaultDate(Date or string or Number)
                var date = default_date;
                if (date instanceof String) {
                    try { date = $.datepicker.parseDate(dateFormat + "mmdd", date); } catch (ignore) { }//server side default
                }
                if (!(date instanceof Date) || isNaN(default_date)) {
                    date = new Date();//client side default
                }
                if (!(default_date instanceof Date) || isNaN(default_date)) {
                    default_date = date;
                }

                var yy = inst.input.val();//form value(String)
                var default_yy = "" + inst.input.prop("defaultValue");//form default(String)

                date = null;
                try { date = $.datepicker.parseDate(dateFormat + "mmdd", default_yy + "0101"); } catch (ignore) { }
                if (date == null) {//form default not valid
                    default_yy = $.datepicker.formatDate(dateFormat, default_date);
                    inst.input.prop("defaultValue", default_yy);//fix form default
                }

                date = null;
                try { date = $.datepicker.parseDate(dateFormat + "mmdd", yy + "0101"); } catch (ignore) { }
                if (date == null) {//form value not valid
                    yy = default_yy;
                    try { date = $.datepicker.parseDate(dateFormat + "mmdd", yy + "0101"); } catch (ignore) { }
                }

                inst.input.datepicker("option", "defaultDate", date);//overwrite defaultDate hack//cause onChangeMonthYear								
                inst.input.val(yy);//fix form value
            },
            onChangeMonthYear: function (year, month, inst) {
                $.datepicker._setYear(inst);
            },
            //onClose: function(dateText, inst){
            //	$.datepicker._setYearMonth(inst);
            //},
            changeMonth: false,
            stepMonths: 12,//how many months to move when clicking the prev/next
            justY: true
        }));
    });
};

//rocdatepicker
$.datepicker._base_findPos=$.datepicker._base_findPos?$.datepicker._base_findPos:$.datepicker._findPos;//backup function
$.datepicker._findPos=function(obj){//overwrite by new function
	if(typeof($(obj).prop("rocDatePickerView"))==="object"){
		position = $(obj).prop("rocDatePickerView").offset();
		return [position.left, position.top];
	}else if(typeof($(obj).prop("rocMonthPickerView"))==="object"){
		position = $(obj).prop("rocMonthPickerView").offset();
		return [position.left, position.top];
	}else if(typeof ($(obj).prop("rocYearPickerView")) === "object"){
	    position = $(obj).prop("rocYearPickerView").offset();
	    return [position.left, position.top];
	}else{
		return this._base_findPos(obj);
	}
};
$.datepicker._base_generateMonthYearHeader=$.datepicker._base_generateMonthYearHeader?$.datepicker._base_generateMonthYearHeader:$.datepicker._generateMonthYearHeader;//backup function
$.datepicker._generateMonthYearHeader=function(inst, drawMonth, drawYear, minDate, maxDate,
			secondary, monthNames, monthNamesShort) {
	var isRocYear = !!(this._get(inst, 'isRocYear'));
	var inMinYear, inMaxYear, month, years, thisYear, determineYear, year, endYear,
		changeMonth = this._get(inst, "changeMonth"),
		changeYear = this._get(inst, "changeYear"),
		showMonthAfterYear = this._get(inst, "showMonthAfterYear"),
		html = "<div class='ui-datepicker-title'>",
		monthHtml = "";

	// month selection
	if (secondary || !changeMonth) {
		monthHtml += "<span class='ui-datepicker-month'>" + monthNames[drawMonth] + "</span>";
	} else {
		inMinYear = (minDate && minDate.getFullYear() === drawYear);
		inMaxYear = (maxDate && maxDate.getFullYear() === drawYear);
		monthHtml += "<select class='ui-datepicker-month' data-handler='selectMonth' data-event='change'>";
		for ( month = 0; month < 12; month++) {
			if ((!inMinYear || month >= minDate.getMonth()) && (!inMaxYear || month <= maxDate.getMonth())) {
				monthHtml += "<option value='" + month + "'" +
					(month === drawMonth ? " selected='selected'" : "") +
					">" + monthNamesShort[month] + "</option>";
			}
		}
		monthHtml += "</select>";
	}

	if (!showMonthAfterYear) {
		html += monthHtml + (secondary || !(changeMonth && changeYear) ? "&#xa0;" : "");
	}

	// year selection
	if ( !inst.yearshtml ) {
		inst.yearshtml = "";
		if (secondary || !changeYear) {
			html += "<span class='ui-datepicker-year'>" + drawYear + "</span>";
		} else {
			// determine range of years to display
			years = this._get(inst, "yearRange").split(":");
			thisYear = new Date().getFullYear();
			determineYear = function(value) {
				var year = (value.match(/c[+\-].*/) ? drawYear + parseInt(value.substring(1), 10) :
					(value.match(/[+\-].*/) ? thisYear + parseInt(value, 10) :
					parseInt(value, 10)));
				return (isNaN(year) ? thisYear : year);
			};
			year = determineYear(years[0]);
			endYear = Math.max(year, determineYear(years[1] || ""));
			year = (minDate ? Math.max(year, minDate.getFullYear()) : year);
			endYear = (maxDate ? Math.min(endYear, maxDate.getFullYear()) : endYear);
/*//2014-12-22 losewin
			inst.yearshtml += "<select class='ui-datepicker-year' data-handler='selectYear' data-event='change'>";
*/
			inst.yearshtml += "<select"+(isRocYear?" title='民國年'":"")+" class='ui-datepicker-year' data-handler='selectYear' data-event='change'>";
			for (; year <= endYear; year++) {
				inst.yearshtml += "<option value='" + year + "'" +
					(year === drawYear ? " selected='selected'" : "") +
/*//2014-12-22 losewin
					">" + year + "</option>";
*/
					">" + (isRocYear?(year>=1912?year-1911:year-1912):year) + "</option>";
			}
			inst.yearshtml += "</select>";

			html += inst.yearshtml;
			inst.yearshtml = null;
		}
	}

	html += this._get(inst, "yearSuffix");
	if (showMonthAfterYear) {
		html += (secondary || !(changeMonth && changeYear) ? "&#xa0;" : "") + monthHtml;
	}
	html += "</div>"; // Close datepicker_header
	return html;
};

//rocdatepicker
$.fn.rocdatepicker=function(options){//2014-12-22 losewin
	var settings=$.extend({},options||{});
	return $.jQuerize(this).each(function(){
		var ceObj=$.jQuerize(this);
		var rocObj = ceObj.prevAll(".roc-date-picker-show:first");
		var dayOfWeekObj = ceObj.prevUntil(rocObj,".roc-date-picker-day-of-week");
		if (!ceObj.hasClass("roc-date-picker-hide") || rocObj.length < 1) {
			return;
		}
		
		if(ceObj.datepicker("option","isRocYear")==null){//not init
			//bind each other
		    ceObj.prop("rocDatePickerView", rocObj).prop("rocDatePickerDayOfWeek", dayOfWeekObj).css({
				"padding-left":"0px","padding-right":"0px",
				"border-left":"0px","border-right":"0px",
				"margin-left":"0px","margin-right":"0px",
				"width":"0px"
			});
			rocObj.prop("rocDatePickerHide",ceObj).on("blur.rocdatepicker",function(){
			    var tmpObj = $(this);
			    if (tmpObj.val() == null || tmpObj.val() == "") {
			        tmpObj.prop("rocDatePickerHide").val("");
			    } else {
			        var tmpVal = $.datepicker.rocDateToCE(tmpObj.val());
			        var tmpDayOfWeek = "";
			        try { tmpDayOfWeek = $.datepicker.formatDate("DD", $.datepicker.parseDate("yymmdd", tmpVal)); } catch (ignore) { }
			        tmpObj.prop("rocDatePickerHide").val(tmpVal);
			        tmpObj.prop("rocDatePickerHide").prop("rocDatePickerDayOfWeek").val(tmpDayOfWeek);
			        if (tmpVal == "") {
			            tmpObj.val("");
			        }
			    }
			});
			
			//normalize defaultValue
			var ceDateString=ceObj.val();
			var rocDateString=rocObj.val();

			if($.datepicker.checkRocDateValid(rocDateString)){
				if((ceDateString=$.datepicker.rocDateToCE(rocDateString))==""){
					rocDateString="";
				}
			} else if ($.datepicker.checkCEDateValid(ceDateString)) {
				if((rocDateString=$.datepicker.ceDateToRoc(ceDateString))==""){
					ceDateString="";
				}
			}else{
				ceDateString="";
				rocDateString="";
			}
			
			if(ceObj.prop("defaultValue")!=ceDateString){
				ceObj.prop("defaultValue",ceDateString).val(ceDateString);
			}
			if (ceDateString!="") {
		        var tmpDayOfWeek = "";
			    try { tmpDayOfWeek = $.datepicker.formatDate("DD", $.datepicker.parseDate("yymmdd", ceDateString)); } catch (ignore) { }
			    ceObj.prop("rocDatePickerDayOfWeek").val(tmpDayOfWeek);
			}
			if(rocObj.prop("defaultValue")!=rocDateString){
				rocObj.prop("defaultValue",rocDateString).val(rocDateString);
			}
		}
		
		//datepicker init
		ceObj.datepicker($.extend({
			isRocYear:true,
			minDate: new Date(1813, 1 - 1, 1),
			maxDate: new Date(2910, 12 - 1, 31),
			buttonText:'民國日期選取',
			showAnim:''
		}, options || {}, {
		    altField: dayOfWeekObj,
		    altFormat: "DD",
			dateFormat: 'yymmdd',
			onClose: function(dateText, inst){
				var tmpObj=inst.input.prop("rocDatePickerView").val($.datepicker.ceDateToRoc(dateText));
				try{tmpObj.trigger("focus");}catch(ignore_err){}
			},
			isRocYear:true
		}));
	});
};

//rocmonthpicker
$.fn.rocmonthpicker=function(options){//2014-12-22 losewin
	var settings=$.extend({},options||{});
	return $.jQuerize(this).each(function(){
		var ceObj=$.jQuerize(this);
		var rocObj=ceObj.prevAll(".roc-month-picker-show:first");
		if(!ceObj.hasClass("roc-month-picker-hide") || rocObj.length<1){
			return;
		}
		
		if(ceObj.datepicker("option","isRocYear")==null){//not init
			//bind each other
			ceObj.prop("rocMonthPickerView",rocObj).css({
				"padding-left":"0px","padding-right":"0px",
				"border-left":"0px","border-right":"0px",
				"margin-left":"0px","margin-right":"0px",
				"width":"0px"
			});
			rocObj.prop("rocMonthPickerHide",ceObj).on("blur.rocmonthpicker",function(){
			    var tmpObj = $(this);
			    if (tmpObj.val() == null || tmpObj.val() == "") {
			        tmpObj.prop("rocMonthPickerHide").val("");
			    } else {
			        var tmpVal = $.datepicker.rocDateToCE(tmpObj.val() + "01");
			        tmpObj.prop("rocMonthPickerHide").val(tmpVal.substr(0, 6));
			        if (tmpVal == "") {
			            tmpObj.val("");
			        }
			    }
			});
			
			//normalize defaultValue
			var ceDateString=ceObj.val()+"01";
			var rocDateString=rocObj.val()+"01";
			if($.datepicker.checkRocDateValid(rocDateString)){
				if((ceDateString=$.datepicker.rocDateToCE(rocDateString))==""){
					rocDateString="";
				}
			} else if ($.datepicker.checkCEDateValid(ceDateString)) {
				if((rocDateString=$.datepicker.ceDateToRoc(ceDateString))==""){
					ceDateString="";
				}
			}else{
				ceDateString="";
				rocDateString="";
			}
			
			if(ceObj.prop("defaultValue")!=ceDateString.substr(0,6)){
				ceObj.prop("defaultValue",ceDateString.substr(0,6)).val(ceDateString.substr(0,6));
			}
			if(rocObj.prop("defaultValue")!=rocDateString.substr(0,5)){
				rocObj.prop("defaultValue",rocDateString.substr(0,5)).val(rocDateString.substr(0,5));
			}
		}
		
		//datepicker init
		ceObj.datepicker($.extend({
			justYM:true,
			isRocYear:true,
			minDate: new Date(1813, 1 - 1, 1),
			maxDate: new Date(2910, 12 - 1, 31),
			buttonText:'民國年月選取',
			showAnim:''
		},options||{},{
			dateFormat: 'yymm',
			beforeShow: function(input, inst){//$(this)===inst.input
				var default_date=inst.input.datepicker("option","defaultDate");//datepicker defaultDate(Date or string or Number)
				var date=default_date;
				if(date instanceof String){
					try{date=$.datepicker.parseDate("yymmdd",date);}catch(ignore){}//server side default
				}
				if(!(date instanceof Date) || isNaN(default_date)){
					date=new Date();//client side default
				}
				if(!(default_date instanceof Date) || isNaN(default_date)){
					default_date=date;
				}
				
				var yymm=inst.input.val();//form value(String)
				var default_yymm=""+inst.input.prop("defaultValue");//form default(String)
				
				date=null;
				try{date=$.datepicker.parseDate("yymmdd",default_yymm+"01");}catch(ignore){}
				if(date==null){//form default not valid
					default_yymm=$.datepicker.formatDate("yymm",default_date);
					inst.input.prop("defaultValue",default_yymm);//fix form default
				}
				
				date=null;
				try{date=$.datepicker.parseDate("yymmdd",yymm+"01");}catch(ignore){}
				if(date==null){//form value not valid
					yymm=default_yymm;
					try{date=$.datepicker.parseDate("yymmdd",yymm+"01");}catch(ignore){}
				}
				
				inst.input.datepicker("option","defaultDate",date);//overwrite defaultDate hack//cause onChangeMonthYear								
				
				var roc_yymm=$.datepicker.ceDateToRoc(yymm+"01").substr(0,5);
				inst.input.val(yymm).prop("rocMonthPickerView").val(roc_yymm);//fix form value
			},
			onChangeMonthYear: function(year, month, inst){
				$.datepicker._setYearMonth(inst);
			},
			//onClose: function(dateText, inst){
			//	$.datepicker._setYearMonth(inst);
			//},
			justYM:true,
			isRocYear:true
		}));
	});
};
//rocyearpicker
$.fn.rocyearpicker = function (options) {//2015-04-09 losewin
    var settings = $.extend({}, options || {});
    return $.jQuerize(this).each(function () {
        var ceObj = $.jQuerize(this);
        var rocObj = ceObj.prevAll(".roc-year-picker-show:first");
        if (!ceObj.hasClass("roc-year-picker-hide") || rocObj.length < 1) {
            return;
        }

        if (ceObj.datepicker("option", "isRocYear") == null) {//not init
            //bind each other
            ceObj.prop("rocYearPickerView", rocObj).css({
                "padding-left": "0px", "padding-right": "0px",
                "border-left": "0px", "border-right": "0px",
                "margin-left": "0px", "margin-right": "0px",
                "width": "0px"
            });
            rocObj.prop("rocYearPickerHide", ceObj).on("blur.rocyearpicker", function () {
                var tmpObj = $(this);
                if (tmpObj.val() == null || tmpObj.val() == "") {
                    tmpObj.prop("rocYearPickerHide").val("");
                }else{
                    var tmpVal = $.datepicker.rocDateToCE(tmpObj.val() + "0101");
                    tmpObj.prop("rocYearPickerHide").val(tmpVal.substr(0, 4));
                    if (tmpVal == "") {
                        tmpObj.val("");
                    }
                }
            });

            //normalize defaultValue
            var ceDateString = ceObj.val() + "0101";
            var rocDateString = rocObj.val() + "0101";
            if ($.datepicker.checkRocDateValid(rocDateString)) {
                if ((ceDateString = $.datepicker.rocDateToCE(rocDateString)) == "") {
                    rocDateString = "";
                }
            } else if ($.datepicker.checkCEDateValid(ceDateString)) {
                if ((rocDateString = $.datepicker.ceDateToRoc(ceDateString)) == "") {
                    ceDateString = "";
                }
            } else {
                ceDateString = "";
                rocDateString = "";
            }

            if (ceObj.prop("defaultValue") != ceDateString.substr(0, 4)) {
                ceObj.prop("defaultValue", ceDateString.substr(0, 4)).val(ceDateString.substr(0, 4));
            }
            if (rocObj.prop("defaultValue") != rocDateString.substr(0, 3)) {
                rocObj.prop("defaultValue", rocDateString.substr(0, 3)).val(rocDateString.substr(0, 3));
            }
        }

        //datepicker init
        ceObj.datepicker($.extend({
            justY: true,
            isRocYear: true,
            minDate: new Date(1813, 1 - 1, 1),
            maxDate: new Date(2910, 12 - 1, 31),
            buttonText: '民國年選取',
            showAnim: ''
        }, options || {}, {
            dateFormat: 'yy',
            beforeShow: function (input, inst) {//$(this)===inst.input
                var default_date = inst.input.datepicker("option", "defaultDate");//datepicker defaultDate(Date or string or Number)
                var date = default_date;
                if (date instanceof String) {
                    try { date = $.datepicker.parseDate("yymmdd", date); } catch (ignore) { }//server side default
                }
                if (!(date instanceof Date) || isNaN(default_date)) {
                    date = new Date();//client side default
                }
                if (!(default_date instanceof Date) || isNaN(default_date)) {
                    default_date = date;
                }

                var yy = inst.input.val();//form value(String)
                var default_yy = "" + inst.input.prop("defaultValue");//form default(String)

                date = null;
                try { date = $.datepicker.parseDate("yymmdd", default_yy + "0101"); } catch (ignore) { }
                if (date == null) {//form default not valid
                    default_yy = $.datepicker.formatDate("yy", default_date);
                    inst.input.prop("defaultValue", default_yy);//fix form default
                }

                date = null;
                try { date = $.datepicker.parseDate("yymmdd", yy + "0101"); } catch (ignore) { }
                if (date == null) {//form value not valid
                    yy = default_yy;
                    try { date = $.datepicker.parseDate("yymmdd", yy + "0101"); } catch (ignore) { }
                }

                inst.input.datepicker("option", "defaultDate", date);//overwrite defaultDate hack//cause onChangeMonthYear								

                var roc_yy = $.datepicker.ceDateToRoc(yy + "0101").substr(0, 3);
                inst.input.val(yy).prop("rocYearPickerView").val(roc_yy);;//fix form value
            },
            onChangeMonthYear: function (year, month, inst) {
                $.datepicker._setYear(inst);
            },
            //onClose: function(dateText, inst){
            //	$.datepicker._setYear(inst);
            //},
            changeMonth: false,
            stepMonths: 12,//how many months to move when clicking the prev/next
            justY: true,
            isRocYear: true
        }));
    });
};

})(jQuery);//ui date picker modify

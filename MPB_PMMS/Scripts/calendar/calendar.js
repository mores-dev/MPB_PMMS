"use strict";

function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function _iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }

(function () {
  //global method
    window.setCalendar = function (selectYear, selectMonth) {
        var allBlockCount = 7 * 6;
        var selectYearString = selectYear.toString();
        var selectMonthString = (selectMonth - 1).toString();
        var selectMonthInfo = calendarData.find(function (yearInfo) {
            return yearInfo.year === selectYear;
        }).months.find(function (monthInfo) {
            return monthInfo.month === selectMonth;
        });
        var selectOneDay = new Date(selectYearString, selectMonthString).getDay(); //這個月的第一天是禮拜幾
        var selectAllDateCount = new Date(
            selectYearString,
            selectMonth.toString(),
            0).getDate(); //取得選擇的月總共有幾天       三月的第零天 = 二月的最後一天

        var dBefore = selectOneDay; //日曆上會顯示幾天上個月的日子
        var dateText = 1;
        var index = 0;
    while (index < allBlockCount) {
        var dateDom = allDateDom[index];
        var dateDomText = dateDom.getElementsByClassName("date")[0];
        var dateDomInfo = dateDom.getElementsByClassName("info")[0];
        index++;

      if (dBefore > 0) {
        dateDom.className = "dateBlock";
        dateDomText.innerHTML = "";
        dateDomInfo.innerHTML = "";
        dBefore--;
      } else if (dateText <= selectAllDateCount) {
          var nowInfo = selectMonthInfo.dateData[dateText - 1];
        dateDom.className = "dateBlock ".concat(nowInfo.status);
        dateDomText.innerText = dateText;
        dateDomInfo.innerHTML = "".concat(nowInfo.now, "/").concat(nowInfo.upperLimit); //這邊帶入後端資料
        dateText++;
      } else {
        dateDom.className = "dateBlock";
        dateDomText.innerHTML = "";
        dateDomInfo.innerHTML = "";
      }
    }
    changeMonthText();
  };

    window.getSelectTime = function () {
    return { year: selectedYear, month: selectedMonth, date: selectedDate };
  };

    var getParameterByName = function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
      results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return "";
    return decodeURIComponent(results[2].replace(/\+/g, " "));
  };

    const changeMonthText = function changeMonthText() {
        calendarMonthText.innerText = "".concat(selectedYear, " / ").concat(selectedMonth);
  };

    var now = new Date();
    var yearParam = Number(getParameterByName("year"));
    var monthParam = Number(getParameterByName("month"));
    var dateParam = Number(getParameterByName("date"));
    var selectedYear = null;
    var selectedMonth = null;
    var selectedDate = !dateParam ? now.getDate() : dateParam; //目前沒用
    var allDateDom = _toConsumableArray(document.querySelectorAll(".calendar .dateBlock"));
    var calendarMonthText = document.querySelector(".month ul li span");

    var init = function init() {
    //查看有沒有參數 如果有再去查看此參數是否存在於calendarData中
    if (!yearParam) {
      selectedYear = calendarData[0].year;
      selectedMonth = calendarData[0].months[0].month;
      //selectedYear = calendarData.year;
      //selectedMonth = calendarData.months[0].month;
    } else if (!monthParam) {
        selectedYear = calendarData.some(function (yearInfo) {
            return  yearInfo.year === yearParam
        }) ? yearParam
        : calendarData[0].year;
        //: calendarData.year;
        selectedMonth = calendarData.find(function (yearInfo) {
            return yearInfo.year === selectedYear
        }).months[0].month;
    } else {
        selectedYear = calendarData.some(function (yearInfo) {
            return yearInfo.year === yearParam
        }) ? yearParam
        : calendarData[0].year;
        //: calendarData.year;
        selectedMonth = calendarData.find(function (yearInfo) {
            return yearInfo.year === selectedYear
        }).months.some(function (monthInfo) {
            return monthInfo.month === monthParam
        }) ? monthParam : calendarData.find(function (yearInfo) {
            return yearInfo.year === selectedYear
        }).months[0].month;
    }

    var searchBtn = $("#btnQuery");
    //const yearSelect = $("#Year");
    //const monthSelect = $("#Month");
    var addBtn = $("#btnAdd");
    var modifyBtn = $("#btnModify");
    searchBtn.click(function (e) {
      e.preventDefault();
      setCalendar(selectedYear, selectedMonth);
      $("#monthC").css("visibility", "visible");
      $("#weekC").css("visibility", "visible");
      $("#daysC").css("visibility", "visible");
      $("#daysB").css("visibility", "visible");
    });
    //addBtn.click(() => {
    //  location.href = 'uNightCalendar_Edit.htm?year=${selectedYear}&month=${selectedMonth}';
    //});
    //modifyBtn.click(() => {
    //  location.href = 'uNightCalendar_Edit.htm?year=${selectedYear}&month=${selectedMonth}';
    //});
    //yearSelect.change(() => {
    //  selectedYear = Number(yearSelect[0].value);
    //  changeMonthSelectOptions();
    //  selectedMonth = Number(monthSelect[0].value);
    //});
    //monthSelect.change(() => {
    //  selectedMonth = Number(monthSelect[0].value);
    //});
    //changeYearSelectOptions();
    //changeMonthSelectOptions();
    setCalendar(selectedYear, selectedMonth);
    //yearSelect[0].value = selectedYear;
    //monthSelect[0].value = selectedMonth;
  };

  //const changeYearSelectOptions = () => {
  //  const yearSelect = $("#yearSelect");
  //  yearSelect[0].options.length = 0;
  //  calendarData.forEach(yearInfo => {
  //    yearSelect[0].add(new Option('${yearInfo.year}年', yearInfo.year));
  //  });
  //};

  //const changeMonthSelectOptions = () => {
  //  const monthSelect = $("#monthSelect");
  //  monthSelect[0].options.length = 0;
  //  calendarData
  //    .find(yearInfo => yearInfo.year === selectedYear)
  //    .months.forEach(monthInfo => {
  //      monthSelect[0].add(new Option('${monthInfo.month}月', monthInfo.month));
  //    });
  //};

  //let calendarData = [];
  //new Promise(resolve => {
  //  setTimeout(() => {
  //    let response = '[{"year":2019,"months":[{"month":10,"dateData":[{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":120,"now":18,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":55,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":120,"now":72,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":34,"status":"active"},{"upperLimit":60,"now":31,"status":"active"},{"upperLimit":60,"now":21,"status":"active"},{"upperLimit":120,"now":8,"status":"active"},{"upperLimit":60,"now":9,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":60,"now":25,"status":"active"},{"upperLimit":60,"now":41,"status":"active"},{"upperLimit":120,"now":45,"status":"active"},{"upperLimit":120,"now":47,"status":"active"},{"upperLimit":60,"now":16,"status":"active"},{"upperLimit":60,"now":51,"status":"almost"},{"upperLimit":60,"now":2,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":60,"now":53,"status":"almost"},{"upperLimit":120,"now":43,"status":"active"},{"upperLimit":120,"now":31,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":120,"now":110,"status":"almost"},{"upperLimit":60,"now":21,"status":"active"}]},{"month":11,"dateData":[{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":120,"now":18,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":55,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":120,"now":72,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":34,"status":"active"},{"upperLimit":60,"now":31,"status":"active"},{"upperLimit":60,"now":21,"status":"active"},{"upperLimit":120,"now":8,"status":"active"},{"upperLimit":60,"now":9,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":60,"now":25,"status":"active"},{"upperLimit":60,"now":41,"status":"active"},{"upperLimit":120,"now":45,"status":"active"},{"upperLimit":120,"now":47,"status":"active"},{"upperLimit":60,"now":16,"status":"active"},{"upperLimit":60,"now":51,"status":"almost"},{"upperLimit":60,"now":2,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":60,"now":53,"status":"almost"},{"upperLimit":120,"now":43,"status":"active"},{"upperLimit":120,"now":31,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":120,"now":110,"status":"almost"}]},{"month":12,"dateData":[{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":120,"now":18,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":55,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":120,"now":72,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":34,"status":"active"},{"upperLimit":60,"now":31,"status":"active"},{"upperLimit":60,"now":21,"status":"active"},{"upperLimit":120,"now":8,"status":"active"},{"upperLimit":60,"now":9,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":60,"now":25,"status":"active"},{"upperLimit":60,"now":41,"status":"active"},{"upperLimit":120,"now":45,"status":"active"},{"upperLimit":120,"now":47,"status":"active"},{"upperLimit":60,"now":16,"status":"active"},{"upperLimit":60,"now":51,"status":"almost"},{"upperLimit":60,"now":2,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":60,"now":53,"status":"almost"},{"upperLimit":120,"now":43,"status":"active"},{"upperLimit":120,"now":31,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":120,"now":110,"status":"almost"},{"upperLimit":60,"now":21,"status":"active"}]}]},{"year":2020,"months":[{"month":1,"dateData":[{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":120,"now":18,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":55,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":120,"now":72,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":34,"status":"active"},{"upperLimit":60,"now":31,"status":"active"},{"upperLimit":60,"now":21,"status":"active"},{"upperLimit":120,"now":8,"status":"active"},{"upperLimit":60,"now":9,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":60,"now":25,"status":"active"},{"upperLimit":60,"now":41,"status":"active"},{"upperLimit":120,"now":45,"status":"active"},{"upperLimit":120,"now":47,"status":"active"},{"upperLimit":60,"now":16,"status":"active"},{"upperLimit":60,"now":51,"status":"almost"},{"upperLimit":60,"now":2,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":60,"now":53,"status":"almost"},{"upperLimit":120,"now":43,"status":"active"},{"upperLimit":120,"now":31,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":120,"now":110,"status":"almost"},{"upperLimit":60,"now":21,"status":"active"}]},{"month":2,"dateData":[{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":120,"now":18,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":55,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":120,"now":72,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":34,"status":"active"},{"upperLimit":60,"now":31,"status":"active"},{"upperLimit":60,"now":21,"status":"active"},{"upperLimit":120,"now":8,"status":"active"},{"upperLimit":60,"now":9,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":60,"now":25,"status":"active"},{"upperLimit":60,"now":41,"status":"active"},{"upperLimit":120,"now":45,"status":"active"},{"upperLimit":120,"now":47,"status":"active"},{"upperLimit":60,"now":16,"status":"active"},{"upperLimit":60,"now":51,"status":"almost"},{"upperLimit":60,"now":2,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":60,"now":53,"status":"almost"},{"upperLimit":120,"now":43,"status":"active"},{"upperLimit":120,"now":31,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":1,"status":"active"}]},{"month":3,"dateData":[{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":120,"now":18,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":55,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":120,"now":72,"status":"active"},{"upperLimit":60,"now":10,"status":"active"},{"upperLimit":60,"now":60,"status":"full"},{"upperLimit":60,"now":34,"status":"active"},{"upperLimit":60,"now":31,"status":"active"},{"upperLimit":60,"now":21,"status":"active"},{"upperLimit":120,"now":8,"status":"active"},{"upperLimit":60,"now":9,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":60,"now":25,"status":"active"},{"upperLimit":60,"now":41,"status":"active"},{"upperLimit":120,"now":45,"status":"active"},{"upperLimit":120,"now":47,"status":"active"},{"upperLimit":60,"now":16,"status":"active"},{"upperLimit":60,"now":51,"status":"almost"},{"upperLimit":60,"now":2,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":11,"status":"active"},{"upperLimit":60,"now":53,"status":"almost"},{"upperLimit":120,"now":43,"status":"active"},{"upperLimit":120,"now":31,"status":"active"},{"upperLimit":60,"now":4,"status":"active"},{"upperLimit":60,"now":1,"status":"active"},{"upperLimit":120,"now":110,"status":"almost"},{"upperLimit":60,"now":21,"status":"active"}]}]}]';

  //    calendarData = JSON.parse(response);
  //    resolve();
  //  }, 100);
  //}).then(() => {
    init();
  //});
})();

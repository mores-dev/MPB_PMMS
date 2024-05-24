(() => {
  //global method
  const setCalendar = (selectYear, selectMonth) => {
    const allBlockCount = 7 * 6;
    const selectYearString = selectYear.toString();
    const selectMonthString = (selectMonth - 1).toString();
    const selectMonthInfo = calendarData;
    const selectOneDay = new Date(selectYearString, selectMonthString).getDay(); //這個月的第一天是禮拜幾
    const selectAllDateCount = new Date(
      selectYearString,
      selectMonth.toString(),
      0
    ).getDate(); //取得選擇的月總共有幾天       三月的第零天 = 二月的最後一天
    const dateDashBoard = document.getElementById("dateDashBoard");
    const dateDashBoardTitle = dateDashBoard.getElementsByTagName("H5")[0];

    dateDashBoardTitle.innerText = `${selectYear +
      " / " +
      selectMonth +
      " / " +
      selectedDate}`;

    let dBefore = selectOneDay; //日曆上會顯示幾天上個月的日子
    let dateText = 1;
    let index = 0;
    while (index < allBlockCount) {
      const dateDom = allDateDom[index];
      const dateDomBtn = dateDom.getElementsByClassName("btnc")[0];
      const dateDomRemaining = dateDom.getElementsByClassName("Remaining")[0];
      const dateDomSwitch = dateDom.querySelector("[name=my-checkbox]");

      // const dateDomInfo = dateDom.getElementsByClassName("info")[0];
      index++;
      if (dBefore > 0) {
        dateDom.classList.add("noDate");
        dBefore--;
      } else if (dateText <= selectAllDateCount) {
        let date = dateText; //形成閉包
        dateDom.classList.remove("noDate");
        dateDomBtn.value = date;
        dateDomBtn.onclick = () => {
          selectedDate = date;
          setDateInfo(selectedDate);
        };
        dateDomRemaining.innerText =
          /* */ selectMonthInfo.dateData[date - 1].now;

        dateDomSwitch.onchange = x => {
          if (!calendarData.dateData[date - 1].isOpen === x.target.checked) {
            if (modifyData[date] === undefined) modifyData[date] = {};
            modifyData[date].isOpen = x.target.checked;
          } else {
            try {
              delete modifyData[date].isOpen;
            } catch {}
          }
        };

        if (
          (selectMonthInfo.dateData[date - 1].isOpen &&
            !dateDomSwitch.checked) ||
          (!selectMonthInfo.dateData[date - 1].isOpen && dateDomSwitch.checked)
        )
          dateDomSwitch.click();
        dateText++;
      } else {
        dateDom.classList.add("noDate");
      }
    }
    calendarMonthText.innerText = `${selectedYear} / ${selectedMonth}`;
  };
  const createNewInput = (id, displayText, checked = false) => {
    const dom = (document.createElement("tr").innerHTML = `
    <td>${displayText}</td>
    <td>
      <div class="onoffswitch">
        <input
          data-on-text="開"
          data-off-text="休"
          type="checkbox"
          name="my-checkbox"
          id='${id}'
          ${checked ? "checked" : ""}
        />
      </div>
    </td>`);
    return dom;
  };
  window.setDateInfo = selectDate => {
    const selectedDateInfo = calendarData.dateData[selectDate - 1];
    const dateDashBoard = document.getElementById("dateDashBoard");
    const dateDashBoardInputBlock = dateDashBoard.querySelector(
      ".panel-body tbody"
    );
    const dateDashBoardTitle = dateDashBoard.getElementsByTagName("H5")[0];
    dateDashBoardTitle.innerText = `${selectedYear +
      " / " +
      selectedMonth +
      " / " +
      selectDate}`;

    dateDashBoardInputBlock.innerHTML = "";

    //狀態是否有修改的紀錄
    selectedDateInfo.areaStatus.forEach(area => {
      dateDashBoardInputBlock.innerHTML += createNewInput(
        area.areaId,
        area.areaName,
        modifyData.hasOwnProperty(selectDate) &&
          modifyData[selectDate].hasOwnProperty(area.areaId)
          ? modifyData[selectDate][area.areaId].status
          : area.status
      );
    });
    $(".panel-body tbody [name='my-checkbox']").bootstrapSwitch();
    // $.fn.bootstrapSwitch.defaults.size = "small";
    // $.fn.bootstrapSwitch.defaults.onColor = "success";

    const [
      successInput,
      upperLimitInput,
      ...areaInput
    ] = dateDashBoard.getElementsByTagName("input");

    //success是否有修改的紀錄
    successInput.value =
      modifyData.hasOwnProperty(selectDate) &&
      modifyData[selectDate].hasOwnProperty("success")
        ? modifyData[selectDate].success
        : selectedDateInfo.success;

    //上限是開放區域的加總
    upperLimitInput.value = selectedDateInfo.areaStatus.reduce(
      (total, area) => {
        return (modifyData.hasOwnProperty(selectDate) &&
        modifyData[selectDate].hasOwnProperty(area.areaId)
        ? modifyData[selectDate][area.areaId].status
        : area.status)
          ? total + area.qty
          : total;
      },
      0
    );

    successInput.onchange = x => {
      if (selectedDateInfo.success !== Number(x.target.value)) {
        if (modifyData[selectDate] === undefined) modifyData[selectDate] = {};
        modifyData[selectDate].success = x.target.value;
      } else {
        try {
          delete modifyData[selectDate].success;
        } catch {}
      }
    };

    areaInput.forEach((input, i) => {
      input.onchange = x => {
        //如果沒有此id的修改記錄就加上去 反之刪除
        if (selectedDateInfo.areaStatus[i].status !== x.target.checked) {
          if (modifyData[selectDate] === undefined) modifyData[selectDate] = {};
          if (modifyData[selectDate][input.id] === undefined)
            modifyData[selectDate][input.id] = {};
          modifyData[selectDate][input.id].status = x.target.checked;
        } else {
          try {
            delete modifyData[selectDate][input.id];
          } catch {}
        }

        //修改upperLimit
        upperLimitInput.value = selectedDateInfo.areaStatus.reduce(
          (total, area) => {
            return (modifyData.hasOwnProperty(selectDate) &&
            modifyData[selectDate].hasOwnProperty(area.areaId)
            ? modifyData[selectDate][area.areaId].status
            : area.status)
              ? total + area.qty
              : total;
          },
          0
        );
      };

      // if (
      //   (selectedDateInfo.areaStatus[i].status &&
      //     !bigOceanWindowSwitch.checked) ||
      //   (!selectedDateInfo.areaStatus[i].status && bigOceanWindowSwitch.checked)
      // )
      //   bigOceanWindowSwitch.click();
    });
  };

  const getParameterByName = (name, url) => {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
      results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return "";
    return decodeURIComponent(results[2].replace(/\+/g, " "));
  };

  const yearParam = Number(getParameterByName("year"));
  const monthParam = Number(getParameterByName("month"));
  const dateParam = Number(getParameterByName("date"));
  let selectedYear = yearParam;
  let selectedMonth = monthParam;
  let selectedDate = !!dateParam ? dateParam : 1;
  const allDateDom = document.querySelectorAll(".calendar .dateBlock");
  const calendarMonthText = document.querySelector(".month ul li span");
  let modifyData = {};

  const init = () => {
    const saveBtn = $("#saveBtn");
    saveBtn.click(e => {
      e.preventDefault();
      console.log(JSON.stringify(modifyData));
    });
    setCalendar(selectedYear, selectedMonth);
    setDateInfo(selectedDate);
  };

  let calendarData = [];

  new Promise(resolve => {
    setTimeout(() => {
      let response = `
        {
        "month":3,
        "dateData":[
        {
        "now":11,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":18,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":60,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"full"
        },
        {
        "now":4,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":55,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":10,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":72,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":10,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":60,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"full"
        },
        {
        "now":34,
        "success":20,
        "isOpen":false,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":31,
        "success":20,
        "isOpen":false,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":21,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":8,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":9,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":1,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":25,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":41,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":45,
        "success":20,
        "isOpen":false,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":47,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":16,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":51,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"almost"
        },
        {
        "now":2,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":4,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":11,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":53,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"almost"
        },
        {
        "now":43,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":31,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":4,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":1,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        },
        {
        "now":110,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"almost"
        },
        {
        "now":21,
        "success":20,
        "isOpen":true,
        "areaStatus":[{"areaId":"1dx459i","areaName":"探險島水族館 - 大黑松小倆口","qty":20,"status":false},
        {"areaId":"4jx9ja8","areaName":"探險島水族館 - 魚的聲音","qty":20,"status":false},
        {"areaId":"9iya82k","areaName":"探險島水族館 - 大洋池大視窗","qty":60,"status":true},
        {"areaId":"jwi1ddx","areaName":"探險島水族館 - 獅子魚","qty":30,"status":true}],
        "status":"active"
        }
        ]
        }
        `;
      calendarData = JSON.parse(response);
      resolve();
    }, 100);
  }).then(() => {
    init();
  });
})();

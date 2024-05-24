using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MPB_BLL.COMMON;
//using MPB_BLL.Oper;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
//using MPB_Entities.Oper;
using MPB_PMMS.Helper;

namespace MPB_PMMS.Areas.Oper.Controllers
{
    public class OperManualController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("OperManual_Query");
        }

        [HttpGet]
        public ActionResult OperManual_Query()
        {
            //OperManual_QueryCondition qc = new OperManual_QueryCondition();
            //qc = QueryMethod(qc);
            return View();
        }

        //[HttpPost]
        //public ActionResult OperManual_Query(OperManual_QueryCondition qc)
        //{
        //    OperManual_QueryCondition qcDecode = (OperManual_QueryCondition)htmlDecode(qc);
        //    qcDecode = QueryMethod(qcDecode);
        //    return View(qcDecode);
        //}

        //private OperManual_QueryCondition QueryMethod(OperManual_QueryCondition qc)
        //{
        //    qc = (OperManual_QueryCondition)QueryConditionSave("OperManual", qc);
        //    PageList<OperManual_QueryResult> vm;
        //    bool IsQuery = true;
        //    if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
        //    {
        //        //是否進入頁面即查詢
        //        IsQuery = true;
        //        qc.R_STATUS = "1";
        //    }
        //    if (IsQuery)
        //    {
        //        OperManual_QueryBLL bll = new OperManual_QueryBLL();
        //        vm = bll.GetPageList(qc);
        //    }
        //    else
        //    {
        //        vm = new PageList<OperManual_QueryResult>();
        //        vm.Items = new List<OperManual_QueryResult>();
        //    }
        //    this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
        //    this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

        //    CodeListBLL clbll = new CodeListBLL();
        //    //下拉選單 狀態
        //    List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01000");
        //    ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.R_STATUS, "");
        //    return qc;
        //}

    }
}
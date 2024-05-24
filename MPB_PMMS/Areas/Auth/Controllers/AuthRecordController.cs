using Newtonsoft.Json;
using MPB_BLL.Auth;
using MPB_BLL.COMMON;
using MPB_PMMS.Helper;
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Web.Security.AntiXss;
using System.Web;

namespace MPB_PMMS.Areas.Auth.Controllers
{
    public class AuthRecordController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("AuthRecord_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult AuthRecord_Query()
        {
            AuthRecord_QueryCondition qc = new AuthRecord_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 2:
        public ActionResult AuthRecord_Query(AuthRecord_QueryCondition qc)
        {
            AuthRecord_QueryCondition qcDecode = (AuthRecord_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private AuthRecord_QueryCondition QueryMethod(AuthRecord_QueryCondition qc)
        {
            qc = (AuthRecord_QueryCondition)QueryConditionSave("AuthRecord", qc);
            PageList<AuthRecord_QueryResult> vm;
            Boolean IsQuery = true;
            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
                //預設條件
                qc.LogOn = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
                qc.LogOff = DateTime.Now.ToString("yyyy/MM/dd");
            }
            if (IsQuery)
            {
                AuthRecord_QueryBLL bll = new AuthRecord_QueryBLL();
                //Fix 2023/10/04 CheckMarx原碼檢測: SQL Injection\路徑 3:
                AuthRecord_QueryCondition newQc = new AuthRecord_QueryCondition();
                ModelBLL.Convert(qc, ref newQc);
                vm = bll.GetPageList(newQc);
            }
            else
            {
                vm = new PageList<AuthRecord_QueryResult>();
                vm.Items = new List<AuthRecord_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += 
                HttpUtility.HtmlDecode(AntiXssEncoder.HtmlEncode(
                "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n"
                , false));

            return qc;
        }
    }
}
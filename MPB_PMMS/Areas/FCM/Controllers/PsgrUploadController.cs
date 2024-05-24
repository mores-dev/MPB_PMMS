using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MPB_BLL.COMMON;
using MPB_BLL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;
using MPB_PMMS.Helper;

namespace MPB_PMMS.Areas.FCM.Controllers
{
    public class PsgrUploadController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("PsgrUpload_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult PsgrUpload_Query()
        {
            PsgrUpload_QueryCondition qc = new PsgrUpload_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 24:
        public ActionResult PsgrUpload_Query(PsgrUpload_QueryCondition qc)
        {
            PsgrUpload_QueryCondition qcDecode = (PsgrUpload_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private PsgrUpload_QueryCondition QueryMethod(PsgrUpload_QueryCondition qc)
        {
            qc = (PsgrUpload_QueryCondition)QueryConditionSave("PsgrUpload", qc);
            PageList<PsgrUpload_QueryResult> vm;
            bool IsQuery = true;

            //航商功能需抓登入帳號的航商ID
            if (string.IsNullOrEmpty(qc.C_ID)) qc.C_ID = User.C_ID;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
            }
            if (IsQuery)
            {
                PsgrUpload_QueryBLL bll = new PsgrUpload_QueryBLL();
                if (!string.IsNullOrWhiteSpace(qc.Upload) && qc.Upload == "1")
                {
                    qc.Upload = "";
                    bll.UploadData(User.C_ID);
                }
                //Fix 2023/10/04 CheckMarx原碼檢測: SQL Injection\路徑 2:
                PsgrUpload_QueryCondition newQc = new PsgrUpload_QueryCondition();
                ModelBLL.Convert(qc, ref newQc);
                vm = bll.GetPageList(newQc);
            }
            else
            {
                vm = new PageList<PsgrUpload_QueryResult>();
                vm.Items = new List<PsgrUpload_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            return qc;
        }

    }
}
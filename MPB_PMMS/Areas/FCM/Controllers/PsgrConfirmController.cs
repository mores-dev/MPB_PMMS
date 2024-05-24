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
    public class PsgrConfirmController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("PsgrConfirm_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult PsgrConfirm_Query()
        {
            PsgrConfirm_QueryCondition qc = new PsgrConfirm_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 18:
        public ActionResult PsgrConfirm_Query(PsgrConfirm_QueryCondition qc)
        {
            PsgrConfirm_QueryCondition qcDecode = (PsgrConfirm_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private PsgrConfirm_QueryCondition QueryMethod(PsgrConfirm_QueryCondition qc)
        {
            qc = (PsgrConfirm_QueryCondition)QueryConditionSave("PsgrConfirm", qc);
            PageList<PsgrConfirm_QueryResult> vm;
            bool IsQuery = true;

            //航商功能需抓登入帳號的航商ID
            if (string.IsNullOrEmpty(qc.C_ID)) qc.C_ID = User.C_ID;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = false;
            }
            if (IsQuery)
            {
                PsgrConfirm_QueryBLL bll = new PsgrConfirm_QueryBLL();
                //Fix 2023/10/04 CheckMarx原碼檢測: SQL Injection\路徑 1:
                PsgrConfirm_QueryCondition newQc = new PsgrConfirm_QueryCondition();
                ModelBLL.Convert(qc, ref newQc);
                vm = bll.GetPageList(newQc);
            }
            else
            {
                vm = new PageList<PsgrConfirm_QueryResult>();
                vm.Items = new List<PsgrConfirm_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            PsgrConfirm_QueryBLL querybll = new PsgrConfirm_QueryBLL();
            //下拉選單 航商
            List<CodeName> lsQueryStr = querybll.GetQueryStr(qc);

            if (string.IsNullOrWhiteSpace(qc.QueryStr) && lsQueryStr.Count > 0)
                qc.QueryStr = lsQueryStr[0].Code;

            ViewBag.QueryStrHtml = ComPage.GetDropdownList(lsQueryStr, qc.QueryStr, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PsgrConfirm_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("PsgrConfirm_Query");
            }

            PsgrConfirm_EditMain em = new PsgrConfirm_EditMain();

            PsgrConfirm_EditBLL bll = new PsgrConfirm_EditBLL();
            if (this.ViewBag.mode.Equals("A"))
            {

            }
            else
            {
                List<PsgrConfirm_EditMain> lsEM = JsonConvert.DeserializeObject<List<PsgrConfirm_EditMain>>(mod_key);
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("PsgrConfirm_Query");
                }
                List<PsgrConfirm_EditDetailGrid1> lsEDGrid1 = bll.GetDataDetailGrid1(em);
                this.ViewBag.JsScript += "var HtDataGrid1 = " + JsonConvert.SerializeObject(lsEDGrid1, Formatting.Indented) + ";\n";
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 證件別
            List<CodeName> lsIdType = clbll.GetCodeList("000", "SYS01005");
            if (string.IsNullOrWhiteSpace(em.IdType))
                em.IdType = lsIdType[0].Code;
            ViewBag.IdTypeHtml = ComPage.GetDropdownList(lsIdType, em.IdType, "");

            return View(em);
        }

        [HttpPost]
        public JsonResult AjaxSaveData(PsgrConfirm_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            PsgrConfirm_EditBLL bll = new PsgrConfirm_EditBLL();

            //1. Check Correctness of Data 
            string msg = bll.CheckData(qc, true);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                dic.Add("ErrMsg", msg);
                dic.Add("ToTalRow", "0");
            }
            else
            {
                //2. Save Data
                qc.CreateId = User.Id;
                msg = bll.InsertData(qc);
                if (string.IsNullOrWhiteSpace(msg))
                {
                    dic.Add("TotalRow", "1");
                    dic.Add("IdNo", qc.IdNo);
                    dic.Add("IdNoEncode", qc.IdNoEncode);
                }
                else
                {
                    dic.Add("ErrMsg", msg);
                    dic.Add("TotalRow", "0");
                }
            }
            return Json(dic);
        }

        [HttpPost]
        public JsonResult AjaxDeleteData(PsgrConfirm_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            PsgrConfirm_EditBLL bll = new PsgrConfirm_EditBLL();

            //1. Check Correctness of Data 
            string msg = bll.CheckData(qc, false);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                dic.Add("ErrMsg", msg);
                dic.Add("ToTalRow", "0");
            }
            else
            {
                //2. Delete Data
                qc.CreateId = User.Id;
                msg = bll.DeleteData(qc);
                if (string.IsNullOrWhiteSpace(msg))
                    dic.Add("TotalRow", "1");
                else
                {
                    dic.Add("ErrMsg", msg);
                    dic.Add("TotalRow", "0");
                }
            }
            return Json(dic);
        }

        ////存檔 處理控制
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult PsgrConfirm_Save(PsgrConfirm_SaveMain sm)
        //{
        //    //手動新增功能，讓刪除也可抓取到ModifyId
        //    string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
        //    ProcessResult pr = new ProcessResult();

        //    AddUserLog("F", mode, sm);
        //    PsgrConfirm_SaveBLL bll = new PsgrConfirm_SaveBLL();
        //    if (mode.Equals("A"))
        //    {
        //        sm.CreateId = User.Id;
        //        bll.AddData(ref pr, sm);
        //    }
        //    else if (mode.Equals("M"))
        //    {
        //        sm.ModifyId = User.Id;
        //        bll.UpdateData(ref pr, sm);
        //    }
        //    else if (mode.Equals("D"))
        //    {
        //        sm.ModifyId = User.Id;
        //        bll.DeleteData(ref pr, sm);
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    //            return View(pr);
        //    pr.ReturnModule = "FCM";
        //    pr.ReturnPage = "PsgrConfirm_Query";
        //    this.TempData["ProcessResult"] = pr;
        //    return RedirectToAction("ShowMessage", "Home", new { area = "" });
        //}

    }
}
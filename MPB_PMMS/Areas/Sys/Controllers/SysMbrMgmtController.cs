using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MPB_BLL.COMMON;
using MPB_BLL.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Sys;
using MPB_PMMS.Helper;

namespace MPB_PMMS.Areas.Sys.Controllers
{
    public class SysMbrMgmtController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("SysMbrMgmt_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult SysMbrMgmt_Query()
        {
            SysMbrMgmt_QueryCondition qc = new SysMbrMgmt_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 39:
        public ActionResult SysMbrMgmt_Query(SysMbrMgmt_QueryCondition qc)
        {
            SysMbrMgmt_QueryCondition qcDecode = (SysMbrMgmt_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private SysMbrMgmt_QueryCondition QueryMethod(SysMbrMgmt_QueryCondition qc)
        {
            qc = (SysMbrMgmt_QueryCondition)QueryConditionSave("SysMbrMgmt", qc);
            PageList<SysMbrMgmt_QueryResult> vm;
            bool IsQuery = true;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
            }
            if (IsQuery)
            {
                SysMbrMgmt_QueryBLL bll = new SysMbrMgmt_QueryBLL();
                if (!string.IsNullOrWhiteSpace(User.C_ID))
                    qc.C_ID = User.C_ID;
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<SysMbrMgmt_QueryResult>();
                vm.Items = new List<SysMbrMgmt_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01001");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.GA_STATUS, "");
            //下拉選單 類型
            List<CodeName> lsType = clbll.GetCodeList("000", "SYS01006");
            ViewBag.GaTypeHtml = ComPage.GetDropdownList(lsType, qc.GaType, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SysMbrMgmt_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("SysMbrMgmt_Query");
            }

            SysMbrMgmt_EditMain em = new SysMbrMgmt_EditMain();

            SysMbrMgmt_EditBLL bll = new SysMbrMgmt_EditBLL();
            if (this.ViewBag.mode.Equals("A"))
            {
                //Default value -- Main (新增)
                em.GaType = "H";
                em.PhoneType = "M";
                em.GaStatus = "Y";
            }
            else
            {
                List<SysMbrMgmt_EditMain> lsEM = JsonConvert.DeserializeObject<List<SysMbrMgmt_EditMain>>(mod_key);
                if (lsEM?.Count > 0) lsEM[0].C_ID = User.C_ID;

                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("SysMbrMgmt_Query");
                }
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01001");
            ViewBag.GaStatusHtml = ComPage.GetDropdownList(lsStatus, em.GaStatus, "");
            //下拉選單 類型
            List<CodeName> lsType = clbll.GetCodeList("000", "SYS01006");
            ViewBag.GaTypeHtml = ComPage.GetDropdownList(lsType, em.GaType, "");
            //下拉選單 電話類型
            List<CodeName> lsPhoneType = clbll.GetCodeList("000", "SYS01007");
            ViewBag.PhoneTypeHtml = ComPage.GetDropdownList(lsPhoneType, em.PhoneType, "");

            return View(em);
        
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SysMbrMgmt_Save(SysMbrMgmt_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            ProcessResult pr = new ProcessResult();

            AddUserLog("F", mode, sm);
            SysMbrMgmt_SaveBLL bll = new SysMbrMgmt_SaveBLL();
            if (mode.Equals("A"))
            {
                if (!string.IsNullOrWhiteSpace(User.C_ID))
                    sm.C_ID = User.C_ID;
                sm.CreateId = User.Id;
                bll.InsertData(ref pr, sm);
            }
            else if (mode.Equals("M"))
            {
                sm.ModifyId = User.Id;
                bll.UpdateData(ref pr, sm);
            }
            else
            {
                return null;
            }

            //            return View(pr);
            pr.ReturnModule = "Sys";
            pr.ReturnPage = "SysMbrMgmt_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }

        [HttpPost]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public JsonResult AjaxCheckKey(SysMbrMgmt_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (qc.GaAAA != "")
            {
                SysMbrMgmt_EditBLL bll = new SysMbrMgmt_EditBLL();
                List<AjaxKeyCountResult> lsAR = bll.Check_Key(qc);
                dic.Add("Data", JsonConvert.SerializeObject(lsAR, Formatting.Indented));
                dic.Add("ToTalRow", lsAR.Count.ToString());
            }
            else
            {
                dic.Add("ToTalRow", "0");
            }
            return Json(dic);
        }

        [HttpPost]
        public JsonResult AjaxCheckValid(SysMbrMgmt_SaveMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (qc.ValidType != "")
            {
                SysMbrMgmt_SaveBLL bll = new SysMbrMgmt_SaveBLL();
                List<AjaxKeyCountResult> lsAR = bll.Check_InputValid(qc);
                dic.Add("Data", JsonConvert.SerializeObject(lsAR, Formatting.Indented));
                dic.Add("ToTalRow", lsAR.Count.ToString());
            }
            else
            {
                dic.Add("ToTalRow", "0");
            }
            return Json(dic);
        }

    }
}
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
    public class FCMVesselController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("FCMVessel_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult FCMVessel_Query()
        {
            FCMVessel_QueryCondition qc = new FCMVessel_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 13:
        public ActionResult FCMVessel_Query(FCMVessel_QueryCondition qc)
        {
            FCMVessel_QueryCondition qcDecode = (FCMVessel_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private FCMVessel_QueryCondition QueryMethod(FCMVessel_QueryCondition qc)
        {
            qc = (FCMVessel_QueryCondition)QueryConditionSave("FCMVessel", qc);
            PageList<FCMVessel_QueryResult> vm;
            bool IsQuery = true;

            //航商功能需抓登入帳號的航商ID
            if (string.IsNullOrEmpty(qc.C_ID)) qc.C_ID = User.C_ID;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
                qc.V_STATUS = "1";
            }
            if (IsQuery)
            {
                FCMVessel_QueryBLL bll = new FCMVessel_QueryBLL();
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<FCMVessel_QueryResult>();
                vm.Items = new List<FCMVessel_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01000");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.V_STATUS, "");
            //下拉選單 航商
            List<CodeName> lsCompany = clbll.GetCodeList_Company();
            ViewBag.CompanyHtml = ComPage.GetDropdownList(lsCompany, qc.C_ID, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FCMVessel_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("FCMVessel_Query");
            }

            FCMVessel_EditMain em = new FCMVessel_EditMain();

            FCMVessel_EditBLL bll = new FCMVessel_EditBLL();
            if (this.ViewBag.mode.Equals("A"))
            {
                //Default value -- Main (新增)
                em.C_ID = User.C_ID;
                em.C_NAME = User.C_NAME;
                em.V_STATUS = "1";
                em.VesselType = "60";
            }
            else
            {
                List<FCMVessel_EditMain> lsEM = JsonConvert.DeserializeObject<List<FCMVessel_EditMain>>(mod_key);
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("FCMVessel_Query");
                }
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01000");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, em.V_STATUS, "");
            //下拉選單 航商
            List<CodeName> lsCompany = clbll.GetCodeList_Company();
            ViewBag.CompanyHtml = ComPage.GetDropdownList(lsCompany, em.C_ID, "");
            //下拉選單 船隻種類
            List<CodeName> lsVesselType = clbll.GetCodeList("000", "FCM01001");
            ViewBag.VesselTypeHtml = ComPage.GetDropdownList(lsVesselType, em.VesselType, "");

            return View(em);
        
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FCMVessel_Save(FCMVessel_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            ProcessResult pr = new ProcessResult();

            AddUserLog("F", mode, sm);
            FCMVessel_SaveBLL bll = new FCMVessel_SaveBLL();
            if (mode.Equals("A"))
            {
                sm.CreateId = User.Id;
                bll.AddData(ref pr, sm);
            }
            else if (mode.Equals("M"))
            {
                sm.ModifyId = User.Id;
                bll.UpdateData(ref pr, sm);
            }
            else if (mode.Equals("D"))
            {
                sm.ModifyId = User.Id;
                bll.DeleteData(ref pr, sm);
            }
            else
            {
                return null;
            }

            //            return View(pr);
            pr.ReturnModule = "FCM";
            pr.ReturnPage = "FCMVessel_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }

        [HttpPost]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public JsonResult AjaxCheckKey(FCMVessel_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (qc.C_ID != "" && qc.V_CODE != "")
            {
                FCMVessel_EditBLL bll = new FCMVessel_EditBLL();
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
    }
}
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
    public class SysConsRecController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("SysConsRec_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult SysConsRec_Query()
        {
            SysConsRec_QueryCondition qc = new SysConsRec_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 37:
        public ActionResult SysConsRec_Query(SysConsRec_QueryCondition qc)
        {
            SysConsRec_QueryCondition qcDecode = (SysConsRec_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private SysConsRec_QueryCondition QueryMethod(SysConsRec_QueryCondition qc)
        {
            qc = (SysConsRec_QueryCondition)QueryConditionSave("SysConsRec", qc);
            PageList<SysConsRec_QueryResult> vm;
            bool IsQuery = true;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
                qc.CR_DATE_START = DateTime.Now.AddMonths(-3).ToString("yyyy/MM/01");
            }
            if (IsQuery)
            {
                SysConsRec_QueryBLL bll = new SysConsRec_QueryBLL();
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<SysConsRec_QueryResult>();
                vm.Items = new List<SysConsRec_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01004");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.CR_STATUS, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SysConsRec_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("SysConsRec_Query");
            }

            SysConsRec_EditMain em = new SysConsRec_EditMain();

            SysConsRec_EditBLL bll = new SysConsRec_EditBLL();
            if (this.ViewBag.mode.Equals("A"))
            {
                //Default value -- Main (新增)
            }
            else
            {
                List<SysConsRec_EditMain> lsEM = JsonConvert.DeserializeObject<List<SysConsRec_EditMain>>(mod_key);
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("SysConsRec_Query");
                }
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01004");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, em.CR_STATUS, "");

            return View(em);
        
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SysConsRec_Save(SysConsRec_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            ProcessResult pr = new ProcessResult();

            AddUserLog("F", mode, sm);
            SysConsRec_SaveBLL bll = new SysConsRec_SaveBLL();
            if (mode.Equals("M"))
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
            pr.ReturnPage = "SysConsRec_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }
    }
}
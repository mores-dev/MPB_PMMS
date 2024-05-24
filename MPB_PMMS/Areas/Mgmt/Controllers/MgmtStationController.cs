using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MPB_BLL.COMMON;
using MPB_BLL.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Mgmt;
using MPB_PMMS.Helper;

namespace MPB_PMMS.Areas.Mgmt.Controllers
{
    public class MgmtStationController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("MgmtStation_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult MgmtStation_Query()
        {
            MgmtStation_QueryCondition qc = new MgmtStation_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 32:
        public ActionResult MgmtStation_Query(MgmtStation_QueryCondition qc)
        {
            MgmtStation_QueryCondition qcDecode = (MgmtStation_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private MgmtStation_QueryCondition QueryMethod(MgmtStation_QueryCondition qc)
        {
            qc = (MgmtStation_QueryCondition)QueryConditionSave("MgmtStation", qc);
            PageList<MgmtStation_QueryResult> vm;
            bool IsQuery = true;
            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
                qc.ST_STATUS = "1";
            }
            if (IsQuery)
            {
                MgmtStation_QueryBLL bll = new MgmtStation_QueryBLL();
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<MgmtStation_QueryResult>();
                vm.Items = new List<MgmtStation_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01000");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.ST_STATUS, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MgmtStation_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("MgmtStation_Query");
            }

            MgmtStation_EditMain em = new MgmtStation_EditMain();
            if (this.ViewBag.mode.Equals("A"))
            {
                //Default value -- Main (新增)
                em.ST_STATUS = "1";
            }
            else
            {
                List<MgmtStation_EditMain> lsEM = JsonConvert.DeserializeObject<List<MgmtStation_EditMain>>(mod_key);
                MgmtStation_EditBLL bll = new MgmtStation_EditBLL();
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("MgmtStation_Query");
                }
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01000");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, em.ST_STATUS, "");

            return View(em);
        
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MgmtStation_Save(MgmtStation_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            ProcessResult pr = new ProcessResult();

            AddUserLog("F", mode, sm);
            MgmtStation_SaveBLL bll = new MgmtStation_SaveBLL();
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
            pr.ReturnModule = "Mgmt";
            pr.ReturnPage = "MgmtStation_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }

        [HttpPost]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public JsonResult AjaxCheckKey(MgmtStation_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (qc.ST_CODE != "")
            {
                MgmtStation_EditBLL bll = new MgmtStation_EditBLL();
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
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
    public class MgmtRouteController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("MgmtRoute_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult MgmtRoute_Query()
        {
            MgmtRoute_QueryCondition qc = new MgmtRoute_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 29:
        public ActionResult MgmtRoute_Query(MgmtRoute_QueryCondition qc)
        {
            MgmtRoute_QueryCondition qcDecode = (MgmtRoute_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private MgmtRoute_QueryCondition QueryMethod(MgmtRoute_QueryCondition qc)
        {
            qc = (MgmtRoute_QueryCondition)QueryConditionSave("MgmtRoute", qc);
            PageList<MgmtRoute_QueryResult> vm;
            bool IsQuery = true;
            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
                qc.R_STATUS = "1";
            }
            if (IsQuery)
            {
                MgmtRoute_QueryBLL bll = new MgmtRoute_QueryBLL();
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<MgmtRoute_QueryResult>();
                vm.Items = new List<MgmtRoute_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01000");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.R_STATUS, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MgmtRoute_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("MgmtRoute_Query");
            }

            MgmtRoute_EditMain em = new MgmtRoute_EditMain();

            if (this.ViewBag.mode.Equals("A"))
            {
                //Default value -- Main (新增)
                em.R_STATUS = "1";
                this.ViewBag.JsScript = "var HtDataGrid1=[];\r\n";
            }
            else
            {
                List<MgmtRoute_EditMain> lsEM = JsonConvert.DeserializeObject<List<MgmtRoute_EditMain>>(mod_key);
                MgmtRoute_EditBLL bll = new MgmtRoute_EditBLL();
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("MgmtRoute_Query");
                }
                List<MgmtRoute_EditDetailGrid> lsEDGrid1 = bll.GetDataDetailGrid(em);
                this.ViewBag.JsScript += "var HtDataGrid1=" + JsonConvert.SerializeObject(lsEDGrid1, Formatting.Indented) + ";\n";
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01000");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, em.R_STATUS, "");

            this.ViewBag.JsScript += "var HtSelectData = {};\n";
            //表格資料處理
            Grid1_DataApply();

            return View(em);
        }

        /// <summary>
        /// 設定表格資料
        /// </summary>
        private void Grid1_DataApply()
        {
            MgmtRoute_EditBLL bll = new MgmtRoute_EditBLL();
            //Grid(Handsontable) 下拉資料來源 選單代號
            List<CodeName> lsStation = bll.GetStation();
            this.ViewBag.JsScript += "HtSelectData['ST_ID'] = " + JsonConvert.SerializeObject(lsStation) + ";\n";
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MgmtRoute_Save(MgmtRoute_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            string resultGrid1 = "" + HttpContext.Request.Form["resultGrid1"];
            ProcessResult pr = new ProcessResult();

            List<MgmtRoute_SaveDetailGrid> sdGrid1 = JsonConvert.DeserializeObject<List<MgmtRoute_SaveDetailGrid>>(resultGrid1);

            AddUserLog("F", mode, sm);
            MgmtRoute_SaveBLL bll = new MgmtRoute_SaveBLL();
            if (mode.Equals("A"))
            {
                sm.CreateId = User.Id;
                bll.AddData(ref pr, sm, sdGrid1);
            }
            else if (mode.Equals("M"))
            {
                sm.ModifyId = User.Id;
                bll.UpdateData(ref pr, sm, sdGrid1);
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
            pr.ReturnPage = "MgmtRoute_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }

        [HttpPost]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public JsonResult AjaxCheckKey(MgmtRoute_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (qc.R_CODE != "")
            {
                MgmtRoute_EditBLL bll = new MgmtRoute_EditBLL();
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
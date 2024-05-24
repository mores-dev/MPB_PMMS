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
    public class SysBulletinController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("SysBulletin_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult SysBulletin_Query()
        {
            SysBulletin_QueryCondition qc = new SysBulletin_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 35:
        public ActionResult SysBulletin_Query(SysBulletin_QueryCondition qc)
        {
            SysBulletin_QueryCondition qcDecode = (SysBulletin_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private SysBulletin_QueryCondition QueryMethod(SysBulletin_QueryCondition qc)
        {
            qc = (SysBulletin_QueryCondition)QueryConditionSave("SysBulletin", qc);
            PageList<SysBulletin_QueryResult> vm;
            bool IsQuery = true;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
            }
            if (IsQuery)
            {
                SysBulletin_QueryBLL bll = new SysBulletin_QueryBLL();
                qc.C_ID = User.C_ID;
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<SysBulletin_QueryResult>();
                vm.Items = new List<SysBulletin_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 公告類別
            List<CodeName> lsSB_TYPE = clbll.GetCodeList_Company(false);
            ViewBag.SbTypeHtml = ComPage.GetDropdownList(lsSB_TYPE, qc.SB_TYPE, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SysBulletin_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("SysBulletin_Query");
            }

            SysBulletin_EditMain em = new SysBulletin_EditMain();

            SysBulletin_EditBLL bll = new SysBulletin_EditBLL();
            if (this.ViewBag.mode.Equals("A"))
            {
                //Default value -- Main (新增)
                em.SB_TYPE = User.C_ID;
            }
            else
            {
                List<SysBulletin_EditMain> lsEM = JsonConvert.DeserializeObject<List<SysBulletin_EditMain>>(mod_key);
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("SysBulletin_Query");
                }
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 公告類別
            List<CodeName> lsSB_TYPE = clbll.GetCodeList_Company(false);
            if (!string.IsNullOrWhiteSpace(User.C_ID))
                lsSB_TYPE.RemoveAll(x => x.Code != User.C_ID);
            ViewBag.SbTypeHtml = ComPage.GetDropdownList(lsSB_TYPE, em.SB_TYPE, "");

            return View(em);
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SysBulletin_Save(SysBulletin_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            ProcessResult pr = new ProcessResult();

            AddUserLog("F", mode, sm);
            SysBulletin_SaveBLL bll = new SysBulletin_SaveBLL();
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
            else
            {
                return null;
            }

            //            return View(pr);
            pr.ReturnModule = "Sys";
            pr.ReturnPage = "SysBulletin_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }
    }
}
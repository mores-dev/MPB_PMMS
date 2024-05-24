using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MPB_BLL.COMMON;
using MPB_BLL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;
using MPB_PMMS.Helper;

namespace MPB_PMMS.Areas.Auth.Controllers
{
    public class AuthRoleController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("AuthRole_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult AuthRole_Query()
        {
            AuthRole_QueryCondition qc = new AuthRole_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult AuthRole_Query(AuthRole_QueryCondition qc)
        {
            AuthRole_QueryCondition qcDecode = (AuthRole_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private AuthRole_QueryCondition QueryMethod(AuthRole_QueryCondition qc)
        {
            qc = (AuthRole_QueryCondition)QueryConditionSave("AuthRole", qc);
            PageList<AuthRole_QueryResult> vm;
            Boolean IsQuery = true;
            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = true;
                //預設條件
                //qc.Status = "Y";
            }
            if (IsQuery)
            {
                AuthRole_QueryBLL bll = new AuthRole_QueryBLL();
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<AuthRole_QueryResult>();
                vm.Items = new List<AuthRole_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01001");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.Status, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthRole_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("AuthRole_Query");
            }

            AuthRole_EditMain em = new AuthRole_EditMain();
            if (this.ViewBag.mode.Equals("A"))
            {
                //Default value -- Main (新增)
                this.ViewBag.JsScript = "var HtDataGrid1=[];\r\n";
            }
            else
            {
                List<AuthRole_EditMain> lsEM = JsonConvert.DeserializeObject<List<AuthRole_EditMain>>(mod_key);
                AuthRole_EditBLL bll = new AuthRole_EditBLL();
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("AuthRole_Query");
                }
                List<AuthRole_EditDetailGrid1> lsEDGrid1 = bll.GetDataDetailGrid1(em);
                this.ViewBag.JsScript += "var HtDataGrid1=" + JsonConvert.SerializeObject(lsEDGrid1, Formatting.Indented) + ";\n";
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單(COMBO BOX) CodeList是否啟用
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01001");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, em.Status, " ");
            List<CodeName> lsRoleType = clbll.GetCodeList("000", "SYS01002");
            ViewBag.RoleTypeHtml = ComPage.GetDropdownList(lsRoleType, em.RoleType, "");
            
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
            AuthRole_EditBLL bll = new AuthRole_EditBLL();
            //Grid(Handsontable) 下拉資料來源 設備類型
            List<CodeName> lsDeviceType = bll.GetDeviceType();
            lsDeviceType.RemoveAll(x => x.Code != "C");
            this.ViewBag.JsScript += "HtSelectData['DeviceTypeId'] = " + JsonConvert.SerializeObject(lsDeviceType) + ";\n";
            //Grid(Handsontable) 下拉資料來源 選單代號
            List<CatCodeName> lsMenuId = bll.GetMenuId();
            this.ViewBag.JsScript += "HtSelectData['MenuId'] = " + JsonConvert.SerializeObject(lsMenuId) + ";\n";
            //連動選單 程式代號
            List<CatCodeName> lsProgId = bll.GetProgId();
            this.ViewBag.JsScript += "HtSelectData['ProgId'] = " + JsonConvert.SerializeObject(lsProgId) + ";\n";
            //加入分類陣列 SelectCat
            this.ViewBag.JsScript += "var SelectCat = new Array();\n";
            this.ViewBag.JsScript += "SelectCat['MenuId'] = \"DeviceTypeId\";\n";
            this.ViewBag.JsScript += "SelectCat['ProgId'] = \"MenuId\";\n";

            //Grid Default Value
            AuthRole_EditDetailGrid1 SchemaGrid1 = new AuthRole_EditDetailGrid1();
            SchemaGrid1.ProgExec = "true";
            SchemaGrid1.ProgAdd = "true";
            SchemaGrid1.ProgMod = "true";
            SchemaGrid1.ProgDel = "true";
            SchemaGrid1.ProgView = "true";
            this.ViewBag.JsScript += "HtSchemaGrid1=" + JsonConvert.SerializeObject(SchemaGrid1, Formatting.Indented) + ";\n";
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthRole_Save(AuthRole_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            string resultGrid1 = "" + HttpContext.Request.Form["resultGrid1"];
            ProcessResult pr = new ProcessResult();

            List<AuthRole_SaveDetailGrid1> sdGrid1 = JsonConvert.DeserializeObject<List<AuthRole_SaveDetailGrid1>>(resultGrid1);

            AddUserLog("F", mode, sm);
            AuthRole_SaveBLL bll = new AuthRole_SaveBLL();
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
                //刪除也抓取ModifyId
                sm.ModifyId = User.Id;
                bll.DeleteData(ref pr, sm);
            }
            else
            {
                return null;
            }

            //            return View(pr);
            pr.ReturnModule = "Auth";
            pr.ReturnPage = "AuthRole_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }

        [HttpPost]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public JsonResult AjaxCheckKey(AuthRole_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (qc.RoleId != "")
            {
                AuthRole_EditBLL bll = new AuthRole_EditBLL();
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
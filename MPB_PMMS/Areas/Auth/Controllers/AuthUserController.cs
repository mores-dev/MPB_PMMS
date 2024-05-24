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
using System.Web.Script.Serialization;

namespace MPB_PMMS.Areas.Auth.Controllers
{
    public class AuthUserController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("AuthUser_Query", new { tknval = TokenValue() });
        }
        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult AuthUser_Query()
        {
            AuthUser_QueryCondition qc = new AuthUser_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 7:
        public ActionResult AuthUser_Query(AuthUser_QueryCondition qc)
        {
            AuthUser_QueryCondition qcDecode = (AuthUser_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private AuthUser_QueryCondition QueryMethod(AuthUser_QueryCondition qc)
        {
            qc = (AuthUser_QueryCondition)QueryConditionSave("AuthUser", qc);
            PageList<AuthUser_QueryResult> vm;
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
                AuthUser_QueryBLL bll = new AuthUser_QueryBLL();
                vm = bll.GetPageList(qc);
            }
            else
            {
                vm = new PageList<AuthUser_QueryResult>();
                vm.Items = new List<AuthUser_QueryResult>();
            }
            this.ViewBag.PageList = QueryPage.CreatePageList(vm.TotalPages, vm.CurrentPage, vm.TotalItems);
            this.ViewBag.JsScript += "var HtData = " + JsonConvert.SerializeObject(vm.Items, Formatting.Indented) + ";\n";

            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 狀態
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01001");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, qc.Status, "");
            //下拉選單 使用者類型
            //List<CodeName> lsUserType = clbll.GetCodeList("000", "SYS01002");
            List<CodeName> lsUserType = clbll.GetCodeList_RoleId();
            ViewBag.UserTypeHtml = ComPage.GetDropdownList(lsUserType, qc.UserType, "");
            return qc;
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthUser_Edit(string mod_key)
        {
            //A-新增(Add) D-刪除(Delete) M-更新(Modify) V-查詢(View) C-複製(Copy)
            if (!CheckEditPara("ACDMV"))    //ViewBag.titleStr is setting in this function
            {
                TempData["AlertMessage"] = "您無此權限!";
                return RedirectToAction("AuthUser_Query");
            }

            AuthUser_EditMain em = new AuthUser_EditMain();
            if (this.ViewBag.mode.Equals("A"))
            {
                em.Pd = "";
                em.DEPT = "";
                em.UserType = "C";
                em.Status = "Y";
                //Default value -- Main (新增)
                this.ViewBag.JsScript = "var HtDataGrid1=[];\r\n";
            }
            else
            {
                var serializer = new JavaScriptSerializer();
                List<AuthUser_EditMain> lsEM = serializer.Deserialize<List<AuthUser_EditMain>>(mod_key);
                AuthUser_EditBLL bll = new AuthUser_EditBLL();
                em = bll.GetDataMain(lsEM[0]);
                if (em == null)//查無資料
                {
                    return RedirectToAction("AuthUser_Query");
                }
            }
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單(COMBO BOX) CodeList是否啟用
            List<CodeName> lsStatus = clbll.GetCodeList("000", "SYS01001");
            ViewBag.StatusHtml = ComPage.GetDropdownList(lsStatus, em.Status, "");
            //下拉選單 角色類型
            List<CodeName> lsRoleId = clbll.GetCodeList_RoleId();
            if (string.IsNullOrWhiteSpace(em.RoleId)) em.RoleId = lsRoleId[0].Code;
            ViewBag.RoleIdHtml = ComPage.GetDropdownList(lsRoleId, em.RoleId, "");
            //下拉選單 使用者類型
            List <CodeName> lsUserType = clbll.GetCodeList("000", "SYS01002");
            ViewBag.UserTypeHtml = ComPage.GetDropdownList(lsUserType, em.UserType, "");
            //下拉選單 航商
            List<CodeName> lsCompany = clbll.GetCodeList_Company();
            ViewBag.CompanyHtml = ComPage.GetDropdownList(lsCompany, em.C_ID, "");
            //下拉選單 航港局單位
            List<CodeName> lsDEPT = clbll.GetCodeList("000", "SYS01003");
            ViewBag.DEPTHtml = ComPage.GetDropdownList(lsDEPT, em.DEPT, "");

            return View(em);
        }

        //存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthUser_Save(AuthUser_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            ProcessResult pr = new ProcessResult();

            AddUserLog("F", mode, sm);
            AuthUser_SaveBLL bll = new AuthUser_SaveBLL();

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
                //刪除也抓取ModifyId
                sm.ModifyId = User.Id;
                bll.DeleteData(ref pr, sm);
            }
            else
            {
                return null;
            }

            pr.ReturnModule = "Auth";
            pr.ReturnPage = "AuthUser_Query";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }

        [HttpPost]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public JsonResult AjaxCheckKey(AuthUser_EditMain qc)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (qc.UserId != "")
            {
                AuthUser_EditBLL bll = new AuthUser_EditBLL();
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

        //變更密碼頁面 處理控制 (依 mod_key 處理)
        [HttpPost]
        public ActionResult AuthUser_ChgPwd()
        {
            AuthUser_EditMain em = new AuthUser_EditMain();
            em.UserId = User.LoginUserId;
            AuthUser_EditBLL bll = new AuthUser_EditBLL();
            em = bll.GetDataMain(em);
            if (em == null)//查無資料
            {
                TempData["AlertMessage"] = "查無使用者!";
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            return View(em);
        }

        //變更密碼存檔 處理控制
        [HttpPost]
        public ActionResult ChangePassword_Save(AuthUser_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            string mode = ("" + HttpContext.Request.Form["mode"]).ToUpper();
            ProcessResult pr = new ProcessResult();

            AuthUser_SaveBLL bll = new AuthUser_SaveBLL();
            if (mode.Equals("M"))
            {
                sm.ModifyId = User.Id;
                bll.UpdatePwd(ref pr, sm);
            }
            else
            {
                return null;
            }

            //            return View(pr);
            pr.ReturnModule = "Home";
            pr.ReturnPage = "Index";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("ShowMessage", "Home", new { area = "" });
        }
    }
}
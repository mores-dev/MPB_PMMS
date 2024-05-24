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

namespace MPB_PMMS.Areas.Auth.Controllers
{
    public class ChangePwdController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            QueryConditionClear();
            //AddUserLog();
            return RedirectToAction("ChangePwd");
        }

        //維護頁面 處理控制 (依 mod_key 處理)
        [HttpGet]
        public ActionResult ChangePwd()
        {
            ChangePwd_EditMain em = new ChangePwd_EditMain();
            em.UserId = User.LoginUserId;
            em.UserName = User.LoginUserName;

            return View(em);
        }

        //存檔 處理控制
        [HttpPost]
        public ActionResult ChangePwd_Save(ChangePwd_SaveMain sm)
        {
            //手動新增功能，讓刪除也可抓取到ModifyId
            ProcessResult pr = new ProcessResult();
            AddUserLog("F", "M", sm);
            ChangePwd_SaveBLL bll = new ChangePwd_SaveBLL();

            sm.ModifyId = User.LoginUserId;
            bll.UpdateData(ref pr, sm);
            if (pr.ReturnId == -1)
            {
                ChangePwd_EditMain em = new ChangePwd_EditMain();
                em.UserId = sm.UserId;
                //em.UserName = sm.UserName;
                //em.Pd = sm.Pd;

                this.TempData["AlertMessage"] = pr.ReturnMessage;
                return RedirectToAction("ChangePwd", em);
            }
            //            return View(pr);
            //pr.ReturnModule = "Auth";
            //pr.ReturnPage = "ChangePwd";
            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("Index", "Login", new { area = "", logout = 1 });
        }

    }
}
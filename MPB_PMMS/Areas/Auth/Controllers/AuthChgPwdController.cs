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
    public class AuthChgPwdController : BaseController
    {
        //[HttpPost]
        [HttpGet]
        public ActionResult AuthChgPwd()
        {
            AuthUser_EditMain em = new AuthUser_EditMain();
            em.UserId = User.LoginUserId;
            AuthUser_EditBLL bll = new AuthUser_EditBLL();
            em = bll.GetDataMain(em);
            if (em == null)//查無資料
            {
                TempData["AlertMessage"] = "查無使用者!";
                return RedirectToAction("Blank", "Home", new { area = "" });
            }

            return View(em);
        }

        //變更密碼存檔 處理控制
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AuthChgPwd_Save(AuthUser_SaveMain sm)
        {
            ProcessResult pr = new ProcessResult();

            AuthChgPwd_SaveBLL bll = new AuthChgPwd_SaveBLL();

            sm.ModifyId = User.Id;
            AddUserLog("F", "M", sm);
            bll.UpdatePwd(ref pr, sm);

            if (pr.ReturnId == -1)
            {
                TempData["AlertMessage"] = pr.ReturnMessage;
                return RedirectToAction("AuthChgPwd");
            }

            pr.ReturnModule = "Home";
            pr.ReturnPage = "Blank";

            this.TempData["ProcessResult"] = pr;
            return RedirectToAction("Blank", "Home", new { area = "" });
        }

        public ActionResult AuthChgPwd_Cancel1()
        {
            return RedirectToAction("Blank", "Home", new { area = "" });
        }
    }
}
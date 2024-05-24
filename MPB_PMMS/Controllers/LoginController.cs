using MPB_BLL.Auth;
using MPB_PMMS.Helper;
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Web.Routing;
using System.Threading;

namespace MPB_PMMS.Controllers
{
    public class LoginController : BaseController
    {
        //public ActionResult Index(string logout)
        //{
        //    return RedirectToAction("SysLogin", new { logout = logout });
        //}

        // GET: Login
        [HttpGet]
        public ActionResult Index(string logout)
        {
            if (!string.IsNullOrEmpty(logout))
            {
                //若為正常登出則加入使用者登出LOG，初始化logout="Y"
                if (logout == "1")
                {
                    AddLogoutLog();
                }
                //this.User = null;
                LogoutProcess();
            }
            var errmsg = TempData["Timeout"] as string;
            if (!string.IsNullOrWhiteSpace(errmsg))
            {
                ViewBag.Timeout = errmsg;//將顯示訊息放入 ViewBag 供 view 使用
            }
            HttpCookie cookies = Response.Cookies["__RequestVerificationToken"];
            foreach(string s in cookies.Values)
            {
                Console.WriteLine("Value: " + s);
            }
                    
            //return View(new AuthLogin_EditEntities() { Account = "", UserPassword = "" });
            int intTmp;
            DateTime dtTmp;
            string tmp;

            if (HttpContext.Application[Request.UserHostAddress] != null)
            {
                tmp = (string)HttpContext.Application[Request.UserHostAddress];
                if (DateTime.TryParse(tmp, out dtTmp))
                {
                    if (DateTime.Now.Subtract(dtTmp).TotalMinutes < 15)
                    {
                        //Response.Redirect("/Login/LoginError", true);
                        return RedirectToAction("LoginError", "Login", new { area = "" });
                    }
                }
            }
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AntiForgeryErrorHandler(ExceptionType = typeof(HttpAntiForgeryException), View = "Index", Controller = "Login", ErrorMessage = "Session Timeout")]

        public ActionResult Index(AuthLogin_EditEntities vm)
        {
            var cookiesRsp = Response.Cookies["__RequestVerificationToken"];
            var cookiesReq = Request.Cookies["__RequestVerificationToken"];
            if (cookiesRsp == null || string.IsNullOrEmpty(cookiesRsp.Value))//set Response
            {
                if (cookiesReq != null && !string.IsNullOrEmpty(cookiesReq.Value))
                {
                    //Fix 2023/10/04 CheckMarx原碼檢測: Reflected XSS All Clients\路徑 1:
                    string strReq = cookiesReq.Value;
                    strReq = strReq.Replace("<", "").Replace(">", "");
                    cookiesRsp = new HttpCookie("__RequestVerificationToken", strReq);
                    Response.SetCookie(cookiesRsp);
                }
            }
            //HttpCookie cookiesReq = Request.Cookies["__RequestVerificationToken"];
            //HttpCookie cookiesRsp = Response.Cookies["__RequestVerificationToken"];
            //cookiesRsp.Value = cookiesReq.Value;
            // incremental delay to prevent brute force attacks
            int intTmp;
            DateTime dtTmp;
            string tmp;

            if (HttpContext.Application[Request.UserHostAddress] == null)
            {
                HttpContext.Application[Request.UserHostAddress] = "1";
            }
            else
            {
                tmp = (string)HttpContext.Application[Request.UserHostAddress];
                if (Int32.TryParse(tmp, out intTmp))
                {
                    if (intTmp < 6)
                        HttpContext.Application[Request.UserHostAddress] = (intTmp + 1).ToString();
                    else
                        HttpContext.Application[Request.UserHostAddress] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (DateTime.TryParse(tmp, out dtTmp))
                {
                    if (DateTime.Now.Subtract(dtTmp).TotalMinutes < 15)
                    {
                        //Response.Redirect("/Login/LoginError", true);
                        return RedirectToAction("LoginError", "Login", new { area = "" });
                    }
                }
                else
                    HttpContext.Application[Request.UserHostAddress] = DateTime.Now.AddMinutes(-10).ToString("yyyy-MM-dd HH:mm:ss");
            }

            ProcessResult pr = new ProcessResult();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            AuthLogin_BLL bll = new AuthLogin_BLL();
            AuthLogin_LoginLog loginLog = bll.GetLoginCount(vm);
            if (loginLog?.ErrCnt > 3 && DateTime.Now < loginLog.LogDT.AddMinutes(15))
            {
                //return RedirectToAction("LoginError", "Login", new { area = "" });
                ModelState.AddModelError("", "登入次數過多!請待15分鐘後重新登入，或是請系統管理員重設密碼!");
                return View(vm);
            }
            var userInfo = bll.GetUserInfo(ref pr, vm);
            //驗証成功
            if (userInfo != null)
            {
                if (userInfo.UserProgramInfos.Count > 0)
                {
                    User = userInfo;
                    FormsAuthentication.SetAuthCookie(userInfo.Id, false);
                    AddLoginLog();
                    bll.ResetLoginLog(vm, loginLog);

                    TimeSpan nowDt = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan chgDt = new TimeSpan(Convert.ToDateTime(User.LastChanged).Ticks);
                    TimeSpan ts = nowDt.Subtract(chgDt).Duration();
                    if (ts.Days < 92)
                        return RedirectToAction("Index", "Home", new { area = "" });
                    else
                    {
                        string msg = "密碼已超過三個月未更換，請更換密碼，\r\n";
                        msg += "1. 密碼為 8 至 16 字元英數字混和。\r\n";
                        msg += "2. 不得與前3次密碼相同。\r\n";
                        this.TempData["AlertMessage"] = msg;
                        return RedirectToAction("ChangePwd", "Changepwd", null);
                    }
                }
                else
                    pr.ReturnMessage = "該帳號無使用權限!";
            }
            else
            {
                bll.Add_LoginErrorCount(vm, loginLog);
            }

            ModelState.AddModelError("", pr.ReturnMessage);

            return View(vm);

        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            if (User != null)
            {
                LogoutProcess();
            }
            return RedirectToAction("Index", new { logout = "1" });
        }

        /// <summary>
        /// 清除所有的 session
        /// </summary>
        private void LogoutProcess()
        {
            HttpContext.Application[Request.UserHostAddress] = "0";
            FormsAuthentication.SignOut();
            
            Session.RemoveAll();

            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            cookie1.HttpOnly = true;
            cookie1.SameSite = SameSiteMode.Lax;
            Response.Cookies.Add(cookie1);

            //HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            //cookie2.Expires = DateTime.Now.AddYears(-1);
            //cookie2.HttpOnly = true;
            //cookie2.SameSite = SameSiteMode.Lax;
            //Response.Cookies.Add(cookie2);
            HttpCookie cookie2 = new HttpCookie("__RequestVerificationToken", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            cookie2.HttpOnly = true;
            cookie2.SameSite = SameSiteMode.Lax;
            Response.Cookies.Add(cookie2);
        }

        public ActionResult LoginError()
        {
            return View();
        }

    }

    public class AntiForgeryErrorHandlerAttribute : HandleErrorAttribute
    {
        //用來指定 redirect 的目標 controller
        public string Controller { get; set; }
        //用來儲存想要顯示的訊息
        public string ErrorMessage { get; set; }
        //覆寫預設發生 exception 時的動作
        public override void OnException(ExceptionContext filterContext)
        {
            //如果發生的 exception 是 HttpAntiForgeryException 就轉導至設定的 controller、action (action 在 base HandleErrorAttribute已宣告)
            
            if (filterContext.Exception is HttpAntiForgeryException)
            {
                //這個屬性要設定為 true 才能接手處理 exception 也才可以 redirect
                filterContext.ExceptionHandled = true;
                //將 errormsg 使用 TempData 暫存 (ViewData 與 ViewBag 因為生命週期的關係都無法正確傳遞)
                //filterContext.Controller.TempData.Add("Timeout", "請再次登入!");
                //指定 redirect 的 controller 偶 action
                //return RedirectToAction("Index", "Login", new { area = "", logout = 1 });
                
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "action", View },
                    { "controller", Controller},
                    });
            }
            else
                base.OnException(filterContext);// exception 不是 HttpAntiForgeryException 就照 mvc 預設流程
        }
    }
}
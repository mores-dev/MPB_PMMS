using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MPB_BLL.Home;
using MPB_BLL.COMMON;
using MPB_Entities.COMMON;
using MPB_Entities.Home;
using MPB_PMMS.Helper;
using System.Web.Security.AntiXss;

namespace MPB_PMMS.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (User == null)
            {
                return
                    RedirectToRoute(
                        new
                        {
                            controller = "Login",
                            action = "Index",
                            logout = "Y",
                        });
            }

            IndexBLL bll = new IndexBLL();
            IndexQueryResult qr = new IndexQueryResult();
            //qr.LoginUser = new UserInfo() { LoginUserId = "admin", LoginUserName = "系統管理員" };
            qr.LoginUser = User;

            LtreeBLL ltBll = new LtreeBLL();
            //List<LtreeQueryResult> vm = ltBll.GetLtree(User.Id);
            qr.LtreeMenu = ltBll.GetLtree(User.LoginUserId);

            return View(qr);
        }

        /// <summary>
        /// 空白頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Blank()
        {
            return View();
        }

        /// <summary>
        /// 顯示執行結果訊息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowMessage()
        {
            ProcessResult pr = (ProcessResult)TempData["ProcessResult"];
            if (pr == null)
            {
                return null;
            }
            else
            {
                pr.ReturnModule = AntiXssEncoder.HtmlEncode(pr.ReturnModule, false);
                pr.ReturnPage = AntiXssEncoder.HtmlEncode(pr.ReturnPage, false);
                pr.ReturnMessage = AntiXssEncoder.HtmlEncode(pr.ReturnMessage, false);
                
                return View(pr);
            }
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}
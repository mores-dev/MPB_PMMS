using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPB_PMMS.Helper
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using MPB_Entities.COMMON;
    using MPB_Entities.Home;

    using WebGrease.Css.Extensions;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 程式名稱
        /// </summary>
        private string programId;
        /// <summary>
        /// 動作
        /// </summary>
        private string programAction;

        public AuthAttribute()
        {

        }

        public AuthAttribute(string programId, string programAction = "")
        {
            this.programId = programId;
            this.programAction = programAction;
        }

        private bool IsAuthorized(AuthorizationContext filterContext)
        {
            UserInfo user = (UserInfo)HttpContext.Current.Session["UserInfo"];

            if (user == null) return false;

            if (string.IsNullOrEmpty(programId)) return true;

            var programs = user.UserProgramInfos.Where(x => x.ProgId == programId);
            if (!string.IsNullOrEmpty(programId) && string.IsNullOrEmpty(programAction))
                return programs.Any();


            if (!string.IsNullOrEmpty(programId) && !string.IsNullOrEmpty(programAction))
            {
                switch (programAction.ToLower())
                {
                    case "add":
                        return programs.Any(x => x.ProgAdd == "Y");
                    case "del":
                        return programs.Any(x => x.ProgDel == "Y");
                    case "vie":
                        return programs.Any(x => x.ProgView == "Y");
                    case "mod":
                        return programs.Any(x => x.ProgMod == "Y");
                    case "run":
                        return programs.Any(x => x.ProgExec == "Y");
                    //case "do":
                    //    return programs.Any(x => x.ProgDo == "Y");
                    //case "und":
                    //    return programs.Any(x => x.ProgUndo == "Y");
                    default:
                        return false;
                }

            }

            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool isAuthorized = IsAuthorized(filterContext); // check authorization
            base.OnAuthorization(filterContext);
            if (!isAuthorized && !(filterContext.ActionDescriptor.ActionName.Equals("Index", StringComparison.InvariantCultureIgnoreCase)
                && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals("Login", StringComparison.InvariantCultureIgnoreCase)))
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpNotFoundResult();
        }

    }
}
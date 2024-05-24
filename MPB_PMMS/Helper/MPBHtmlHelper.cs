using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPB_PMMS.Helper
{
    using System.Linq.Expressions;
    using System.Web.Mvc;
    
    using MPB_Entities.COMMON;

    /// <summary>
    /// 標題名稱
    /// </summary>
    public static class MPBHtmlHelper
    {
        public static MvcHtmlString HtmlTitleHelper(this HtmlHelper html, string programId)
        {
            UserInfo user = (UserInfo)HttpContext.Current.Session["UserInfo"];
            var programs = user.UserProgramInfos.FirstOrDefault(x => x.ProgId == programId);

            if (programs != null) return new MvcHtmlString(programs.ProgName);

            return new MvcHtmlString("");
        }
    }
}
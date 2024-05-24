using MPB_BLL.COMMON;
using System;
using System.Configuration;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MPB_PMMS.Helper
{
    /// <summary>
    /// 修正CHECKMARX AJAX傳值 CSRF問題
    /// </summary>
    public class RequsetVerificationTokenAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContextBase httpContext = filterContext.HttpContext;
            try
            {                
                if (httpContext != null)
                {
                    string requestToken;
                    if (httpContext.Request.HttpMethod == "GET")
                    {
                        string str = httpContext.Request.QueryString["tknval"];
                        EnDeCode.DecryptAES256(str, ConfigurationManager.AppSettings["EnDeCodeKey"], out requestToken);
                    }
                    else if(httpContext.Request.HttpMethod == "POST")
                    {
                         requestToken = httpContext.Request.Headers["RequestVerificationToken"];
                    }
                    else
                    {
                        throw new Exception(" this method not support : " + httpContext.Request.HttpMethod);
                    }
                    
                    var p = requestToken.Split(':');
                    AntiForgery.Validate(p[0], p[1]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
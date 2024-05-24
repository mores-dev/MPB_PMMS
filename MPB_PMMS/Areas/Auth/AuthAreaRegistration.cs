using System.Web.Mvc;

namespace MPB_PMMS.Areas.Auth
{
    public class AuthAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Auth";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Auth_default",
                "Auth/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
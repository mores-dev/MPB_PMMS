using System.Web.Mvc;

namespace MPB_PMMS.Areas.FCM
{
    public class FCMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FCM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FCM_default",
                "FCM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
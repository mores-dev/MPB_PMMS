using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security.AntiXss;

namespace MPB_PMMS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiXssEncoder.MarkAsSafe(LowerCodeCharts.Default, LowerMidCodeCharts.None, MidCodeCharts.None
                , UpperMidCodeCharts.CjkRadicalsSupplement | UpperMidCodeCharts.KangxiRadicals | UpperMidCodeCharts.Bopomofo | UpperMidCodeCharts.CjkUnifiedIdeographsExtensionA
                | UpperMidCodeCharts.CjkUnifiedIdeographs | UpperMidCodeCharts.LatinExtendedD, UpperCodeCharts.CjkCompatibilityIdeographs);


        }
    }
}

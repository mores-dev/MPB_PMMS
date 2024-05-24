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

namespace MPB_PMMS.Controllers
{
    public class ErrorController : BaseController
    {
        [HttpGet]
        public ActionResult Blank()
        {
            return View();
        }
    }
}
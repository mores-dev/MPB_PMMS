using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MPB_BLL.COMMON;
using MPB_Entities.COMMON;
using MPB_PMMS.Helper;
using Microsoft.Reporting.WebForms;
using MPB_BLL.FCM;
using MPB_Entities.FCM;
using System.Linq;

namespace MPB_PMMS.Areas.FCM.Controllers
{
    public class NCCRStatisticController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("NCCRStatistic_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult NCCRStatistic_Query()
        {
            NCCRStatistic_QueryCondition qc = new NCCRStatistic_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 16:
        public ActionResult NCCRStatistic_Query(NCCRStatistic_QueryCondition qc)
        {
            NCCRStatistic_QueryCondition qcDecode = (NCCRStatistic_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private NCCRStatistic_QueryCondition QueryMethod(NCCRStatistic_QueryCondition qc)
        {
            qc = (NCCRStatistic_QueryCondition)QueryConditionSave("NCCRStatistic", qc);
            bool IsQuery = true;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = false;
                qc.OdrDtEnd = DateTime.Now.ToString("yyyy/MM/dd");
            }
            ViewBag.IsQuery = IsQuery;

            if (IsQuery)
            {
                //setReport
                SetReportData(qc);
            }

            return qc;
        }

        private void SetReportData(NCCRStatistic_QueryCondition qc)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.SizeToReportContent = true;
            reportViewer.ShowPrintButton = false;
            reportViewer.ShowZoomControl = true;
            reportViewer.ZoomMode = ZoomMode.Percent;

            reportViewer.LocalReport.ReportPath = HttpContext.Server.MapPath("~/App_Data/NCCRStatistic.rdlc");

            NCCRStatistic_QueryBLL bll = new NCCRStatistic_QueryBLL();
            //set param
            DataTable dt = bll.GetPrint1List(qc).Tables[0];
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            //設定共用報表顯示參數
            setReportPubParameters(reportViewer, qc);

            reportViewer.LocalReport.Refresh();

            ViewBag.ReportViewer = reportViewer;
        }

        /// <summary>
        /// 設定報表顯示參數
        /// </summary>
        /// <param name="rpt1"></param>
        /// <param name="qc"></param>
        private void setReportPubParameters(ReportViewer rpt1, NCCRStatistic_QueryCondition qc)
        {
            List<ReportParameter> parameters1 = new List<ReportParameter>();
            //===================
            parameters1.Add(new ReportParameter("OdrDtStart", qc.OdrDtStart));
            parameters1.Add(new ReportParameter("OdrDtEnd", qc.OdrDtEnd));
            //===================
            rpt1.LocalReport.SetParameters(parameters1.ToArray());
        }
    }
}
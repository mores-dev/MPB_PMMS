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
    public class PsgrRegisterListController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("PsgrRegisterList_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult PsgrRegisterList_Query()
        {
            PsgrRegisterList_QueryCondition qc = new PsgrRegisterList_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 22:
        public ActionResult PsgrRegisterList_Query(PsgrRegisterList_QueryCondition qc)
        {
            PsgrRegisterList_QueryCondition qcDecode = (PsgrRegisterList_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private PsgrRegisterList_QueryCondition QueryMethod(PsgrRegisterList_QueryCondition qc)
        {
            qc = (PsgrRegisterList_QueryCondition)QueryConditionSave("PsgrRegisterList", qc);
            Boolean IsQuery = true;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = false;
                qc.OdrDt = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd");
            }
            ViewBag.IsQuery = IsQuery;
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 交易類別
            List<CodeName> lsOdrType = clbll.GetCodeList("", "SYS01006");
            ViewBag.OdrTypeHtml = ComPage.GetDropdownList(lsOdrType, qc.OdrType, "");

            if (IsQuery)
            {
                if (!string.IsNullOrWhiteSpace(qc.OdrType))
                    qc.OdrTypeName = lsOdrType.First(x => x.Code == qc.OdrType).Name;
                //航商功能需抓登入帳號的航商ID
                if (string.IsNullOrWhiteSpace(qc.C_ID)) qc.C_ID = User.C_ID;

                //setReport
                SetReportData(qc);
            }

            return qc;
        }

        private void SetReportData(PsgrRegisterList_QueryCondition qc)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.SizeToReportContent = true;
            reportViewer.ShowPrintButton = true;
            reportViewer.ShowZoomControl = true;
            reportViewer.ShowRefreshButton = false;
            reportViewer.ShowBackButton = false;
            reportViewer.ShowExportControls = true;
            reportViewer.ZoomMode = ZoomMode.Percent;
            
            reportViewer.LocalReport.ReportPath = HttpContext.Server.MapPath("~/App_Data/PsgrRegisterList.rdlc");

            PsgrRegisterList_QueryBLL bll = new PsgrRegisterList_QueryBLL();
            //set param
            DataTable dt = bll.GetPrint1List(qc);
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
        private void setReportPubParameters(ReportViewer rpt1, PsgrRegisterList_QueryCondition qc)
        {
            List<ReportParameter> parameters1 = new List<ReportParameter>();
            //===================
            parameters1.Add(new ReportParameter("OdrDt", qc.OdrDt));
            parameters1.Add(new ReportParameter("OdrType", string.IsNullOrWhiteSpace(qc.OdrType) ? "全部" : qc.OdrTypeName));
            //===================
            rpt1.LocalReport.SetParameters(parameters1.ToArray());
        }
    }
}
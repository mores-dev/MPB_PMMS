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
using System.Web.Script.Serialization;

namespace MPB_PMMS.Areas.FCM.Controllers
{
    public class PsgrManifestController : BaseController
    {
        public ActionResult Index()
        {
            QueryConditionClear();
            if (User == null)
                return RedirectToAction("Index", "Login", new { area = "", logout = 1 });

            AddUserLog();
            return RedirectToAction("PsgrManifest_Query", new { tknval = TokenValue() });
        }

        [HttpGet]
        [RequsetVerificationToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF
        public ActionResult PsgrManifest_Query()
        {
            PsgrManifest_QueryCondition qc = new PsgrManifest_QueryCondition();
            qc = QueryMethod(qc);
            return View(qc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//Fix 2023/10/04 CheckMarx原碼檢測: CSRF\路徑 20:
        public ActionResult PsgrManifest_Query(PsgrManifest_QueryCondition qc)
        {
            PsgrManifest_QueryCondition qcDecode = (PsgrManifest_QueryCondition)htmlDecode(qc);
            qcDecode = QueryMethod(qcDecode);
            return View(qcDecode);
        }

        private PsgrManifest_QueryCondition QueryMethod(PsgrManifest_QueryCondition qc)
        {
            qc = (PsgrManifest_QueryCondition)QueryConditionSave("PsgrManifest", qc);
            Boolean IsQuery = true;

            if (HttpContext.Session["QUERY_INPUT_MAP"] == null)
            {
                //是否進入頁面即查詢
                IsQuery = false;
                qc.ShippingDt = DateTime.Now.ToString("yyyy/MM/dd");
            }
            ViewBag.IsQuery = IsQuery;
            PsgrManifest_ParamName pn = SetQueryParam(qc);
            if (IsQuery)
            {
                //setReport
                SetReportData(qc, pn);
            }

            return qc;
        }

        private PsgrManifest_ParamName SetQueryParam(PsgrManifest_QueryCondition qc)
        {
            CodeListBLL clbll = new CodeListBLL();
            //下拉選單 航商
            List<CodeName> lsC_ID = clbll.GetCodeList_Company(true);
            if (!string.IsNullOrWhiteSpace(User.C_ID)) lsC_ID.RemoveAll(x => x.Code != User.C_ID);
            else if (string.IsNullOrWhiteSpace(qc.C_ID)) qc.C_ID = lsC_ID[0].Code;
            ViewBag.C_IDHtml = ComPage.GetDropdownList(lsC_ID, qc.C_ID, "");
            
            //下拉選單 場站
            List<CodeName> lsStation = new List<CodeName>();
            lsStation.Add(new CodeName { Code = "D", Name = "東港" });
            lsStation.Add(new CodeName { Code = "L", Name = "小琉球" });
            if (string.IsNullOrWhiteSpace(qc.Station)) qc.Station = "D";
            ViewBag.StationHtml = ComPage.GetDropdownList(lsStation, qc.Station, "");

            //下拉選單 航班
            PsgrManifest_QueryBLL bll = new PsgrManifest_QueryBLL();
            List<CodeName> lsVoyageTime = bll.GetVoyageTime(qc);
            if (string.IsNullOrWhiteSpace(qc.VoyageTime) && lsVoyageTime.Count > 0) qc.VoyageTime = lsVoyageTime[0].Code;
            ViewBag.VoyageTimeHtml = ComPage.GetDropdownList(lsVoyageTime, qc.VoyageTime, "");

            PsgrManifest_ParamName pn = new PsgrManifest_ParamName();
            pn.ShippingDt = qc.ShippingDt;
            pn.C_ID = GetReportParameterName(lsC_ID, qc.C_ID);
            pn.Station = GetReportParameterName(lsStation, qc.Station);
            pn.VoyageTime = GetReportParameterName(lsVoyageTime, qc.VoyageTime);
            pn.UserName = User.Id + " " + User.Name;

            return pn;
        }

        private void SetReportData(PsgrManifest_QueryCondition qc, PsgrManifest_ParamName pn)
        {
            ReportViewer reportViewer = GetRepeortViewer("~/App_Data/PsgrManifest.rdlc");

            PsgrManifest_QueryBLL bll = new PsgrManifest_QueryBLL();
            //set param
            DataTable dt = bll.GetPrint1List(qc);
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));

            pn.DtlCnt = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["PASSENGER_CNT"]) : 0;
            //設定共用報表顯示參數
            setReportPubParameters(reportViewer, qc, pn);

            reportViewer.LocalReport.Refresh();

            ViewBag.ReportViewer = reportViewer;
        }

        /// <summary>
        /// 設定報表顯示參數
        /// </summary>
        /// <param name="rpt1"></param>
        /// <param name="qc"></param>
        private void setReportPubParameters(ReportViewer rpt1, PsgrManifest_QueryCondition qc, PsgrManifest_ParamName pn)
        {
            List<ReportParameter> parameters1 = new List<ReportParameter>();
            //===================
            parameters1.Add(new ReportParameter("ShippingDt", qc.ShippingDt));
            parameters1.Add(new ReportParameter("C_ID", pn.C_ID));
            parameters1.Add(new ReportParameter("Station", pn.Station));
            parameters1.Add(new ReportParameter("VoyageTime", pn.VoyageTime));
            parameters1.Add(new ReportParameter("UserName", pn.UserName));
            parameters1.Add(new ReportParameter("DtlCnt", pn.DtlCnt.ToString()));
            //===================
            rpt1.LocalReport.SetParameters(parameters1.ToArray());
        }

        [HttpPost]
        public JsonResult AjaxGetQueryVoyageTime(PsgrManifest_QueryCondition qc)
        {
            PsgrManifest_QueryCondition qcDecode = (PsgrManifest_QueryCondition)htmlDecode(qc);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            PsgrManifest_QueryBLL bll = new PsgrManifest_QueryBLL();
            Dictionary<string, object> dic1 = new Dictionary<string, object>();
            var serializer = new JavaScriptSerializer();

            dic1.Add("VoyageTime", bll.GetVoyageTime(qc));

            dic.Add("Data", serializer.Serialize(dic1));

            return Json(dic);
        }
    }
}
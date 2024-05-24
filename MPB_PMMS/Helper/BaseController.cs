using MPB_Entities.COMMON;
using MPB_BLL.COMMON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Web.Configuration;
using Microsoft.Reporting.WebForms;
using NLog;
using System.Web.Security.AntiXss;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Configuration;

namespace MPB_PMMS.Helper
{
    public class BaseController : System.Web.Mvc.Controller
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        private static string AESKey = ConfigurationManager.AppSettings["EnDeCodeKey"];

        public BaseController()
        {
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    Exception ex = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;

        //    //string controllerName = filterContext.RouteData.Values["controller"].ToString();
        //    //string actionName = filterContext.RouteData.Values["action"].ToString();
        //    string url = "/Error/Index";
        //    filterContext.Result = RedirectToAction("Index", "Error", "Error");
        //}

        protected override void HandleUnknownAction(string actionName)
        {
            Response.Redirect("/", true);
        }

        protected object htmlDecode(object qc)
        {
            Type objType = qc.GetType();
            PropertyInfo[] propInfos = objType.GetProperties();
            foreach(var prop in propInfos)
            {
                if (!prop.CanWrite) continue;
                if (prop.PropertyType.IsPrimitive)
                {
                    if (prop.PropertyType.FullName == "System.String")
                        prop.SetValue(qc, HttpUtility.HtmlDecode(AntiXssEncoder.HtmlEncode(prop.GetValue(qc).ToString(), false)));
                }
            }
            
            return qc;
        }

        /// <summary>
        /// 登入使用者
        /// </summary>
        protected new UserInfo User
        {
            get
            {
                //測試用
                //if (Session == null || Session["UserInfo"] == null)
                //{
                //    return new UserInfo()
                //    {
                //        Id = "admin",
                //        Name = "管理者",
                //        Email = "xxx@xxx.xx",
                //        Enable = "Y",
                //        SubSec = "811",
                //        SecName = "標準股",
                //        CenterNo = "400",
                //        CenterName = "台中郵件處理中心",
                //        LoginUserId = "admin",
                //        LoginUserName = "管理者",
                //        LoginUser = new UserInfo()
                //        {
                //          Id = "admin",
                //           Name = "管理者",
                //           Email = "xxx@xxx.xx",
                //          Enable = "Y",
                //          SubSec = "811",
                //          SecName = "標準股",
                //          CenterNo = "400",
                //          CenterName = "台中郵件處理中心",
                //        }
                //    };
                //}

                return (UserInfo)Session["UserInfo"];
            }
            set
            {
                Session["UserInfo"] = value;
            }
        }
        /// <summary>
        /// 網站網路路徑
        /// </summary>
        protected string HostUrl {
            get
            {
                string rtn = WebConfigurationManager.AppSettings["ImgHostUrl"].ToString();
                if (string.IsNullOrWhiteSpace(rtn))
                {
                    rtn = Request.Url.Authority.ToLower();
                    if (rtn.IndexOf("http") < 0)
                        rtn = "http://" + rtn;
                }
                return rtn;
            }
        }
        /// <summary>
        /// 圖片上傳實體路徑
        /// </summary>
        protected string ImgUplPath
        {
            get
            {
                string rtn = WebConfigurationManager.AppSettings["ImgUploadPath"].ToString();
                if (string.IsNullOrWhiteSpace(rtn))
                    rtn = Server.MapPath("~/CDPSUpload");
                return rtn;
            }
        }

        /// <summary>
        /// 將查詢頁面條件存至session
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        protected object QueryConditionSave(object qc)
        {
            if (Request.HttpMethod == "POST")
            {
                Session["QUERY_INPUT_MAP"] = qc;
                return qc;
            }
            else if (Session["QUERY_INPUT_MAP"] != null)
            {
                return Session["QUERY_INPUT_MAP"];
            }
            else
            {
                return qc;
            }
        }
        /// <summary>
        /// 將查詢頁面條件存至session
        /// </summary>
        /// <param name="ProgId">程式代碼</param>
        /// <param name="qc">查詢條件</param>
        /// <returns></returns>
        protected object QueryConditionSave(string ProgId, object qc)
        {
            Dictionary<string, object> dic = (Dictionary<string, object>)Session["QUERY_INPUT_MAP"];
            if (dic == null)
            {
                dic = new Dictionary<string, object>();
            }
            if (Request.HttpMethod == "POST")
            {
                //按查詢鈕，用畫面的值取代
                if (dic.ContainsKey(ProgId))
                {
                    dic.Remove(ProgId);
                }
                dic.Add(ProgId, qc);
                Session["QUERY_INPUT_MAP"] = dic;
                return qc;
            }
            else if (dic.ContainsKey(ProgId))
            {
                //回上一頁，用session取代
                dic.TryGetValue(ProgId, out qc);
                return qc;
            }
            else
            {
                return qc;
            }
        }
        /// <summary>
        /// 清除session中的查詢頁面條件
        /// </summary>
        protected void QueryConditionClear()
        {
            Session.Remove("QUERY_INPUT_MAP");
        }
        /// <summary>
        /// 檢查edit頁面傳入參數
        /// </summary>
        /// <param name="allowMode">A新增;D刪除;M修改;V檢視</param>
        /// <returns></returns>
        protected Boolean CheckEditParaByParam(string allowMode)
        {
            //非法的mode ADMV
            if (allowMode.IndexOf(allowMode) < 0)
            {
                return false;
            }
            return AuthorizedProcess(allowMode, string.Empty);
        }
        /// <summary>
        /// 檢查edit頁面傳入參數
        /// </summary>
        /// <param name="AllowMode">不會拿來判斷，請用CheckEditParaByParam</param>
        /// <param name="skipAuth">是否檢查權限</param>
        /// <returns></returns>
        protected Boolean CheckEditPara(string AllowMode)
        {
            string mode = HttpUtility.HtmlDecode(AntiXssEncoder.HtmlEncode(("" + Request.Form["mode"]).ToUpper(), false));
            string modKey = HttpUtility.HtmlDecode(AntiXssEncoder.HtmlEncode("" + Request.Form["mod_key"], false));
            
            if (!mode.Equals("A"))
            {
                //不是新增檢查是否有傳入KEY值

                if (modKey.Equals(""))
                {
                    return false;
                }
                List<object> mod_key_obj = JsonConvert.DeserializeObject<List<object>>(modKey);
                if (mod_key_obj.Count != 1)
                {
                    return false;
                }
            }

            return AuthorizedProcess(mode, modKey);
        }

        /// <summary>
        /// 檢查User是否有該模式的權限: A: 新增 / C: 複製 / D: 刪除 / V: 檢視 / M: 修改 / R: 執行 / O: 正向操作 / U: 負向操作
        /// </summary>
        /// <param name="AllowMode"></param>
        /// <returns></returns>
        protected Boolean CheckAuth(string AllowMode)
        {
            return IsAuthorized(AllowMode);
        }

        /// <summary>
        /// 授權流程
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private bool AuthorizedProcess(string mode, string modKey)
        {
            if (!IsAuthorized(mode))
                return false;

            SettingPageInformation(mode);
            return true;
        }

        private void SettingPageInformation(string mode)
        {
            string isEditMode = "false";
            string isViewMode = "false";
            string isDeleteMode = "false";
            string titleStr = "";
            if (mode.Equals("A") || mode.Equals("C"))
            {
                titleStr = "新增";
            }
            else if (mode.Equals("M"))
            {
                isEditMode = "true";
                titleStr = "修改";
            }
            else if (mode.Equals("V"))
            {
                isEditMode = "true";
                isViewMode = "true";
                titleStr = "檢視";
            }
            else if (mode.Equals("D"))
            {
                isEditMode = "true";
                isDeleteMode = "true";
                titleStr = "刪除";
            }
            else
            {
                titleStr = "未知";
            }


            this.ViewBag.mode = mode;
            this.ViewBag.isEditMode = isEditMode;
            this.ViewBag.isViewMode = isViewMode;
            this.ViewBag.isDeleteMode = isDeleteMode;
            this.ViewBag.titleStr = titleStr;
            this.ViewBag.currentUrl = AntiXssEncoder.HtmlEncode("" + Request.Form["currentUrl"], false);
        }

        private bool IsAuthorized(string programAction)
        {
            UserInfo user = User;
            string programId = this.ControllerContext.RouteData.Values["controller"].ToString();

            if (user == null) return false;

            if (string.IsNullOrEmpty(programId)) return true;

            var programs = user.UserProgramInfos.Where(x => x.ProgId == programId);

            if (!string.IsNullOrEmpty(programId) && !string.IsNullOrEmpty(programAction))
            {
                switch (programAction.ToUpper())
                {
                    case "A": //新增
                        return programs.Any(x => x.ProgAdd == "Y");
                    case "C": //新增
                        return programs.Any(x => x.ProgAdd == "Y");
                    case "D": //刪除
                        return programs.Any(x => x.ProgDel == "Y");
                    case "V": //檢視
                        return programs.Any(x => x.ProgView == "Y");
                    case "M": //修改
                        return programs.Any(x => x.ProgMod == "Y");
                    case "R": //執行
                        return programs.Any(x => x.ProgExec == "Y");
                    //case "O": //正向操作
                    //    return programs.Any(x => x.ProgDo == "Y");
                    //case "U": //負向操作
                    //    return programs.Any(x => x.ProgUndo == "Y");
                    default:
                        return false;
                }

            }

            return false;
        }

        /// <summary>
        /// 加入使用者操作記錄LOG
        /// </summary>
        /// <returns></returns>
        protected void AddUserLog(string logType = "F", string progFunc = "", object keyData = null, List<object> keyDatas = null)
        {
            string programId = this.ControllerContext.RouteData.Values["controller"].ToString();
            string ClientIP = "";
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                ClientIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            UserLog em = new UserLog();
            em.LogType = logType;
            em.ProgId = programId;
            em.UserId = User.Id;
            em.LoginIp = ClientIP;
            em.ProgFunc = progFunc;
            if (keyData == null) em.KeyData = "";
            else em.KeyData = GetLogRecord(keyData);
            if (keyDatas != null) GetLogRecords(keyDatas);

            UserProgLogBLL bll = new UserProgLogBLL();
            bll.AddUserProgLog(em);
        }

        /// <summary>
        /// 加入使用者登入LOG
        /// </summary>
        /// <returns></returns>
        protected void AddLoginLog()
        {
            string ClientIP = "";
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                ClientIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }


            UserLog em = new UserLog();
            em.LogType = "I";
            em.ProgId = "";
            em.UserId = User.Id;
            em.LoginIp = ClientIP;

            UserLogBLL bll = new UserLogBLL();
            bll.AddUserLogLog(em);

        }

        /// <summary>
        /// 加入使用者登出LOG
        /// </summary>
        /// <returns></returns>
        protected void AddLogoutLog()
        {
            string ClientIP = "";
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                ClientIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            //無Session資料可記錄
            if (User != null && string.IsNullOrWhiteSpace(User.Id))
            {
                UserLog em = new UserLog();
                em.LogType = "O";
                em.ProgId = "";
                em.UserId = User.Id;
                em.LoginIp = ClientIP;

                UserLogBLL bll = new UserLogBLL();
                bll.AddUserLogLog(em);
            }

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
        }

        private string GetLogRecords(List<object> entities)
        {
            StringBuilder sb = new StringBuilder();
            foreach (object o in entities)
                sb.Append(GetLogRecord(o) + "||");
            return sb.ToString();
        }
        private string GetLogRecord(object o)
        {
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] infos = o.GetType().GetProperties();
            foreach (PropertyInfo info in infos)
            {
                sb.AppendFormat("{0}:{1},", info.Name, info.GetValue(o, null));
            }
            return sb.ToString();
        }

        protected string HtmlDecode(string encode)
        {
            if (string.IsNullOrWhiteSpace(encode))
                encode = "";
            return HttpUtility.HtmlDecode(encode);
        }

        protected string GetReportParameterName(List<CodeName> lst, string code)
        {
            string rtn = "全部";
            if (lst.Count(x => x.Code == code) > 0)
                rtn = lst.Where(x => x.Code == code).FirstOrDefault().Name;
            return rtn;
        }

        protected ReportViewer GetRepeortViewer(string reportPath)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.SizeToReportContent = true;
            reportViewer.ShowPrintButton = false;
            reportViewer.ShowZoomControl = true;
            reportViewer.ZoomMode = ZoomMode.Percent;
            
            reportViewer.ShowRefreshButton = false;
            reportViewer.ShowBackButton = false;
            reportViewer.ShowExportControls = true;
            
            reportViewer.LocalReport.ReportPath = HttpContext.Server.MapPath(reportPath);

            return reportViewer;
        }

        public string TokenValue()
        {
            AntiForgery.GetTokens(null, out string cookieToken, out string formToken);
            EnAES256Code(cookieToken + ":" + formToken, out string token);
            return token;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected bool EnAES256Code(string str, out string res)
        {
            return EnDeCode.EncryptAES256(str, AESKey, out res);
        }
    }
}

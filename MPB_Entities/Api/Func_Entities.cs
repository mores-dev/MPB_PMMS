using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class Func_Header
    {
        //API功能代碼
        [DisplayName("API功能代碼")]
        public string FUNC_ID { get; set; }

        //設備上傳時間
        [DisplayName("設備上傳時間")]
        public string SYS_DATE { get; set; }

        //API簽章
        [DisplayName("API簽章")]
        public string FUNC_SIG { get; set; }

        //API參數
        [DisplayName("API參數")]
        public JObject FUNC_DATA { get; set; }
    }

    public class ApiResult<T>
    {
        //API功能代碼
        [DisplayName("API功能代碼")]
        public string FUNC_ID { get; set; }

        //設備上傳時間
        [DisplayName("設備上傳時間")]
        public string SYS_DATE { get; set; }

        //API參數
        [DisplayName("執行結果")]
        public string RSPN_CODE { get; set; }

        //API參數
        [DisplayName("執行結果說明")]
        public string RSPN_MSG { get; set; }

        //API參數
        [DisplayName("回傳值")]
        public T RSPN_DATA { get; set; }


        public ApiResult() { }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(Func_Header fh, T data, string msg = "")
        {
            FUNC_ID = fh.FUNC_ID;
            SYS_DATE = fh.SYS_DATE;
            RSPN_CODE = "1";
            RSPN_DATA = data;
            if (!string.IsNullOrWhiteSpace(msg))
                RSPN_MSG = msg;
        }

        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(Func_Header fh, T data)
        {
            FUNC_ID = fh.FUNC_ID;
            SYS_DATE = fh.SYS_DATE;
            RSPN_CODE = "1";
            RSPN_DATA = data;
        }
        /// <summary>
        /// 建立成功結果
        /// </summary>
        /// <param name="data"></param>
        public ApiResult(Func_Header fh, string errCode, string errMsg)
        {
            FUNC_ID = fh.FUNC_ID;
            SYS_DATE = fh.SYS_DATE;
            RSPN_CODE = "0";
            RSPN_MSG = errMsg;
        }
    }

    public class APIRequest
    {
        /// <summary>
        /// API功能代碼
        /// </summary>
        public string FUNC_ID { get; set; }

        /// <summary>
        /// 上傳時間
        /// </summary>
        public string SYS_DATE { get; set; }

        /// <summary>
        /// 航商代號
        /// </summary>
        public string ACNT_NO { get; set; }

        /// <summary>
        /// API認證簽章
        /// </summary>
        public string FUNC_SIG { get; set; }

        /// <summary>
        /// 參數欄位
        /// </summary>
        public JObject FUNC_DATA { get; set; }


    }

    public class APIResponse<T>
    {
        public APIResponse()
        {

        }

        public APIResponse(string funcID, string sysDate, string acntNo)
        {
            this.FUNC_ID = funcID;
            this.SYS_DATE = sysDate;
            this.ACNT_NO = acntNo;
        }
        /// <summary>
        /// API功能代碼
        /// </summary>
        public string FUNC_ID { get; set; }

        /// <summary>
        /// 上傳時間
        /// </summary>
        public string SYS_DATE { get; set; }

        /// <summary>
        /// 航商代號
        /// </summary>
        public string ACNT_NO { get; set; }

        /// <summary>
        /// 執行結果
        /// </summary>
        public string RSPN_CODE { get; set; }

        /// <summary>
        /// 執行結果說明
        /// </summary>
        public object RSPN_MSG { get; set; }

        /// <summary>
        /// 回傳內容
        /// </summary>
        public T RSPN_DATA { get; set; }

        public APIResponse<T> OK(T t)
        {
            this.RSPN_CODE = "1";
            this.RSPN_DATA = t;
            this.RSPN_MSG = "";
            return this;
        }

        public APIResponse<T> Error(string code, string msg)
        {
            this.RSPN_CODE = code;
            this.RSPN_MSG = msg;
            return this;
        }

        public APIResponse<T> Error(string msg)
        {
            return Error("0", msg);
        }
    }
}

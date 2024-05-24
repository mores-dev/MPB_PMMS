using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MPB_Entities.KIOSK
{
    /// <summary>
    /// 請求訊息
    /// </summary>
    public class RequestModel<T>
    {
        /// <summary>
        /// 機台名稱
        /// </summary>
        [Required]
        public string DEVICE_ID { get; set; }

        ///// <summary>
        ///// IP位置
        ///// </summary>
        //public string ip { get; set; }

        ///// <summary>
        ///// 驗證token
        ///// </summary>
        //public string token { get; set; }

        /// <summary>
        /// 請求時間
        /// </summary>
        [Required]
        public DateTime? sendDateTime { get; set; }

        /// <summary>
        /// 傳送資料(單筆)
        /// </summary>
        public T data { get; set; }

        ///// <summary>
        ///// 傳送資料(多筆)
        ///// </summary>
        //public List<T> datas { get; set; }

    }

    /// <summary>
    /// 回覆訊息
    /// </summary>
    public class ResponseModel<T>
    {
        /// <summary>
        /// 回覆代碼
        /// </summary>
        [Required]
        public string code { get; set; }

        /// <summary>
        /// 回覆訊息
        /// </summary>
        [Required]
        public string msg { get; set; }

        /// <summary>
        /// 回覆時間
        /// </summary>
        public DateTime sendDateTime { get; set; }

        /// <summary>
        /// 回覆資料(單筆)
        /// </summary>
        public T data { get; set; }

        ///// <summary>
        ///// 回覆資料(多筆)
        ///// </summary>
        //public List<T> datas { get; set; }

        /// <summary>
        /// 回覆正常
        /// </summary>
        /// <returns></returns>        
        public ResponseModel<T> OK()
        {
            this.code = "00";
            this.sendDateTime = DateTime.Now;
            return this;
        }


        /// <summary>
        ///  回覆正常
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public ResponseModel<T> OK(T t)
        {
            this.code = "00";
            this.data = t;
            this.sendDateTime = DateTime.Now;
            return this;
        }

        /// <summary>
        /// 回覆錯誤
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public ResponseModel<T> Error(string str)
        {
            this.code = "99";
            this.msg = str;
            this.sendDateTime = DateTime.Now;
            return this;
        }

        public ResponseModel<T> Error(string code, string str)
        {
            this.code = code;
            this.msg = str;
            this.sendDateTime = DateTime.Now;
            return this;
        }
    }

    //public class ErrorModel
    //{
        
    //}
}
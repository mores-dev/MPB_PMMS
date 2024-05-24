using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Sys
{
    /// <summary>
    /// 單筆區欄位 - SysConsRec
    /// </summary>
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SysConsRec_SaveMain : SaveBase
    {
        [DisplayName("序號")]
        [Column("CR_ID")]
        public string CR_ID { get; set; }

        [DisplayName("狀態")]
        [Column("CR_STATUS")]
        public string CR_STATUS { get; set; }

        [DisplayName("回覆日期")]
        [Column("CR_RESP_DATE")]
        public string CR_RESP_DATE { get; set; }

        [DisplayName("回覆內容")]
        [Column("CR_RESP_CONTENT")]
        public string CR_RESP_CONTENT { get; set; }
    }
}

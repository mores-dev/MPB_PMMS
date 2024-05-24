using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Sys
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SysConsRec_EditMain : EditBase
    {
        [DisplayName("序號")]
        [Column("CR_ID")]
        public string CR_ID { get; set; }

        [DisplayName("諮詢日期")]
        [Column("CR_DATE")]
        public string CR_DATE { get; set; }

        [DisplayName("狀態")]
        [Column("CR_STATUS")]
        public string CR_STATUS { get; set; }

        [DisplayName("主旨")]
        [Column("CR_TITLE")]
        public string CR_TITLE { get; set; }

        [DisplayName("內容")]
        [Column("CR_CONTENT")]
        public string CR_CONTENT { get; set; }

        [DisplayName("回覆日期")]
        [Column("CR_RESP_DATE")]
        public string CR_RESP_DATE { get; set; }

        [DisplayName("回覆內容")]
        [Column("CR_RESP_CONTENT")]
        public string CR_RESP_CONTENT { get; set; }

        [DisplayName("姓名")]
        [Column("NAME")]
        public string NAME { get; set; }

        [DisplayName("電話")]
        [Column("PHONE")]
        public string PHONE { get; set; }

        [DisplayName("EMAIL")]
        [Column("EMAIL")]
        public string EMAIL { get; set; }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Sys
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SysConsRec_QueryCondition : QueryBase
    {
        //提問日期-起
        [DisplayName("諮詢日期-起")]
        public string CR_DATE_START { get; set; }

        //提問日期-迄
        [DisplayName("諮詢日期-迄")]
        public string CR_DATE_END { get; set; }

        //提問日期-迄
        [DisplayName("諮詢日期")]
        public string CR_DATE { get; set; }

        //狀態
        [DisplayName("狀態")]
        public string CR_STATUS { get; set; }

    }

    public class SysConsRec_QueryResult
    {
        [DisplayName("諮詢日期")]
        [Column("CR_DATE")]
        public string CR_DATE { get; set; }

        [DisplayName("姓名")]
        [Column("NAME")]
        public string NAME { get; set; }

        [DisplayName("狀態")]
        [Column("CR_STATUS")]
        public string CR_STATUS { get; set; }

        [DisplayName("主旨")]
        [Column("CR_TITLE")]
        public string CR_TITLE { get; set; }

        [DisplayName("聯絡方式")]
        [Column("PHONE")]
        public string PHONE { get; set; }

        [DisplayName("內容")]
        [Column("CR_CONTENT")]
        public string CR_CONTENT { get; set; }

        [DisplayName("序號")]
        [Column("CR_ID")]
        public string CR_ID { get; set; }
    }
}

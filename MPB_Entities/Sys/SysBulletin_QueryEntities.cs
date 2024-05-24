using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Sys
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SysBulletin_QueryCondition : QueryBase
    {
        //公告日期-起
        [DisplayName("公告日期-起")]
        public string SB_DATE_START { get; set; }

        //公告日期-迄
        [DisplayName("公告日期-迄")]
        public string SB_DATE_END { get; set; }

        //公告日期
        [DisplayName("公告日期")]
        public string SB_DATE { get; set; }

        //公告類別
        [DisplayName("公告類別")]
        public string SB_TYPE { get; set; }

        //標題
        [DisplayName("標題")]
        public string SB_TITLE { get; set; }

        //標題
        [DisplayName("C_ID")]
        public string C_ID { get; set; }
    }

    public class SysBulletin_QueryResult
    {
        [DisplayName("公告日期")]
        [Column("SB_DATE")]
        public string SB_DATE { get; set; }

        [DisplayName("標題")]
        [Column("SB_TITLE")]
        public string SB_TITLE { get; set; }

        [DisplayName("內容")]
        [Column("SB_CONTENT")]
        public string SB_CONTENT { get; set; }

        [DisplayName("公告單位")]
        [Column("SB_TYPE")]
        public string SB_TYPE { get; set; }

        [DisplayName("公告序號")]
        [Column("SB_ID")]
        public string SB_ID { get; set; }

        //button modify
        [Column("BM")]
        public string BM { get; set; }
    }
}

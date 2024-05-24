using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Sys
{
    /// <summary>
    /// 單筆區欄位 - SysBulletin
    /// </summary>
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SysBulletin_SaveMain : SaveBase
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
    }
}

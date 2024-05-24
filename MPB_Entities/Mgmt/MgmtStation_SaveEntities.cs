using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Mgmt
{
    /// <summary>
    /// 單筆區欄位 - MgmtStation
    /// </summary>
    [System.Web.Mvc.Bind(Exclude = "")]
    public class MgmtStation_SaveMain : SaveBase
    {
        [DisplayName("場站序號")]
        [Column("ST_ID")]
        public string ST_ID { get; set; }

        [DisplayName("場站代號")]
        [Column("ST_CODE")]
        public string ST_CODE { get; set; }

        [DisplayName("場站名稱")]
        [Column("ST_NAME")]
        public string ST_NAME { get; set; }

        [DisplayName("狀態")]
        [Column("ST_STATUS")]
        public string ST_STATUS { get; set; }

        [DisplayName("備註")]
        [Column("ST_MEMO")]
        public string ST_MEMO { get; set; }
    }
}

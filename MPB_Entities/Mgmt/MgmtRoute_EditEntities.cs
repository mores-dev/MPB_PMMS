using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Mgmt
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class MgmtRoute_EditMain : EditBase
    {
        [DisplayName("航線序號")]
        [Column("R_ID")]
        public string R_ID { get; set; }

        [DisplayName("航線代號")]
        [Column("R_CODE")]
        public string R_CODE { get; set; }

        [DisplayName("航線名稱")]
        [Column("R_NAME")]
        public string R_NAME { get; set; }

        [DisplayName("狀態")]
        [Column("R_STATUS")]
        public string R_STATUS { get; set; }

        [DisplayName("起站")]
        [Column("ST_ID_START")]
        public string ST_ID_START { get; set; }

        [DisplayName("末站")]
        [Column("ST_ID_END")]
        public string ST_ID_END { get; set; }
    }

    public class MgmtRoute_EditDetailGrid
    {
        [DisplayName("航線序號")]
        [Column("R_ID")]
        public string R_ID { get; set; }

        [DisplayName("場站序號")]
        [Column("ST_ID")]
        public string ST_ID { get; set; }

        [DisplayName("排序")]
        [Column("ST_ODR")]
        public string ST_ODR { get; set; }
    }
}

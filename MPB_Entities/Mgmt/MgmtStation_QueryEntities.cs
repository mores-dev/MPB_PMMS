using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Mgmt
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class MgmtStation_QueryCondition : QueryBase
    {
        //館區
        [DisplayName("場站代號")]
        public string ST_CODE{ get; set; }

        //設備類型
        [DisplayName("場站名稱")]
        public string ST_NAME { get; set; }

        //設備編號
        [DisplayName("狀態")]
        public string ST_STATUS { get; set; }

    }

    public class MgmtStation_QueryResult
    {
        [DisplayName("場站代號")]
        [Column("ST_CODE")]
        public string ST_CODE { get; set; }

        [DisplayName("場站名稱")]
        [Column("ST_NAME")]
        public string ST_NAME { get; set; }

        [DisplayName("狀態")]
        [Column("ST_STATUS")]
        public string ST_STATUS { get; set; }

        [DisplayName("場站序號")]
        [Column("ST_ID")]
        public string ST_ID { get; set; }
    }
}

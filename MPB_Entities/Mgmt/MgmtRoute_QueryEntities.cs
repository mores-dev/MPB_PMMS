using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Mgmt
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class MgmtRoute_QueryCondition : QueryBase
    {
        //館區
        [DisplayName("航線代號")]
        public string R_CODE{ get; set; }

        //設備類型
        [DisplayName("航線名稱")]
        public string R_NAME { get; set; }

        //設備編號
        [DisplayName("狀態")]
        public string R_STATUS { get; set; }

    }

    public class MgmtRoute_QueryResult
    {
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

        [DisplayName("航線序號")]
        [Column("R_ID")]
        public string R_ID { get; set; }
    }
}

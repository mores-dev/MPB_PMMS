using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Mgmt
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class MgmtCompany_QueryCondition : QueryBase
    {
        //航商代號
        [DisplayName("航商代號")]
        public string C_CODE{ get; set; }

        //航商名稱
        [DisplayName("航商名稱")]
        public string C_NAME { get; set; }

        //狀態
        [DisplayName("狀態")]
        public string C_STATUS { get; set; }

    }

    public class MgmtCompany_QueryResult
    {
        [DisplayName("航商代號")]
        [Column("C_CODE")]
        public string C_CODE { get; set; }

        [DisplayName("航商名稱")]
        [Column("C_NAME")]
        public string C_NAME { get; set; }

        [DisplayName("狀態")]
        [Column("C_STATUS")]
        public string C_STATUS { get; set; }

        [DisplayName("航商統編")]
        [Column("C_TAX_ID")]
        public string C_TAX_ID { get; set; }

        [DisplayName("航商序號")]
        [Column("C_ID")]
        public string C_ID { get; set; }
    }
}

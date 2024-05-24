using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Sys
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SysMbrMgmt_QueryCondition : QueryBase
    {
        //帳號
        [DisplayName("帳號")]
        public string GaAAA { get; set; }

        //類型
        [DisplayName("類型")]
        public string GaType { get; set; }

        [DisplayName("業者名稱")]
        public string GaName { get; set; }

        [DisplayName("聯絡人")]
        public string Contact { get; set; }

        //狀態
        [DisplayName("狀態")]
        public string GA_STATUS { get; set; }

        [DisplayName("C_ID")]
        public string C_ID { get; set; }
    }

    public class SysMbrMgmt_QueryResult
    {
        [DisplayName("類型")]
        [Column("GA_TYPE")]
        public string GaType { get; set; }

        [DisplayName("帳號")]
        [Column("GA_ACCOUNT")]
        public string GaAAA { get; set; }

        [DisplayName("業者名稱")]
        [Column("GA_NAME")]
        public string GaName { get; set; }

        [DisplayName("聯絡人")]
        [Column("CONTACT")]
        public string Contact { get; set; }

        [DisplayName("連絡電話")]
        [Column("PHONE")]
        public string Phone { get; set; }

        [DisplayName("狀態")]
        [Column("GA_STATUS")]
        public string GaStatus { get; set; }

        //button modify
        [Column("BM")]
        public string BM { get; set; }
    }
}

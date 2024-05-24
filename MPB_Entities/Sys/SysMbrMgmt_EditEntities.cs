using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Sys
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SysMbrMgmt_EditMain : EditBase
    {
        [DisplayName("帳號")]
        [Column("ACCOUNT")]
        public string GaAAA { get; set; }

        [DisplayName("航商代號")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        [DisplayName("類型")]
        [Column("GA_TYPE")]
        public string GaType { get; set; }

        [DisplayName("業者名稱")]
        [Column("GA_NAME")]
        public string GaName { get; set; }

        [DisplayName("密碼")]
        [Column("GA_PPP")]
        public string GA_PPP { get; set; }

        [DisplayName("密碼確認")]
        [Column("GA_PPPC")]
        public string GA_PPPC { get; set; }

        [DisplayName("統一編號")]
        [Column("UNI_ID")]
        public string UniId { get; set; }

        [DisplayName("EMAIL")]
        [Column("EMAIL")]
        public string Email { get; set; }

        [DisplayName("電話類型")]
        [Column("PHONE_TYPE")]
        public string PhoneType { get; set; }

        [DisplayName("區碼")]
        [Column("AREA_NUMBER")]
        public string AreaNumber { get; set; }

        [DisplayName("分機")]
        [Column("EXT")]
        public string Ext { get; set; }

        [DisplayName("電話號碼")]
        [Column("PHONE")]
        public string Phone { get; set; }

        [DisplayName("聯絡人")]
        [Column("CONTACT")]
        public string Contact { get; set; }

        [DisplayName("狀態")]
        [Column("GA_STATUS")]
        public string GaStatus { get; set; }

    }
}

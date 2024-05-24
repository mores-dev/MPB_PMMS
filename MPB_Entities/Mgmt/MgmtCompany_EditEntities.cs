using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Mgmt
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class MgmtCompany_EditMain : EditBase
    {
        [DisplayName("航商序號")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        [DisplayName("航商代號")]
        [Column("C_CODE")]
        public string C_CODE { get; set; }

        [DisplayName("航商名稱")]
        [Column("C_NAME")]
        public string C_NAME { get; set; }

        [DisplayName("航商名稱_英文")]
        [Column("C_NAME_EN")]
        public string C_NAME_EN { get; set; }

        [DisplayName("狀態")]
        [Column("C_STATUS")]
        public string C_STATUS { get; set; }

        [DisplayName("統編")]
        [Column("C_TAX_ID")]
        public string C_TAX_ID { get; set; }

        [DisplayName("備註")]
        [Column("C_MEMO")]
        public string C_MEMO { get; set; }

        [DisplayName("連絡電話")]
        [Column("PHONE")]
        public string Phone { get; set; }

        [DisplayName("地址")]
        [Column("C_ADDRESS")]
        public string C_Address { get; set; }

        [DisplayName("EMAIL")]
        [Column("EMAIL")]
        public string Email { get; set; }

        [DisplayName("訂票電話")]
        [Column("ODR_PHONE")]
        public string Odr_Phone { get; set; }

        [DisplayName("官網網址")]
        [Column("LAND_URL")]
        public string LAND_URL { get; set; }

        [DisplayName("票價網址")]
        [Column("PRICE_URL")]
        public string Price_URL { get; set; }

        [DisplayName("訂票網站")]
        [Column("ODR_URL")]
        public string Odr_URL { get; set; }

        [DisplayName("Logo網址")]
        [Column("LOGO_URL")]
        public string Logo_URL { get; set; }
    }
}

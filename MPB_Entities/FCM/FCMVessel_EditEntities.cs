using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.FCM
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class FCMVessel_EditMain : EditBase
    {
        [DisplayName("航商名稱")]
        [Column("C_NAME")]
        public string C_NAME { get; set; }

        [DisplayName("航商序號")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        [DisplayName("船隻序號")]
        [Column("V_ID")]
        public string V_ID { get; set; }

        [DisplayName("船舶號數")]
        [Column("V_CODE")]
        public string V_CODE { get; set; }

        [DisplayName("船名")]
        [Column("V_NAME")]
        public string V_NAME { get; set; }

        [DisplayName("船名_MTNet")]
        [Column("API_NAME")]
        public string API_NAME { get; set; }

        [DisplayName("船名_英文")]
        [Column("V_NAME_EN")]
        public string V_NAME_EN { get; set; }

        [DisplayName("狀態")]
        [Column("V_STATUS")]
        public string V_STATUS { get; set; }

        [DisplayName("乘客限額")]
        [Column("MAXIMUM")]
        public int MAXIMUM { get; set; }

        [DisplayName("所屬公司名稱")]
        [Column("BelongCompany")]
        public string BelongCompany { get; set; }

        [DisplayName("所屬公司統編")]
        [Column("BelongTAX_ID")]
        public string BelongTAX_ID { get; set; }

        [DisplayName("AIS船舶編號")]
        [Column("MMSI")]
        public string MMSI { get; set; }

        [DisplayName("國際船舶編號")]
        [Column("IMO")]
        public string IMO { get; set; }

        [DisplayName("船呼")]
        [Column("CallSign")]
        public string CallSign { get; set; }

        [DisplayName("船舶種類")]
        [Column("VesselType")]
        public string VesselType { get; set; }

        [DisplayName("船舶總噸位(噸)")]
        [Column("GrossTonnage")]
        public string GrossTonnage { get; set; }

        [DisplayName("船長(公尺)")]
        [Column("VesselLength")]
        public string VesselLength { get; set; }

        [DisplayName("船寬(公尺)")]
        [Column("VesselWidth")]
        public string VesselWidth { get; set; }

        [DisplayName("吃水深度(公尺)")]
        [Column("LoadDraft")]
        public string LoadDraft { get; set; }

        [DisplayName("備註")]
        [Column("MEMO")]
        public string Memo { get; set; }
    }
}

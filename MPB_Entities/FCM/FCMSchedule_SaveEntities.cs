using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.FCM
{
    /// <summary>
    /// 單筆區欄位 - FCMSchedule
    /// </summary>
    [System.Web.Mvc.Bind(Exclude = "")]
    public class FCMSchedule_SaveMain : SaveBase
    {
        [DisplayName("航商名稱")]
        [Column("C_NAME")]
        public string C_NAME { get; set; }

        [DisplayName("航商序號")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        [DisplayName("航班序號")]
        [Column("SC_ID")]
        public string SC_ID { get; set; }

        [DisplayName("航班代號")]
        [Column("SC_CODE")]
        public string SC_CODE { get; set; }

        [DisplayName("航班名稱")]
        [Column("SC_NAME")]
        public string SC_NAME { get; set; }

        [DisplayName("狀態")]
        [Column("SC_STATUS")]
        public string SC_STATUS { get; set; }

        [DisplayName("時段")]
        [Column("SC_TIME")]
        public string SC_TIME { get; set; }

        [DisplayName("套用航線")]
        [Column("R_ID")]
        public string R_ID { get; set; }

        [DisplayName("預估航行時間")]
        [Column("TRAVEL_TIME")]
        public int TravelTime { get; set; }
    }
}

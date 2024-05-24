using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.FCM
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class PsgrUpload_QueryCondition : QueryBase
    {
        //航商代號
        [DisplayName("航商代號")]
        public string C_ID { get; set; }
        
        //上傳
        [DisplayName("上傳")]
        public string Upload { get; set; }
    }

    public class PsgrUpload_QueryResult
    {
        [DisplayName("航商代號")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        [DisplayName("航商名稱")]
        [Column("C_NAME")]
        public string CName { get; set; }

        [DisplayName("航班名稱")]
        [Column("R_NAME")]
        public string RName { get; set; }

        [DisplayName("航班時間")]
        [Column("VOYAGE_TIME")]
        public string VoyageTime { get; set; }

        [DisplayName("船隻名稱")]
        [Column("VESSEL_NAME")]
        public string VesselName { get; set; }

        [DisplayName("登錄人數")]
        [Column("PASSENGER_CNT")]
        public string PassengerCnt { get; set; }

        [DisplayName("上傳狀態")]
        [Column("UPLOAD_STS")]
        public string UploadSts { get; set; }

    }
}

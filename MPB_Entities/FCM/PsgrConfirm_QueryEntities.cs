using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.FCM
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class PsgrConfirm_QueryCondition : QueryBase
    {
        //航商代號
        [DisplayName("航商代號")]
        public string C_ID { get; set; }

        //航站
        [DisplayName("航站_1")]
        public string Station { get; set; }

        //航班時間
        [DisplayName("航班時間_2")]
        public string VoyageTime { get; set; }

        //船舶代號
        [DisplayName("船舶代號_3")]
        public string VesselId { get; set; }

        public string QueryStr { get; set; }

    }

    public class PsgrConfirm_QueryResult
    {
        [DisplayName("航商名稱")]
        [Column("C_NAME")]
        public string CName { get; set; }

        [DisplayName("航班名稱")]
        [Column("R_NAME")]
        public string RName { get; set; }

        [DisplayName("船隻名稱")]
        [Column("VESSEL_NAME")]
        public string VesselName { get; set; }

        [DisplayName("乘客姓名")]
        [Column("PSGR_NAME")]
        public string PsgrName { get; set; }

        [DisplayName("證件別")]
        [Column("ID_TYPE")]
        public string IdType { get; set; }

        [DisplayName("證件號碼")]
        [Column("ID_NO")]
        public string IdNo { get; set; }

        [DisplayName("出生日期")]
        [Column("BIRTH")]
        public string Birth { get; set; }

        //航商代號
        [DisplayName("航商代號")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        //航站
        [DisplayName("航站_1")]
        [Column("STATION")]
        public string Station { get; set; }

        //航班時間
        [DisplayName("航班時間_2")]
        [Column("VOYAGE_TIME")]
        public string VoyageTime { get; set; }

        //船舶代號
        [DisplayName("船舶代號_3")]
        [Column("VESSEL_ID")]
        public string VesselId { get; set; }

    }
}

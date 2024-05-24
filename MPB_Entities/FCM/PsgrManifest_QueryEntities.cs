using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.FCM
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class PsgrManifest_QueryCondition : QueryBase
    {

        [DisplayName("日期")]
        public string ShippingDt { get; set; }

        [DisplayName("航商代號")]
        public string C_ID { get; set; }

        [DisplayName("場站")]
        public string Station { get; set; }

        [DisplayName("航班")]
        public string VoyageTime { get; set; }

    }

    public class PsgrManifest_ParamName
    {

        [DisplayName("日期")]
        public string ShippingDt { get; set; }

        [DisplayName("航商代號")]
        public string C_ID { get; set; }

        [DisplayName("場站")]
        public string Station { get; set; }

        [DisplayName("航班")]
        public string VoyageTime { get; set; }

        [DisplayName("使用者")]
        public string UserName { get; set; }

        [DisplayName("驗票人數")]
        public int DtlCnt { get; set; }

    }
}

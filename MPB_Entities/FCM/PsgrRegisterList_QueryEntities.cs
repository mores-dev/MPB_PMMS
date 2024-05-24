using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.FCM
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class PsgrRegisterList_QueryCondition : QueryBase
    {

        [DisplayName("預約日期")]
        public string OdrDt { get; set; }

        [DisplayName("預約類別")]
        public string OdrType { get; set; }

        [DisplayName("預約類別_名稱")]
        public string OdrTypeName { get; set; }

        [DisplayName("航商代號")]
        public string C_ID { get; set; }

    }
}

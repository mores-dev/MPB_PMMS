using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.FCM
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class NCCRStatistic_QueryCondition : QueryBase
    {
        [DisplayName("日期")]
        public string OdrDtStart { get; set; }

        [DisplayName("日期")]
        public string OdrDtEnd { get; set; }
    }
}

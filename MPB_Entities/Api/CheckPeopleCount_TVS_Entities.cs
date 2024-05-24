using System.Collections.Generic;
using DataAccessUtility;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class CheckPeopleCount_TVS_FUNC_Entities
    {
        public string BUSINESS_DATE { get; set; }
        public string R_ID { get; set; }
        public string SC_TIME { get; set; }
        public string V_ID { get; set; }
    }

    public class CheckPeopleCount_TVS_RSPN_Entities
    {
        [Column("BUSINESS_DATE")]
        public string BUSINESS_DATE { get; set; }

        [Column("R_ID")]
        public string R_ID { get; set; }

        [Column("SC_TIME")]
        public string SC_TIME { get; set; }

        [Column("V_ID")]
        public string V_ID { get; set; }

        [Column("PEOPLE_CNT")]
        public int PEOPLE_CNT { get; set; }

        [Column("BABY_CNT")]
        public int BABY_CNT { get; set; }
    }
}

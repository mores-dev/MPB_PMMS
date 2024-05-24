using System.Collections.Generic;
using System.ComponentModel;
using DataAccessUtility;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class QueryVessel_FUNC_Entities
    {
        public string SHIPPING_DATE { get; set; }
    }

    public class QueryVessel_RSPN_Entities
    {
        public string SHIPPING_DATE { get; set; }
        public List<QueryVessel_VESSEL_Entities> VESSELS { get; set; }
    }

    public class QueryVessel_VESSEL_Entities
    {
        [Column("VESSEL_ID")]
        public string VESSEL_ID { get; set; }

        [Column("VESSEL_NAME")]
        public string VESSEL_NAME { get; set; }

        [Column("VOYAGE")]
        public string VOYAGE { get; set; }

        [Column("VOYAGE_TIME")]
        public string VOYAGE_TIME { get; set; }

        [Column("STATION")]
        public string STATION { get; set; }

        [Column("PASSENGER_NO")]
        public string PASSENGER_NO { get; set; }
    }
}

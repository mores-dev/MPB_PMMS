using System.Collections.Generic;
using System.ComponentModel;
using DataAccessUtility;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class QueryManifest_FUNC_Entities
    {
        public string SHIPPING_DATE { get; set; }
        public string VOYAGE { get; set; }
        public string VOYAGE_TIME { get; set; }
        public string STATION { get; set; }
    }

    public class QueryManifest_RSPN_Entities
    {
        public string SHIPPING_DATE { get; set; }
        public string VESSEL_ID { get; set; }
        public string VESSEL_NAME { get; set; }
        public string VOYAGE { get; set; }
        public string VOYAGE_TIME { get; set; }
        public string STATION { get; set; }
        public string PASSENGER_CNT { get; set; }
        public List<QueryManifestDtl_Entities> Dtls { get; set; }
    }

    public class QueryManifestDtl_Entities
    {
        [Column("TICKET_NO")]
        public string TICKET_NO { get; set; }
        [Column("BOARDING_DT")]
        public string BOARDING_DT { get; set; }
        [Column("ID_TYPE")]
        public string ID_TYPE { get; set; }
        [Column("ID_NO")]
        public string ID_NO { get; set; }
        [Column("PSGR_NAME")]
        public string NAME { get; set; }
        [Column("BIRTH")]
        public string BIRTH { get; set; }
        [Column("SEX")]
        public string SEX { get; set; }
        [Column("PHONE")]
        public string PHONE { get; set; }
        [Column("AGENT_ID")]
        public string AGENT_ID { get; set; }
        [Column("AGENT_NAME")]
        public string AGENT_NAME { get; set; }
    }
}

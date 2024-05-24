using System.Collections.Generic;
using DataAccessUtility;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class SyncOperData_TVS_FUNC_Entities
    {
        public string ACCOUNT { get; set; }
        public string C_ID { get; set; }
    }

    public class SyncOperData_TVS_RSPN_Entities
    {
        public List<SyncOperData_TVS_Route_Entities> ROUTES { get; set; }

        public List<SyncOperData_TVS_Vessel_Entities> VESSELS { get; set; }

        public List<SyncOperData_TVS_Schedule_Entities> SCHEDULES { get; set; }

        public List<SyncOperData_TVS_Station_Entities> STATIONS { get; set; }

        public List<SyncOperData_TVS_Company_Entities> COMPANIES { get; set; }
    }

    public class SyncOperData_TVS_Route_Entities
    {
        [Column("R_ID")]
        public string R_ID { get; set; }

        [Column("R_NAME")]
        public string R_NAME { get; set; }

        [Column("ST_ID_START")]
        public string ST_ID_START { get; set; }

        [Column("ST_ID_END")]
        public string ST_ID_END { get; set; }
    }

    public class SyncOperData_TVS_Vessel_Entities
    {
        [Column("V_ID")]
        public string V_ID { get; set; }

        [Column("V_NAME")]
        public string V_NAME { get; set; }

        [Column("MAXIMUM")]
        public int MAXIMUM { get; set; }
    }

    public class SyncOperData_TVS_Schedule_Entities
    {
        [Column("SC_ID")]
        public string SC_ID { get; set; }

        [Column("SC_NAME")]
        public string SC_NAME { get; set; }

        [Column("SC_TIME")]
        public string SC_TIME { get; set; }

        [Column("R_ID")]
        public string R_ID { get; set; }
    }

    public class SyncOperData_TVS_Station_Entities
    {
        [Column("ST_ID")]
        public string ST_ID { get; set; }

        [Column("ST_NAME")]
        public string ST_NAME { get; set; }
    }

    public class SyncOperData_TVS_Company_Entities
    {
        [Column("C_ID")]
        public string C_ID { get; set; }

        [Column("C_NAME")]
        public string C_NAME { get; set; }
    }
}

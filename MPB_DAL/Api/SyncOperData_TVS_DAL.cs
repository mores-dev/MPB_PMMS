using MPB_Entities.Api;
using System.Collections.Generic;

namespace MPB_DAL.Api
{
    public class SyncOperData_TVS_DAL : DALBase
    {
        public SyncOperData_TVS_DAL() { }

        public SyncOperData_TVS_DAL(DbManager db) : base(db) { }

        public string Get_C_ID(SyncOperData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT TOP 1 C_ID FROM cAFCAccount WHERE ACCOUNT = @ACCOUNT AND UserType = 'C' AND C_ID > '' ";

            return SingleOrDefault<string>(@sql, qc);
        }

        public List<SyncOperData_TVS_Route_Entities> Select_Route()
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //航線序號
            sql += " A.R_ID ";
            //航線名稱
            sql += " , A.R_NAME ";
            //起站
            sql += " , A.ST_ID_START ";
            //終點站
            sql += " , A.ST_ID_END ";

            //FROM AND LEFT JOIN 
            sql += " FROM cRoute A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.R_STATUS = '1' ";
            sql += " ORDER BY A.R_ID";

            return Fetch<SyncOperData_TVS_Route_Entities>(@sql);
        }

        public List<SyncOperData_TVS_Vessel_Entities> Select_Vessel(SyncOperData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //船隻序號
            sql += " A.V_ID ";
            //船名
            sql += " , A.V_NAME ";
            //乘客限額
            sql += " , A.MAXIMUM ";

            //FROM AND LEFT JOIN 
            sql += " FROM cVessel A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.C_ID = @C_ID";
            sql += "   And A.V_STATUS = '1' ";
            sql += " ORDER BY A.V_ID";

            return Fetch<SyncOperData_TVS_Vessel_Entities>(@sql, qc);
        }

        public List<SyncOperData_TVS_Schedule_Entities> Select_Schedule(SyncOperData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //航班序號
            sql += " A.SC_ID ";
            //航班名稱
            sql += " , A.SC_NAME ";
            //航班時刻
            sql += " , A.SC_TIME ";
            //套用航線
            sql += " , A.R_ID ";

            //FROM AND LEFT JOIN 
            sql += " FROM cSchedule A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.C_ID = @C_ID";
            sql += "   And A.SC_STATUS = '1' ";
            sql += " ORDER BY A.SC_ID";

            return Fetch<SyncOperData_TVS_Schedule_Entities>(@sql, qc);
        }

        public List<SyncOperData_TVS_Station_Entities> Select_Station()
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //場站序號
            sql += " A.ST_ID ";
            //場站名稱
            sql += " , A.ST_NAME ";

            //FROM AND LEFT JOIN 
            sql += " FROM cStation A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.ST_STATUS = '1' ";
            sql += " ORDER BY A.ST_ID";

            return Fetch<SyncOperData_TVS_Station_Entities>(@sql);
        }

        public List<SyncOperData_TVS_Company_Entities> Select_Company(SyncOperData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID ";
            //航商名稱
            sql += " , A.C_NAME ";

            //FROM AND LEFT JOIN 
            sql += " FROM cCompany A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.C_ID = @C_ID";
            sql += "   And A.C_STATUS = '1' ";
            sql += " ORDER BY A.C_ID";

            return Fetch<SyncOperData_TVS_Company_Entities>(@sql, qc);
        }
    }
}

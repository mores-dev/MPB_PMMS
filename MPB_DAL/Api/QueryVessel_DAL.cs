using MPB_Entities.Api;
using NLog.LayoutRenderers;
using System.Collections.Generic;

namespace MPB_DAL.Api
{
    public class QueryVessel_DAL : DALBase
    {
        public QueryVessel_DAL() { }

        public QueryVessel_DAL(DbManager db) : base(db) { }

        public List<QueryVessel_VESSEL_Entities> Select_Vessel(QueryVessel_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //船舶號數
            sql += " A.V_ID AS VESSEL_ID ";
            //船舶名稱
            sql += " , B.V_NAME AS VESSEL_NAME ";
            //班次
            sql += " , CASE WHEN ISNULL(A.SC_ID, '') = '' THEN A.SC_TIME ELSE A.SC_ID END AS VOYAGE ";
            //班次時間
            sql += " , A.SC_TIME AS VOYAGE_TIME ";
            //航站
            sql += " , CASE WHEN C.ST_ID_START = 'ST00001' THEN 'D' WHEN C.ST_ID_START = 'ST00002' THEN 'L' ELSE '' END AS STATION ";
            //乘客限額
            sql += " , B.MAXIMUM AS PASSENGER_NO ";

            //FROM AND LEFT JOIN 
            sql += " FROM pPsgrManifest A ";
            sql += " LEFT JOIN cVessel B ON A.V_ID = B.V_ID ";
            sql += " LEFT JOIN cRoute C ON A.R_ID = C.R_ID ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.BUSINESS_DATE = @SHIPPING_DATE ";

            return Fetch<QueryVessel_VESSEL_Entities>(@sql, qc);
        }
    }
}

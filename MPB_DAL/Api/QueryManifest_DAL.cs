using MPB_Entities.Api;
using System.Collections.Generic;

namespace MPB_DAL.Api
{
    public class QueryManifest_DAL : DALBase
    {
        public QueryManifest_DAL() { }

        public QueryManifest_DAL(DbManager db) : base(db) { }

        public QueryManifest_RSPN_Entities Select_Manifest(QueryManifest_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT TOP 1 ";

            //船舶號數
            sql += " CONVERT(VARCHAR, A.SHIPPING_DATE) AS SHIPPING_DATE ";
            //船舶名稱
            sql += " , A.STATION ";
            //班次
            sql += " , A.VOYAGE_TIME ";
            //班次時間
            sql += " , A.VESSEL_ID ";
            //航站
            sql += " , A.VESSEL_NAME ";
            //乘客限額
            sql += " , A.VOYAGE ";
            //乘客人數
            sql += " , A.PASSENGER_CNT ";

            //FROM AND LEFT JOIN 
            sql += " FROM cManifest A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.SHIPPING_DATE = @SHIPPING_DATE ";
            sql += "   And A.STATION = @STATION ";
            sql += "   And A.VOYAGE_TIME = @VOYAGE_TIME ";

            return SingleOrDefault<QueryManifest_RSPN_Entities>(@sql, qc);
        }

        public List<QueryManifestDtl_Entities> Select_ManifestDtl(QueryManifest_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //票券號碼
            sql += " A.TICKET_NO ";
            //登船時間
            sql += " , CONVERT(VARCHAR, A.BOARDING_DT, 120) AS BOARDING_DT ";
            //證件類別  0：身分證，1：護照，2：居留證，3：鄉民卡
            sql += " , A.ID_TYPE ";
            //證件號碼
            sql += " , A.ID_NO ";
            //姓名
            sql += " , A.PSGR_NAME ";
            //出生日期
            sql += " , CONVERT(VARCHAR, A.BIRTH) AS BIRTH ";
            //性別
            sql += " , A.SEX ";
            //連絡電話
            sql += " , A.PHONE ";
            //旅行社代碼
            sql += " , A.AGENT_ID ";
            //旅行社名稱
            sql += " , A.AGENT_NAME ";

            //FROM AND LEFT JOIN 
            sql += " FROM cManifestDtl A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.SHIPPING_DATE = @SHIPPING_DATE ";
            sql += "   And A.STATION = @STATION ";
            sql += "   And A.VOYAGE_TIME = @VOYAGE_TIME ";

            return Fetch<QueryManifestDtl_Entities>(@sql, qc);
        }
    }
}

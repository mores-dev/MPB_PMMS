using MPB_Entities.Api;
using System.Collections.Generic;

namespace MPB_DAL.Api
{
    public class CheckPeopleCount_TVS_DAL : DALBase
    {
        public CheckPeopleCount_TVS_DAL() { }

        public CheckPeopleCount_TVS_DAL(DbManager db) : base(db) { }

        public int Get_People_Count(CheckPeopleCount_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //乘船人數
            sql += " SUM(A.SEAT_CNT) AS PEOPLE_CNT ";

            //FROM AND LEFT JOIN 
            sql += " FROM pPsgrManifestDtl A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.BUSINESS_DATE = @BUSINESS_DATE ";
            sql += "   And A.R_ID = @R_ID ";
            sql += "   And A.SC_TIME = @SC_TIME ";
            sql += "   And A.V_ID = @V_ID ";
            sql += " GROUP BY A.BUSINESS_DATE, A.R_ID, A.SC_TIME, A.V_ID";

            return SingleOrDefault<int>(@sql, qc);
        }

        public int Get_Baby_Count(CheckPeopleCount_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            //乘船人數
            sql += " SUM(A.ALLOW_CNT - A.SEAT_CNT) AS BABY_CNT ";

            //FROM AND LEFT JOIN 
            sql += " FROM pPsgrManifestDtl A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.BUSINESS_DATE = @BUSINESS_DATE ";
            sql += "   And A.R_ID = @R_ID ";
            sql += "   And A.SC_TIME = @SC_TIME ";
            sql += "   And A.V_ID = @V_ID ";
            sql += " GROUP BY A.BUSINESS_DATE, A.R_ID, A.SC_TIME, A.V_ID";

            return SingleOrDefault<int>(@sql, qc);
        }
    }
}

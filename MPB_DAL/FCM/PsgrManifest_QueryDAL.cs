using MPB_Entities.COMMON;
using MPB_Entities.FCM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace MPB_DAL.FCM
{
    public class PsgrManifest_QueryDAL : DALBase
    {
        public PsgrManifest_QueryDAL() { }

        public PsgrManifest_QueryDAL(DbManager db) : base(db) { }

        public DataSet GetPrint1List(DbParameter[] parms)
        {
            return ExecuteStoredProcedure("SP_Report_PsgrManifest", parms);
        }

        public List<CodeName> SelectVoyageTime(PsgrManifest_QueryCondition qc)
        {
            string sql = "";

            sql += "Select VOYAGE_TIME as code";

            sql += " , SUBSTRING(A.VOYAGE_TIME, 1, 2) + ':' + SUBSTRING(A.VOYAGE_TIME, 3, 2)";
            sql += "   + ' ' + A.VESSEL_NAME as name";

            sql += "  From cManifest A";
            sql += " Where 1 = 1";

            sql += "   And A.SHIPPING_DATE = @ShippingDt";
            sql += "   And A.C_ID = @C_ID";
            sql += "   And A.STATION = @Station";

            return Fetch<CodeName>(@sql, qc);
        }
    }
}

using MPB_Entities.Api;
using System.Collections.Generic;

namespace MPB_DAL.Api
{
    public class CheckGroupStatus_TVS_DAL : DALBase
    {
        public CheckGroupStatus_TVS_DAL() { }

        public CheckGroupStatus_TVS_DAL(DbManager db) : base(db) { }

        public CheckGroupStatus_TVS_RSPN_Entities Get_Group_Status(CheckGroupStatus_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            sql += " A.GR_NO ";
            sql += " , ISNULL(A.EDIT_LOCK, 'N') AS EDIT_LOCK ";

            //FROM AND LEFT JOIN 
            sql += " FROM rGroupRecord A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.GR_NO = @GR_NO ";

            return SingleOrDefault<CheckGroupStatus_TVS_RSPN_Entities>(@sql, qc);
        }
    }
}

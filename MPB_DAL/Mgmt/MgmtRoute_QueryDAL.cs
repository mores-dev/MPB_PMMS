
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtRoute_QueryDAL : DALBase
    {
        public MgmtRoute_QueryDAL() { }

        public MgmtRoute_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<MgmtRoute_QueryResult> GetPageList(MgmtRoute_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航線序號
            sql += " A.R_ID";
            //航線代碼
            sql += " , A.R_CODE";
            //航線名稱
            sql += " , A.R_NAME";
            //備註
            sql += " , A.R_MEMO";
            //起站
            sql += " , B.ST_NAME AS ST_ID_START";
            //末站
            sql += " , B1.ST_NAME AS ST_ID_END";
            //是否啟用  
            sql += ",  CASE WHEN A.R_STATUS = '1' THEN '啟用' ELSE '' END ";
            sql += " AS R_STATUS ";
            //FROM AND LEFT JOIN 
            sql += " FROM cRoute A ";
            sql += " LEFT JOIN cStation B ON B.ST_ID = A.ST_ID_START ";
            sql += " LEFT JOIN cStation B1 ON B1.ST_ID = A.ST_ID_END ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //sql += "   AND R_STATUS = '1' ";

            //查詢條件 
            if (!string.IsNullOrWhiteSpace(qc.R_CODE))
            {
                sql += " and A.R_CODE = @R_CODE ";
            }
            //查詢條件 航線名稱
            if (!string.IsNullOrWhiteSpace(qc.R_NAME))
            {
                sql += " and A.R_NAME like '%' + @R_NAME + '%' ";
            }
            //查詢條件 狀態
            if (!string.IsNullOrWhiteSpace(qc.R_STATUS))
            {
                sql += " and A.R_STATUS = @R_STATUS ";
            }

            return PageList<MgmtRoute_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

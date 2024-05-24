
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtStation_QueryDAL : DALBase
    {
        public MgmtStation_QueryDAL() { }

        public MgmtStation_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<MgmtStation_QueryResult> GetPageList(MgmtStation_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //場站序號
            sql += " A.ST_ID";
            //場站代碼
            sql += " , A.ST_CODE";
            //場站名稱
            sql += " , A.ST_NAME";
            //備註
            sql += " , A.ST_MEMO";
            //是否啟用  
            sql += ",  case when A.ST_STATUS= '1' then '啟用' else '' end ";
            sql += " AS ST_STATUS ";
            //FROM AND LEFT JOIN 
            sql += " FROM cStation A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //sql += "   AND ST_STATUS = '1' ";

            //查詢條件 
            if (!string.IsNullOrWhiteSpace(qc.ST_CODE))
            {
                sql += " and A.ST_CODE = @ST_CODE ";
            }
            //查詢條件 場站名稱
            if (!string.IsNullOrWhiteSpace(qc.ST_NAME))
            {
                sql += " and A.ST_NAME like '%' + @ST_NAME + '%' ";
            }
            //查詢條件 狀態
            if (!string.IsNullOrWhiteSpace(qc.ST_STATUS))
            {
                sql += " and A.ST_STATUS = @ST_STATUS ";
            }

            return PageList<MgmtStation_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

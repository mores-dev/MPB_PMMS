
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtCompany_QueryDAL : DALBase
    {
        public MgmtCompany_QueryDAL() { }

        public MgmtCompany_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<MgmtCompany_QueryResult> GetPageList(MgmtCompany_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID";
            //航商代碼
            sql += " , A.C_CODE";
            //航商名稱
            sql += " , A.C_NAME";
            //航商統編
            sql += " , A.C_TAX_ID";
            //備註
            sql += " , A.C_MEMO";
            //是否啟用  
            sql += ",  case when A.C_STATUS= '1' then '啟用' else '' end ";
            sql += " AS C_STATUS ";
            //FROM AND LEFT JOIN 
            sql += " FROM cCompany A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   AND C_ID NOT IN ('C00000') ";
            //sql += "   AND C_STATUS = '1' ";

            //查詢條件 
            if (!string.IsNullOrWhiteSpace(qc.C_CODE))
            {
                sql += " and A.C_CODE = @C_CODE ";
            }
            //查詢條件 航商名稱
            if (!string.IsNullOrWhiteSpace(qc.C_NAME))
            {
                sql += " and A.C_NAME like '%' + @C_NAME + '%' ";
            }
            //查詢條件 狀態
            if (!string.IsNullOrWhiteSpace(qc.C_STATUS))
            {
                sql += " and A.C_STATUS = @C_STATUS ";
            }

            return PageList<MgmtCompany_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

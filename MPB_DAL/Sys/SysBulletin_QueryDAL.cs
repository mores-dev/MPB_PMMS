
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Sys
{
    public class SysBulletin_QueryDAL : DALBase
    {
        public SysBulletin_QueryDAL() { }

        public SysBulletin_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<SysBulletin_QueryResult> GetPageList(SysBulletin_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //公告序號
            sql += " A.SB_ID";
            //公告日期
            sql += " , CONVERT(VARCHAR, A.SB_DATE, 111) AS SB_DATE";
            //公告標題
            sql += " , CASE WHEN LEN(A.SB_TITLE) > 30 THEN LEFT(A.SB_TITLE, 30) + '......' ELSE A.SB_TITLE END AS SB_TITLE";
            //公告內容
            sql += " , CASE WHEN LEN(A.SB_CONTENT) > 20 THEN LEFT(A.SB_CONTENT, 20) + '......' ELSE A.SB_CONTENT END AS SB_CONTENT";
            //公告類別  
            sql += " , C.C_NAME AS SB_TYPE";

            sql += " , CASE WHEN A.SB_TYPE LIKE '%' + @C_ID + '%' THEN 'N' ELSE 'R' END as BM ";

            //FROM AND LEFT JOIN 
            sql += " FROM cSysBulletin A ";
            sql += " LEFT JOIN cCompany C ON C.C_ID = A.SB_TYPE";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //sql += "   AND SB_STATUS = '1' ";
            //查詢條件 
            if (!string.IsNullOrWhiteSpace(qc.SB_DATE_START))
            {
                sql += " and A.SB_DATE >= @SB_DATE_START ";
            }
            if (!string.IsNullOrWhiteSpace(qc.SB_DATE_END))
            {
                sql += " and A.SB_DATE <= @SB_DATE_END ";
            }
            if (!string.IsNullOrWhiteSpace(qc.SB_DATE))
            {
                sql += " and A.SB_DATE = @SB_DATE ";
            }
            //查詢條件 標題
            if (!string.IsNullOrWhiteSpace(qc.SB_TITLE))
            {
                sql += " and A.SB_TITLE like '%' + @SB_TITLE + '%' ";
            }
            //查詢條件 公告類別
            if (!string.IsNullOrWhiteSpace(qc.SB_TYPE))
            {
                sql += " and A.SB_TYPE = @SB_TYPE ";
            }
            sql += " ORDER BY A.SB_ID DESC ";

            return PageList<SysBulletin_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

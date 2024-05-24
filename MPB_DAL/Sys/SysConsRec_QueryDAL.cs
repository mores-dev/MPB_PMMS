
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Sys
{
    public class SysConsRec_QueryDAL : DALBase
    {
        public SysConsRec_QueryDAL() { }

        public SysConsRec_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<SysConsRec_QueryResult> GetPageList(SysConsRec_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //序號
            sql += " A.CR_ID";
            //諮詢日期
            sql += " , CONVERT(VARCHAR, A.CR_DATE, 111) AS CR_DATE";
            //主旨
            sql += " , CASE WHEN LEN(A.CR_TITLE) > 30 THEN LEFT(A.CR_TITLE, 30) + '......' ELSE A.CR_TITLE END AS CR_TITLE";
            //聯絡方式
            sql += " , A.PHONE";
            //內容
            sql += " , CASE WHEN LEN(A.CR_CONTENT) > 20 THEN LEFT(A.CR_CONTENT, 20) + '......' ELSE A.CR_CONTENT END AS CR_CONTENT";
            //姓名
            sql += " , A.NAME";
            //狀態
            sql += " , C.CodeDesc AS CR_STATUS ";
            //FROM AND LEFT JOIN 
            sql += " FROM cConsRec A ";
            sql += " LEFT JOIN cCodeList C ON C.CodeKey = 'SYS01004' AND C.CodeID = A.CR_STATUS ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //sql += "   AND CR_STATUS = '1' ";
            //查詢條件 諮詢日期-起
            if (!string.IsNullOrWhiteSpace(qc.CR_DATE_START))
            {
                sql += " and A.CR_DATE >= @CR_DATE_START ";
            }
            //查詢條件 諮詢日期-迄
            if (!string.IsNullOrWhiteSpace(qc.CR_DATE_END))
            {
                sql += " and A.CR_DATE <= @CR_DATE_END ";
            }
            //查詢條件 諮詢日期
            if (!string.IsNullOrWhiteSpace(qc.CR_DATE))
            {
                sql += " and A.CR_DATE = @CR_DATE ";
            }
            //查詢條件 狀態
            if (!string.IsNullOrWhiteSpace(qc.CR_STATUS))
            {
                sql += " and A.CR_STATUS = @CR_STATUS ";
            }
            sql += " ORDER BY A.CR_ID DESC";

            return PageList<SysConsRec_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

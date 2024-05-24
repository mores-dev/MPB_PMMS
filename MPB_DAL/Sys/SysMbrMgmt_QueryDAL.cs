
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Sys
{
    public class SysMbrMgmt_QueryDAL : DALBase
    {
        public SysMbrMgmt_QueryDAL() { }

        public SysMbrMgmt_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<SysMbrMgmt_QueryResult> GetPageList(SysMbrMgmt_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //類型
            sql += " C1.CodeDesc AS GA_TYPE";
            //帳號
            sql += " , A.ACCOUNT AS GA_ACCOUNT";
            //業者名稱
            sql += " , A.NAME AS GA_NAME";
            //聯絡人
            sql += " , A.CONTACT AS CONTACT";
            //連絡電話
            sql += " , A.PHONE AS PHONE";
            //狀態
            sql += " , CASE WHEN A.AccStatus = 'Y' THEN '啟用' ELSE '' END AS GA_STATUS";

            sql += " , CASE WHEN A.C_ID LIKE '%' + ISNULL(@C_ID, '') + '%' THEN 'N' ELSE 'R' END as BM ";

            //FROM AND LEFT JOIN 
            sql += " FROM cGAAccount A ";
            //sql += " LEFT JOIN cCodeList C ON C.CodeKey = 'SYS01001' AND C.CodeID = A.AccStatus ";
            sql += " LEFT JOIN cCodeList C1 ON C1.CodeKey = 'SYS01006' AND C1.CodeID = A.GA_TYPE ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   AND A.ACCOUNT NOT IN ('admin')";
            //查詢條件 帳號
            if (!string.IsNullOrWhiteSpace(qc.GaAAA))
            {
                sql += " and A.ACCOUNT LIKE '%' + @GaAAA + '%' ";
            }
            //查詢條件 類型
            if (!string.IsNullOrWhiteSpace(qc.GaType))
            {
                sql += " and A.GA_TYPE = @GaType ";
            }
            //查詢條件 狀態
            if (!string.IsNullOrWhiteSpace(qc.GA_STATUS))
            {
                sql += " and A.AccStatus = @GA_STATUS ";
            }
            //查詢條件 業者名稱
            if (!string.IsNullOrWhiteSpace(qc.GaName))
            {
                sql += " and A.NAME LIKE '%' + @GaName + '%' ";
            }
            //查詢條件 聯絡人
            if (!string.IsNullOrWhiteSpace(qc.Contact))
            {
                sql += " and A.CONTACT LIKE '%' + @Contact + '%' ";
            }

            if (!string.IsNullOrWhiteSpace(qc.C_ID))
                sql += " And A.C_ID LIKE '%' + @C_ID + '%'";

            sql += " ORDER BY A.ACCOUNT ";

            return PageList<SysMbrMgmt_QueryResult>(qc.ToPage, @sql, qc);
        }
    }
}

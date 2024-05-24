
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Auth
{
    public class AuthRecord_QueryDAL : DALBase
    {
        public AuthRecord_QueryDAL() { }

        public AuthRecord_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<AuthRecord_QueryResult> GetPageList(AuthRecord_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";
            //操作時間
            sql += " Convert(Varchar(19), A.LogDT, 121) as LogDT";
            //系統別
            //sql += " , CASE WHEN A.LogSys = 'C' THEN '中央' ";
            //sql += "   WHEN A.LogSys = 'O' THEN '營運'";
            //sql += "   END AS LogSys";
            //類型
            sql += " , case When A.LogType = 'I' THEN '登入'";
            sql += "   When A.LogType = 'X' Then '登入失敗'";
            sql += "   When A.LogType = 'O' Then '登出'";
            sql += "   When A.LogType = 'F' And ProgFunc = '' Then '查詢'";
            sql += "   When A.LogType = 'F' And ProgFunc = 'A' Then '新增'";
            sql += "   When A.LogType = 'F' And ProgFunc = 'M' Then '修改'";
            sql += " End as LogType ";

            //程式名稱
            sql += " , ISNULL(B.FuncName, '') as ProgName";
            //使用者
            sql += " , A.UserID + ' ' + ISNULL(C.NAME, '') as UserID";
            //IP
            sql += " , LoginIP as IP";
            //FROM AND LEFT JOIN 
            sql += " FROM LogRecord   A    "; //記錄檔
            sql += " Left join cFuncInfo B ON A.ProgID = B.FuncValue1 AND A.LogType = 'F' "; // 程式清單檔
            sql += " Left Join cAFCAccount C ON A.UserID = C.ACCOUNT";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += " and A.LogSys = 'C' ";

            //查詢條件 程式代號
            if (!string.IsNullOrWhiteSpace(qc.LogOn))
            {
                sql += " and A.LogDT > @LogOn ";
            }
            //查詢條件 程式名稱
            if (!string.IsNullOrWhiteSpace(qc.LogOff))
            {
                sql += " and A.LogDT < @LogOff + ' 23:59:59'";
            }
            //ORDER BY (排序欄位)
            sql += " ORDER BY A.LogDT desc";

            return PageList<AuthRecord_QueryResult>(qc.ToPage, @sql, 20, qc);
        }

    }
}

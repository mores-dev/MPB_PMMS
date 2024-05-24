
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.FCM
{
    public class FCMVessel_QueryDAL : DALBase
    {
        public FCMVessel_QueryDAL() { }

        public FCMVessel_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<FCMVessel_QueryResult> GetPageList(FCMVessel_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID";
            //航商名稱
            sql += " , D.C_NAME";
            //船隻序號
            sql += " , A.V_ID";
            //船隻代碼
            sql += " , A.V_CODE";
            //船隻名稱
            sql += " , A.V_NAME";
            //乘客限額
            sql += " , MAXIMUM";
            //是否啟用  
            sql += ",  case when A.V_STATUS= '1' then '啟用' else '' end ";
            sql += " AS V_STATUS ";
            //FROM AND LEFT JOIN 
            sql += " FROM cVessel A ";
            sql += " LEFT JOIN cCompany D ON D.C_ID = A.C_ID ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //sql += "   AND V_STATUS = '1' ";
            //查詢條件 航商序號
            if (!string.IsNullOrWhiteSpace(qc.C_ID))
            {
                sql += "   AND A.C_ID = @C_ID ";
            }
            //查詢條件 
            if (!string.IsNullOrWhiteSpace(qc.V_CODE))
            {
                sql += " and A.V_CODE like '%' +  @V_CODE + '%'";
            }
            //查詢條件 船隻名稱
            if (!string.IsNullOrWhiteSpace(qc.V_NAME))
            {
                sql += " and A.V_NAME like '%' + @V_NAME + '%' ";
            }
            //查詢條件 狀態
            if (!string.IsNullOrWhiteSpace(qc.V_STATUS))
            {
                sql += " and A.V_STATUS = @V_STATUS ";
            }

            return PageList<FCMVessel_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

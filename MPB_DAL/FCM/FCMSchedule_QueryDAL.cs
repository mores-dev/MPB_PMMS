
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.FCM
{
    public class FCMSchedule_QueryDAL : DALBase
    {
        public FCMSchedule_QueryDAL() { }

        public FCMSchedule_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<FCMSchedule_QueryResult> GetPageList(FCMSchedule_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID";
            //航商名稱
            sql += " , D.C_NAME";
            //航班序號
            sql += " , A.SC_ID";
            //航班代碼
            sql += " , A.SC_CODE";
            //航班名稱
            sql += " , A.SC_NAME";
            //航班時間
            sql += " , ISNULL(C.CodeDesc, A.SC_TIME) SC_TIME";
            //套用航線
            sql += " , B.R_NAME";
            //預估航行時間(分鐘)
            sql += " , A.TRAVEL_TIME";
            //是否啟用  
            sql += ",  case when A.SC_STATUS= '1' then '啟用' else '' end ";
            sql += " AS SC_STATUS ";
            //FROM AND LEFT JOIN 
            sql += " FROM cSchedule A ";
            sql += " LEFT JOIN cRoute B ON B.R_ID = A.R_ID ";
            sql += " LEFT JOIN cCodeList C ON A.SC_TIME = C.CodeID AND C.CodeKey = 'SYS01003' ";
            sql += " LEFT JOIN cCompany D ON D.C_ID = A.C_ID ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //sql += "   AND SC_STATUS = '1' ";
            //查詢條件 航商序號
            if (!string.IsNullOrWhiteSpace(qc.C_ID))
            {
                sql += "   AND A.C_ID = @C_ID ";
            }
            //查詢條件 
            if (!string.IsNullOrWhiteSpace(qc.SC_CODE))
            {
                sql += " and A.SC_CODE like '%' +  @SC_CODE + '%'";
            }
            //查詢條件 航班名稱
            if (!string.IsNullOrWhiteSpace(qc.SC_NAME))
            {
                sql += " and A.SC_NAME like '%' + @SC_NAME + '%' ";
            }
            //查詢條件 狀態
            if (!string.IsNullOrWhiteSpace(qc.SC_STATUS))
            {
                sql += " and A.SC_STATUS = @SC_STATUS ";
            }
            //查詢條件 預估航行時間
            if (!string.IsNullOrWhiteSpace(qc.TravelTime))
            {
                sql += " and A.TRAVEL_TIME = @TravelTime ";
            }

            return PageList<FCMSchedule_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

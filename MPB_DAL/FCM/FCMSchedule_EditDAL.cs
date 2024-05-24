
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.FCM
{
    public class FCMSchedule_EditDAL : DALBase
    {
        public FCMSchedule_EditDAL() { }

        public FCMSchedule_EditDAL(DbManager db) : base(db) { }

        public FCMSchedule_EditMain Select_cSchedule(FCMSchedule_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " C.C_ID";
            //航商名稱
            sql += " , C.C_NAME";
            //航班序號
            sql += " , A.SC_ID";
            //航班代碼
            sql += " , A.SC_CODE";
            //航班名稱
            sql += " , A.SC_NAME";
            //航班時間
            sql += " , A.SC_TIME";
            //套用航線
            sql += " , A.R_ID";
            //預估航行時間
            sql += " , A.TRAVEL_TIME";
            //是否啟用  
            sql += ",  A.SC_STATUS";
            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cSchedule A ";
            sql += " LEFT JOIN cCompany C ON C.C_ID = A.C_ID";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 航商序號
            sql += " and A.C_ID = @C_ID ";
            //查詢條件 航班序號
            sql += " and A.SC_ID = @SC_ID ";

            return SingleOrDefault<FCMSchedule_EditMain>(@sql, qc);
        }

        /// <summary>
        /// 取得套用航線
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<CodeName> Select_cRoute()
        {
            string sql = "";
            sql += "SELECT R_ID AS code, R_NAME AS name ";
            sql += " From cRoute ";
            sql += " WHERE 1 = 1  ";
            sql += " and R_STATUS = '1' ";
            return Fetch<CodeName>(@sql);
        }

        /// <summary>
        /// cFCMSchedule 設備資料檔 KEY值檢查
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(FCMSchedule_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cSchedule ";
            sql += " WHERE 1 = 1  ";
            sql += " and C_ID = @C_ID ";
            sql += " and SC_CODE = @SC_CODE ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}

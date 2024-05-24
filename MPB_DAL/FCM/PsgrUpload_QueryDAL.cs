
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;
using System;

namespace MPB_DAL.FCM
{
    public class PsgrUpload_QueryDAL : DALBase
    {
        public PsgrUpload_QueryDAL() { }

        public PsgrUpload_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<PsgrUpload_QueryResult> GetPageList(PsgrUpload_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID";
            //航商名稱
            sql += " , A.C_NAME";
            //航站
            sql += " , A.STATION";
            //航班時間
            sql += " , A.VOYAGE_TIME";
            //船舶代號
            sql += " , A.VESSEL_ID";
            //航班名稱
            sql += " , A.VOYAGE_TIME + ' ' + A.R_NAME AS R_NAME";
            //船隻名稱
            sql += " , A.VESSEL_NAME";
            //登錄人數
            sql += " , A.PASSENGER_CNT";
            //上傳狀態
            sql += " , CASE WHEN A.UPLOAD_STS = 'N' THEN '未上傳' ELSE '' END";
            sql += "   AS UPLOAD_STS";


            //FROM AND LEFT JOIN 
            sql += " FROM cManifest A ";
            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "  AND A.SHIPPING_DATE = CONVERT(DATE, GETDATE())";
            
            if (!string.IsNullOrWhiteSpace(qc.C_ID))
                sql += "  AND A.C_ID = @C_ID ";

            sql += " Order by A.C_ID, A.VOYAGE_TIME";

            return PageList<PsgrUpload_QueryResult>(qc.ToPage, @sql, 100, qc);
        }

    }
}

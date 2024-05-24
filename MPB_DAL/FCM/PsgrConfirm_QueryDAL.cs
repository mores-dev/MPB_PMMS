
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;
using System;

namespace MPB_DAL.FCM
{
    public class PsgrConfirm_QueryDAL : DALBase
    {
        public PsgrConfirm_QueryDAL() { }

        public PsgrConfirm_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<PsgrConfirm_QueryResult> GetPageList(PsgrConfirm_QueryCondition qc)
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
            sql += " , A.R_NAME";
            //船隻名稱
            sql += " , A.VESSEL_NAME";
            //乘客姓名
            sql += " , B.PSGR_NAME";
            //證件別
            sql += " , ISNULL(C.CodeDesc, '') as ID_TYPE";
            //證件號碼
            sql += " , B.ID_NO";
            //出生日期  
            sql += " , Convert(Varchar, B.BIRTH, 111) as BIRTH ";
            //FROM AND LEFT JOIN 
            sql += " FROM cManifest A ";
            sql += " Join cManifestDtl B ON A.SHIPPING_DATE = B.SHIPPING_DATE";
            sql += "  And A.STATION = B.STATION AND A.VOYAGE_TIME = B.VOYAGE_TIME AND A.VESSEL_ID = B.VESSEL_ID";

            sql += " Left Join cCodeList C ON C.CodeKey = 'SYS01005' AND B.ID_TYPE = C.CodeID";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "  AND A.SHIPPING_DATE = CONVERT(DATE, GETDATE())";
            sql += "  And A.STATION = @Station";
            sql += "  And A.VOYAGE_TIME = @VoyageTime";
            sql += "  ANd A.VESSEL_ID = @VesselId";
            
            if (!string.IsNullOrWhiteSpace(qc.C_ID))
                sql += "  AND A.C_ID = @C_ID ";

            sql += " Order by B.BOARDING_DT";

            return PageList<PsgrConfirm_QueryResult>(qc.ToPage, @sql, 100, qc);
        }

        public List<CodeName> GetQueryStr(PsgrConfirm_QueryCondition qc)
        {
            string sql = "";

            sql += " Select";
            sql += "  C_ID + '|' + STATION + '|' + VOYAGE_TIME + '|' + VESSEL_ID as Code";
            sql += "  , VOYAGE_TIME + ' ' +  VESSEL_NAME +  ' ' + R_NAME as Name";
            sql += "   From cManifest A";
            sql += "  Where 1=1";
            sql += "  AND A.SHIPPING_DATE = CONVERT(DATE, GETDATE())";
            if (!string.IsNullOrWhiteSpace(qc.C_ID))
                sql += "  AND A.C_ID = @C_ID ";

            sql += " Order by A.C_ID, A.STATION, A.VOYAGE_TIME";

            return Fetch<CodeName>(@sql, qc);
        }
    }
}

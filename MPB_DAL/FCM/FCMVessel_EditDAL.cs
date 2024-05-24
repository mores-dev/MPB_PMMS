
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.FCM
{
    public class FCMVessel_EditDAL : DALBase
    {
        public FCMVessel_EditDAL() { }

        public FCMVessel_EditDAL(DbManager db) : base(db) { }

        public FCMVessel_EditMain Select_cVessel(FCMVessel_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " C.C_ID";
            //航商名稱
            sql += " , C.C_NAME";
            //船隻序號
            sql += " , A.V_ID";
            //船舶號數
            sql += " , A.V_CODE";
            //船名
            sql += " , A.V_NAME";
            //船名
            sql += " , A.API_NAME";
            //船名_英文
            sql += " , A.V_NAME_EN";
            //乘客限額
            sql += " , A.MAXIMUM";
            //所屬公司名稱  
            sql += ",  A.BelongCompany";
            //所屬公司統編
            sql += ",  A.BelongTAX_ID";
            //是否啟用  
            sql += ",  A.V_STATUS";
            //AIS船舶編號
            sql += ",  A.MMSI";
            //國際船舶編號
            sql += ",  A.IMO";
            //船呼
            sql += ",  A.CallSign";
            //船舶種類
            sql += ",  A.VesselType";
            //船舶總噸位(噸)
            sql += ",  A.GrossTonnage";
            //船長(公尺)
            sql += ",  A.VesselLength";
            //船寬(公尺)
            sql += ",  A.VesselWidth";
            //吃水深度(公尺)
            sql += ",  A.LoadDraft";
            //備註
            sql += ",  A.MEMO";

            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cVessel A ";
            sql += " LEFT JOIN cCompany C ON C.C_ID = A.C_ID";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 航商序號
            sql += " and A.C_ID = @C_ID ";
            //查詢條件 船隻序號
            sql += " and A.V_ID = @V_ID ";

            return SingleOrDefault<FCMVessel_EditMain>(@sql, qc);
        }

        /// <summary>
        /// cFCMVessel 設備資料檔 KEY值檢查
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(FCMVessel_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cVessel ";
            sql += " WHERE 1 = 1  ";
            sql += " and C_ID = @C_ID ";
            sql += " and V_CODE = @V_CODE ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}


using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtCompany_EditDAL : DALBase
    {
        public MgmtCompany_EditDAL() { }

        public MgmtCompany_EditDAL(DbManager db) : base(db) { }

        public MgmtCompany_EditMain Select_cCompany(MgmtCompany_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID";
            //航商代碼
            sql += " , A.C_CODE";
            //航商名稱
            sql += " , A.C_NAME";
            //航商名稱_英文
            sql += " , A.C_NAME_EN";
            //航商名稱
            sql += " , A.C_TAX_ID";
            //備註
            sql += " , A.C_MEMO";
            //是否啟用  
            sql += ",  A.C_STATUS";
            //聯絡電話  
            sql += ",  A.PHONE";
            //地址  
            sql += ",  A.C_ADDRESS";
            //EMAIL  
            sql += ",  A.EMAIL";
            //訂票電話  
            sql += ",  A.ODR_PHONE";
            //官網網址  
            sql += ",  A.LAND_URL";
            //票價網址  
            sql += ",  A.PRICE_URL";
            //訂票網址  
            sql += ",  A.ODR_URL";
            //LOGO網址  
            sql += ",  A.LOGO_URL";
            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cCompany A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 設備代號
            sql += " and A.C_ID = @C_ID ";

            return SingleOrDefault<MgmtCompany_EditMain>(@sql, qc);
        }

        /// <summary>
        /// cMgmtCompany 設備資料檔 KEY值檢查
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(MgmtCompany_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cCompany ";
            sql += " WHERE 1 = 1  ";
            sql += " and C_CODE = @C_CODE ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}

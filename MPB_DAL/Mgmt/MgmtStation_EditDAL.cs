
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtStation_EditDAL : DALBase
    {
        public MgmtStation_EditDAL() { }

        public MgmtStation_EditDAL(DbManager db) : base(db) { }

        public MgmtStation_EditMain Select_cStation(MgmtStation_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //場站序號
            sql += " A.ST_ID";
            //場站代碼
            sql += " , A.ST_CODE";
            //場站名稱
            sql += " , A.ST_NAME";
            //備註
            sql += " , A.ST_MEMO";
            //是否啟用  
            sql += ",  A.ST_STATUS";
            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cStation A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 設備代號
            sql += " and A.ST_ID = @ST_ID ";

            return SingleOrDefault<MgmtStation_EditMain>(@sql, qc);
        }

        /// <summary>
        /// cMgmtStation 設備資料檔 KEY值檢查
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(MgmtStation_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cStation ";
            sql += " WHERE 1 = 1  ";
            sql += " and ST_CODE = @ST_CODE ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}

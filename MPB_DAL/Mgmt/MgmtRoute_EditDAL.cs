
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtRoute_EditDAL : DALBase
    {
        public MgmtRoute_EditDAL() { }

        public MgmtRoute_EditDAL(DbManager db) : base(db) { }

        public MgmtRoute_EditMain Select_cRoute(MgmtRoute_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航線序號
            sql += " A.R_ID";
            //航線代碼
            sql += " , A.R_CODE";
            //航線名稱
            sql += " , A.R_NAME";
            //備註
            sql += " , A.R_MEMO";
            //是否啟用  
            sql += ",  A.R_STATUS";
            //起站  
            sql += ",  A.ST_ID_START";
            //末站  
            sql += ",  A.ST_ID_END";
            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cRoute A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 航線序號
            sql += " and A.R_ID = @R_ID ";

            return SingleOrDefault<MgmtRoute_EditMain>(@sql, qc);
        }

        public List<MgmtRoute_EditDetailGrid> Select_cRouteDtl(MgmtRoute_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航線序號
            sql += " A.R_ID";
            //場站序號
            sql += " , A.ST_ID";
            //排序
            sql += " , A.ST_ODR";

            //FROM AND LEFT JOIN 
            sql += " FROM cRouteDtl A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 航線序號
            sql += " and A.R_ID = @R_ID ";

            sql += " Order By A.ST_ODR";

            return Fetch<MgmtRoute_EditDetailGrid>(@sql, qc);
        }

        public List<CodeName> Select_cStation()
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT A.ST_ID as code, A.ST_NAME as name";
            sql += " FROM cStation A WHERE A.ST_STATUS = '1'";

            return Fetch<CodeName>(@sql);
        }

        /// <summary>
        /// cMgmtRoute 設備資料檔 KEY值檢查
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(MgmtRoute_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cRoute ";
            sql += " WHERE 1 = 1  ";
            sql += " and R_CODE = @R_CODE ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}

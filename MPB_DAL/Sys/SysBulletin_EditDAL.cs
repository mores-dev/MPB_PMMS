
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Sys
{
    public class SysBulletin_EditDAL : DALBase
    {
        public SysBulletin_EditDAL() { }

        public SysBulletin_EditDAL(DbManager db) : base(db) { }

        public SysBulletin_EditMain Select_cSysBulletin(SysBulletin_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //公告序號
            sql += " A.SB_ID";
            //公告日期
            sql += " , CONVERT(VARCHAR, A.SB_DATE, 111) AS SB_DATE";
            //公告標題
            sql += " , A.SB_TITLE";
            //公告內容
            sql += " , A.SB_CONTENT";
            //公告單位  
            sql += " , A.SB_TYPE ";
            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cSysBulletin A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 公告序號
            sql += " and A.SB_ID = @SB_ID ";

            return SingleOrDefault<SysBulletin_EditMain>(@sql, qc);
        }
    }
}

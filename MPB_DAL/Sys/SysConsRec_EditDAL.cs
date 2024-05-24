
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Sys
{
    public class SysConsRec_EditDAL : DALBase
    {
        public SysConsRec_EditDAL() { }

        public SysConsRec_EditDAL(DbManager db) : base(db) { }

        public SysConsRec_EditMain Select_cConsRec(SysConsRec_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //序號
            sql += " A.CR_ID";
            //諮詢日期
            sql += " , CONVERT(VARCHAR, A.CR_DATE, 111) AS CR_DATE";
            //主旨
            sql += " , A.CR_TITLE";
            //內容
            sql += " , A.CR_CONTENT";
            //狀態
            sql += " , A.CR_STATUS ";
            //回覆日期
            sql += " , CONVERT(VARCHAR, A.CR_RESP_DATE, 111) AS CR_RESP_DATE";
            //回覆內容
            sql += " , A.CR_RESP_CONTENT";
            //姓名
            sql += " , A.NAME";
            //電話
            sql += " , A.PHONE";
            //EMAIL
            sql += " , A.EMAIL";
            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cConsRec A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 序號
            sql += " and A.CR_ID = @CR_ID ";

            return SingleOrDefault<SysConsRec_EditMain>(@sql, qc);
        }

    }
}

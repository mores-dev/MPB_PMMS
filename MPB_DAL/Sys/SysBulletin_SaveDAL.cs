
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Sys
{
    public class SysBulletin_SaveDAL : DALBase
    {
        public SysBulletin_SaveDAL() { }

        public SysBulletin_SaveDAL(DbManager db) : base(db) { }

        public int GetMaxSB_ID()
        {
            string sql = "";

            sql += " SELECT RIGHT(ISNULL(MAX(SB_ID), '0'), 4) FROM cSysBulletin WHERE SB_ID > CONVERT(VARCHAR, GETDATE(), 12)";

            return SingleOrDefault<int>(sql);
        }

        public int Insert_cSysBulletin(SysBulletin_SaveMain sm)
        {
            string sql = "";

            sql = "Insert into cSysBulletin (";
            //公告序號
            sql += "SB_ID";
            //公告日期
            sql += ", SB_DATE";
            //公告單位
            sql += ", SB_TYPE";
            //公告標題
            sql += ", SB_TITLE";
            //公告內容
            sql += ", SB_CONTENT";

            //reserve columns
            sql += "\n" + ", SYNCTIME";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //公告序號
            sql += "  @SB_ID";
            //公告日期
            sql += ", @SB_DATE";
            //公告單位
            sql += ", @SB_TYPE";
            //公告標題
            sql += ", @SB_TITLE";
            //公告內容
            sql += ", @SB_CONTENT";

            //reserve columns
            sql += "\n" + ", ''";
            //資料建立紀錄
            sql += ", SysDateTime(), @CreateId, SysDateTime(), @CreateId ";

            return Execute(@sql, sm);
        }

        public int Update_cSysBulletin(SysBulletin_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cSysBulletin ";
            sql += " SET ";

            //公告標題
            sql += " SB_TITLE = @SB_TITLE";
            //公告內容
            sql += " , SB_CONTENT = @SB_CONTENT";
            //公告日期
            sql += " , SB_DATE = @SB_DATE";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 公告序號 
            sql += " AND SB_ID = @SB_ID";

            return Execute(@sql, sm);
        }
    }
}

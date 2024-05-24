
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtRoute_SaveDAL : DALBase
    {
        public MgmtRoute_SaveDAL() { }

        public MgmtRoute_SaveDAL(DbManager db) : base(db) { }

        public int GetMaxR_ID()
        {
            string sql = "";

            sql += " SELECT RIGHT(ISNULL(MAX(R_ID), '0'), 5) FROM cRoute";

            return SingleOrDefault<int>(sql);
        }

        public int Insert_cRoute(MgmtRoute_SaveMain sm)
        {
            string sql = "";

            sql = "Insert into cRoute (";
            //航線序號
            sql += "R_ID";
            //航線代號
            sql += ", R_CODE";
            //航線名稱
            sql += ", R_NAME";
            //起站
            sql += ", ST_ID_START";
            //末站
            sql += ", ST_ID_END";
            //狀態
            sql += ", R_STATUS";
            //備註
            sql += ", R_MEMO";

            //reserve columns
            sql += "\n" + ", SYNCTIME";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //航線序號
            sql += "  @R_ID";
            //航線代號
            sql += ", @R_CODE";
            //航線名稱
            sql += ", @R_NAME";
            //起站
            sql += ", @ST_ID_START";
            //末站
            sql += ", @ST_ID_END";
            //狀態
            sql += ", @R_STATUS";
            //備註
            sql += ", ''";

            //reserve columns
            sql += "\n" + ", ''";
            //資料建立紀錄
            sql += ", SysDateTime(), @CreateId, SysDateTime(), @CreateId ";

            return Execute(@sql, sm);
        }

        public int Insert_cRouteDtl(MgmtRoute_SaveDetailGrid sm)
        {
            string sql = "";

            sql = "Insert into cRouteDtl (";
            //航線序號
            sql += "R_ID";
            //場站序號
            sql += ", ST_ID";
            //排序
            sql += ", ST_ODR)";

            sql += "\n" + " Select ";
            //航線序號
            sql += "  @R_ID";
            //場站序號
            sql += ", @ST_ID";
            //排序
            sql += ", @ST_ODR";

            return Execute(@sql, sm);
        }

        public int Update_cRoute(MgmtRoute_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cRoute ";
            sql += " SET ";

            //航線名稱
            sql += " R_NAME = @R_NAME";
            //航線代號
            sql += " , R_CODE = @R_CODE";
            //起站
            sql += " , ST_ID_START = @ST_ID_START";
            //末站
            sql += " , ST_ID_END = @ST_ID_END";
            //狀態
            sql += " , R_STATUS = @R_STATUS";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航線序號 
            sql += " AND R_ID = @R_ID";

            return Execute(@sql, sm);
        }

        public int Delete_cRouteDtl(MgmtRoute_SaveMain sm)
        {
            string sql = "";
            sql += "DELETE cRouteDtl ";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航線序號 
            sql += " AND R_ID = @R_ID";

            return Execute(@sql, sm);
        }

        public int Delete_cRoute(MgmtRoute_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cRoute ";
            sql += " SET ";

            //狀態
            sql += " R_STATUS = '0',";

            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航線序號 
            sql += " AND R_ID = @R_ID";

            return Execute(@sql, sm);
        }

    }
}

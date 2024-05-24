
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtStation_SaveDAL : DALBase
    {
        public MgmtStation_SaveDAL() { }

        public MgmtStation_SaveDAL(DbManager db) : base(db) { }

        public int GetMaxST_ID()
        {
            string sql = "";

            sql += " SELECT RIGHT(ISNULL(MAX(ST_ID), '0'), 5) FROM cStation";

            return SingleOrDefault<int>(sql);
        }

        public int Insert_cStation(MgmtStation_SaveMain sm)
        {
            string sql = "";

            sql = "Insert into cStation (";
            //場站序號
            sql += "ST_ID";
            //場站代號
            sql += ", ST_CODE";
            //場站名稱
            sql += ", ST_NAME";
            //狀態
            sql += ", ST_STATUS";
            //備註
            sql += ", ST_MEMO";

            //reserve columns
            sql += "\n" + ", SYNCTIME";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //場站序號
            sql += "  @ST_ID";
            //場站代號
            sql += ", @ST_CODE";
            //場站名稱
            sql += ", @ST_NAME";
            //狀態
            sql += ", @ST_STATUS";
            //備註
            sql += ", @ST_MEMO";

            //reserve columns
            sql += "\n" + ", ''";
            //資料建立紀錄
            sql += ", SysDateTime(), @CreateId, SysDateTime(), @CreateId ";


            return Execute(@sql, sm);
        }

        public int Update_cStation(MgmtStation_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cStation ";
            sql += " SET ";

            //場站名稱
            sql += " ST_NAME = @ST_NAME";
            //場站代號
            sql += " , ST_CODE = @ST_CODE";
            //狀態
            sql += " , ST_STATUS = @ST_STATUS";
            //備註
            sql += " , ST_MEMO = @ST_MEMO";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 場站序號 
            sql += " AND ST_ID = @ST_ID";

            return Execute(@sql, sm);
        }

        public int Delete_cStation(MgmtStation_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cStation ";
            sql += " SET ";

            //狀態
            sql += " ST_STATUS = '0',";

            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 場站序號 
            sql += " AND ST_ID = @ST_ID";

            return Execute(@sql, sm);
        }

    }
}

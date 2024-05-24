
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.FCM
{
    public class FCMSchedule_SaveDAL : DALBase
    {
        public FCMSchedule_SaveDAL() { }

        public FCMSchedule_SaveDAL(DbManager db) : base(db) { }

        public int GetMaxSC_ID()
        {
            string sql = "";

            sql += " SELECT RIGHT(ISNULL(MAX(SC_ID), '0'), 5) FROM cSchedule";

            return SingleOrDefault<int>(sql);
        }

        public int Insert_cSchedule(FCMSchedule_SaveMain sm)
        {
            string sql = "";

            sql = "Insert into cSchedule (";
            //航商序號
            sql += "C_ID";
            //航班序號
            sql += ", SC_ID";
            //航班代號
            sql += ", SC_CODE";
            //航班名稱
            sql += ", SC_NAME";
            //狀態
            sql += ", SC_STATUS";
            //航班時間
            sql += ", SC_TIME";
            //套用航線
            sql += ", R_ID";
            //預估航行時間
            sql += ", TRAVEL_TIME";

            //reserve columns
            sql += "\n" + ", SYNCTIME";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //航商序號
            sql += "  @C_ID";
            //航班序號
            sql += ", @SC_ID";
            //航班代號
            sql += ", @SC_CODE";
            //航班名稱
            sql += ", @SC_NAME";
            //狀態
            sql += ", @SC_STATUS";
            //航班時間
            sql += ", @SC_TIME";
            //套用航線
            sql += ", @R_ID";
            //預估航行時間
            sql += ", @TravelTime";

            //reserve columns
            sql += "\n" + ", ''";
            //資料建立紀錄
            sql += ", SysDateTime(), @CreateId, SysDateTime(), @CreateId ";

            return Execute(@sql, sm);
        }

        public int Update_cSchedule(FCMSchedule_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cSchedule ";
            sql += " SET ";

            //航班名稱
            sql += " SC_NAME = @SC_NAME";
            //航班代號
            sql += " , SC_CODE = @SC_CODE";
            //狀態
            sql += " , SC_STATUS = @SC_STATUS";
            //航班時間
            sql += " , SC_TIME = @SC_TIME";
            //套用航線
            sql += " , R_ID = @R_ID";
            //套用航線
            sql += " , TRAVEL_TIME = @TravelTime";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航商序號 
            sql += " AND C_ID = @C_ID";
            //條件 航班序號 
            sql += " AND SC_ID = @SC_ID";

            return Execute(@sql, sm);
        }

        public int Delete_cSchedule(FCMSchedule_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cSchedule ";
            sql += " SET ";

            //狀態
            sql += " SC_STATUS = '0',";

            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航商序號 
            sql += " AND C_ID = @C_ID";
            //條件 航班序號 
            sql += " AND SC_ID = @SC_ID";

            return Execute(@sql, sm);
        }

    }
}

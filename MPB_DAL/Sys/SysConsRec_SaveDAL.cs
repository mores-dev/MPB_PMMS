
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Sys
{
    public class SysConsRec_SaveDAL : DALBase
    {
        public SysConsRec_SaveDAL() { }

        public SysConsRec_SaveDAL(DbManager db) : base(db) { }

        public int Update_cConsRec(SysConsRec_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cConsRec ";
            sql += " SET ";

            //狀態
            sql += " CR_STATUS = @CR_STATUS";
            //回覆日期
            sql += " , CR_RESP_DATE = @CR_RESP_DATE";
            //回覆內容
            sql += " , CR_RESP_CONTENT = @CR_RESP_CONTENT";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 序號 
            sql += " AND CR_ID = @CR_ID";

            return Execute(@sql, sm);
        }

    }
}

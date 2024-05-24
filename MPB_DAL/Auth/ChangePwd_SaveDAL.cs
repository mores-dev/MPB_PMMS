
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System;
using System.Collections.Generic;

namespace MPB_DAL.Auth
{
    public class ChangePwd_SaveDAL : DALBase
    {
        public ChangePwd_SaveDAL() { }

        public ChangePwd_SaveDAL(DbManager db) : base(db) { }

        public int Update_UserPwd(ChangePwd_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cAFCAccount ";
            sql += " SET ";

            sql += "PwdChg2 = PwdChg1, ";
            sql += "PwdChg1 = PASSWORD, ";

            //使用者密碼
            sql += " PASSWORD = @Pd,";

            sql += " LastChanged = GetDate(), ";

            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";
            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 使用者代號 
            sql += " AND ACCOUNT = @UserId";

            return Execute(@sql, sm);
        }

        public ChangePwd_SaveMain Select_AFCAccount(ChangePwd_SaveMain sm)
        {
            string sql = "";
            sql += "select ACCOUNT as UserID, PASSWORD as Pwd ";
            sql += "    , PwdChg1 as Pwd1, PwdChg2 as Pwd2";

            sql += " from cAFCAccount ";
            sql += " where 1=1 ";
            sql += "   AND ACCOUNT = @UserId";

            return SingleOrDefault<ChangePwd_SaveMain>(sql, sm);
        }
    }
}

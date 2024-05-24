
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System;
using System.Collections.Generic;

namespace MPB_DAL.Auth
{
    public class AuthUser_SaveDAL : DALBase
    {
        public AuthUser_SaveDAL() { }

        public AuthUser_SaveDAL(DbManager db) : base(db) { }

        public int Insert_cAFCA(AuthUser_SaveMain userEdit)
        {
            string sql = "";

            sql = "Insert into cAFCAccount (";
            //使用者代號
            sql += "ACCOUNT";
            //設備類型
            sql += ", DeviceTypeId";
            //密碼
            sql += ", PASSWORD";
            //使用者名稱
            sql += ", NAME";
            //使用者類型
            sql += ", UserType";
            //單位
            sql += ", DEPT";
            //EMAIL
            sql += ", EMAIL";
            //狀態
            sql += ", AccStatus";
            //航商
            sql += ", C_ID";
            //資料建立紀錄
            sql += ", SYNCTIME, CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += " Select ";
            //使用者代號
            sql += "  @UserId";
            //設備類型
            sql += ", 'C'";
            //密碼
            sql += ", @Pd";
            //使用者名稱
            sql += ", @UserName";
            //使用者類型
            sql += ", @UserType";
            //單位
            sql += ", @DEPT";
            //EMAIL
            sql += ", ''";
            //狀態
            sql += ", @Status";
            //航商
            sql += ", @C_ID";
            //資料建立紀錄
            sql += ", '', SysDateTime(), @CreateId, SysDateTime(), @CreateId";

            return Execute(@sql, userEdit);
        }

        public int Insert_Grid1_AccountRole(AuthUserRole_SaveDetailGrid1 sd)
        {
            string sql = "";

            sql += "Insert Into cAccountRole (";
            //帳號
            sql += "ACCOUNT";
            //設備類型
            sql += ", DeviceTypeID";
            //群組代號
            sql += ", RoleID";
            sql += ") ";
            sql += " Select ";
            //帳號
            sql += " @UserId";
            //設備類型
            sql += ", 'C'";
            //群組代號
            sql += ", @RoleId";

            return Execute(@sql, sd);
        }

        public int Update_cAFCAccount(AuthUser_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cAFCAccount ";
            sql += " SET ";
            //使用者名稱
            sql += " NAME = @UserName,";

            if (!string.IsNullOrWhiteSpace(sm.Pd))
            {
                sql += " PASSWORD = @Pd, ";
                sql += " LastChanged = GetDate(), ";
            }
            sql += " PwdErr = 0, ";
            //部門ID
            sql += " DEPT = @DEPT,";
            //狀態
            sql += " AccStatus = @Status,";
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

        public int Delete_Grid1_AccountRole(AuthUser_SaveMain sm)
        {
            string sql = "";

            sql += "DELETE FROM cAccountRole";
            sql += " Where ACCOUNT = @UserId";

            return Execute(sql, sm);
        }

        public int Delete_AuthUser(AuthUser_SaveMain userEdit)
        {
            string sql = "";
            sql += "UPDATE cAFCAccount ";
            sql += " SET ";
            //啟用否
            sql += " AccStatus = 'N',";
            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";
            sql += " where 1 = 1 ";
            //條件 使用者代號 
            sql += " AND ACCOUNT = @UserId";

            return Execute(@sql, userEdit);
        }

        public int Update_AuthUserPwd(AuthUser_SaveMain sm)
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

        public AuthUser_SaveMain Select_AuthUser(AuthUser_SaveMain sm)
        {
            string sql = "";
            sql = "select ACCOUNT as UserID, PASSWORD as Pwd ";
            sql += "    , PwdChg1 as Pwd1, PwdChg2 as Pwd2";

            sql += " from cAFCAccount ";
            sql += " where 1 = 1 ";
            sql += "   AND ACCOUNT = @UserId";
            return SingleOrDefault<AuthUser_SaveMain>(sql, sm);
        }
    }
}

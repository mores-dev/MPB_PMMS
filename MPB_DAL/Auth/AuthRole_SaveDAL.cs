
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System;
using System.Collections.Generic;

namespace MPB_DAL.Auth
{
    public class AuthRole_SaveDAL : DALBase
    {
        public AuthRole_SaveDAL() { }

        public AuthRole_SaveDAL(DbManager db) : base(db) { }

        public int Insert_RoleInfo(AuthRole_SaveMain userEdit)
        {
            string sql = "";

            sql = "Insert into cRoleInfo (";
            //群組代號
            sql += "RoleID";
            //群組名稱
            sql += ", RoleName";
            //群組類型
            sql += ", RoleType";
            //狀態
            sql += ", RoleStatus";
            //資料建立紀錄
            sql += ", SYNCTIME, CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += " Select ";
            //群組代號
            sql += " @RoleId";
            //群組名稱
            sql += ", @RoleName";
            //群組類型
            sql += ", @RoleType";
            //狀態
            sql += ", @Status";
            //資料建立紀錄
            sql += ", '', SysDateTime(), @CreateId, SysDateTime(), @CreateId";


            return Execute(@sql, userEdit);
        }

        /// <summary>
        /// 取得角色ID，流水號順編
        /// </summary>
        /// <returns></returns>
        public int GetRoleNewID(AuthRole_SaveMain sm)
        {
            string sql = "";

            sql += "Select Count(*) as Cnt ";
            sql += "  From cRoleInfo";
            sql += " Where 1 = 1";

            sql += "   And RoleType = @RoleType";

            return SingleOrDefault<int>(@sql, sm);
        }

        public int Insert_Grid1_RoleFunc(AuthRole_SaveDetailGrid1 sd)
        {
            string sql = "";

            sql += "Insert Into cRoleFunc (";
            //群組代號
            sql += "RoleID";
            //程式代號
            sql += ", FuncID";
            //是否有執行權限
            sql += ", IsRun";
            //是否有查詢權限
            sql += ", IsQuery";
            //是否有新增權限
            sql += ", IsInsert";
            //是否有修改權限
            sql += ", IsEdit";
            //是否有刪除權限
            sql += ", IsDelete";
            //是否有檢視權限
            sql += ", IsView";
            //資料建立紀錄
            sql += ", SYNCTIME, CREATEDT, CREATEID, MODIFYDT, MODIFYID)";
            sql += " ";
            sql += " Select";
            //群組代號
            sql += " @RoleId";
            //程式代號
            sql += ", @ProgId";
            //是否有執行權限
            sql += ", @ProgExec";
            //是否有查詢權限
            sql += ", @ProgQuery";
            //是否有新增權限
            sql += ", @ProgAdd";
            //是否有修改權限
            sql += ", @ProgMod";
            //是否有刪除權限
            sql += ", @ProgDel";
            //是否有檢視權限
            sql += ", @ProgView";
            //資料建立紀錄
            sql += ", '', SysDateTime(), @CreateId, SysDateTime(), @CreateId";

            return Execute(@sql, sd);
        }

        public int Update_RoleInfo(AuthRole_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cRoleInfo ";
            sql += " SET ";
            //群組名稱
            sql += " RoleName = @RoleName,";
            //群組說明
            sql += " RoleType = @RoleType,";
            //狀態
            sql += " RoleStatus = @Status,";
            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";
            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 使用者代號 
            sql += " AND RoleID = @RoleId";

            return Execute(@sql, sm);
        }

        public int Delete_RoleInfo(AuthRole_SaveMain userEdit)
        {
            string sql = "";

            sql += "Delete From cRoleInfo ";
            sql += " Where 1 = 1 ";
            sql += "  And RoleID = @RoleId";

            return Execute(@sql, userEdit);
        }

        public int Delete_Grid1_RoleFunc(AuthRole_SaveMain sm)
        {
            string sql = "";

            sql += "Delete From cRoleFunc ";
            sql += " Where 1 = 1 ";
            sql += "  And RoleID = @RoleId";

            return Execute(@sql, sm);
        }

    }
}

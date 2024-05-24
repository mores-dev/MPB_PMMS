using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPB_Entities.COMMON;
using MPB_Entities.Auth;

namespace MPB_DAL.Auth
{
    public class AuthLogin_DAL : DALBase
    {
        public AuthLogin_DAL() { }

        public AuthLogin_DAL(DbManager db) : base(db) { }

        public AuthLogin_EditEntities Select_ATH_USER(AuthLogin_EditEntities qc, string encryptionPd)
        {
            string sql = "";
            sql += @"select distinct ACCOUNT, AccStatus, IsNull(PwdErr, 0) PwdErr, PASSWORD ";
            sql += " , Convert(Varchar(19), ISNULL(LastLogin, GetDate()), 121) as LastLogin";
            sql += "  from cAFCAccount M ";
            sql += " where  1 = 1 ";
            sql += "  AND M.DeviceTypeID IN ('C') ";
            //sql += "  AND M.ACCOUNT = @Account and M.PASSWORD = @encryptionPd";
            sql += "  AND M.ACCOUNT = @UserAAA ";

            return SingleOrDefault<AuthLogin_EditEntities>(sql, qc, new { encryptionPd = encryptionPd });
        }

        public UserInfo Select_UserInfo(string userId)
        {
            string sql = @"select distinct U.ACCOUNT, U.NAME, U.AccStatus, U.PASSWORD, U.C_ID, C.C_NAME ";
            sql += " , Convert(Varchar(19), ISNULL(U.LastChanged, U.CREATEDT), 121) as LastChanged";
            sql += "\n" + "from cAFCAccount U  ";
            sql += "\n" + "left join cCompany C ON C.C_ID = U.C_ID ";
            //sql += "\n" + "left join AuthDept S on S.DeptID = U.DeptID";
            sql += "\n" + "where U.ACCOUNT = @UserId and U.AccStatus = 'Y'";
            sql += "\n" + "  AND U.DeviceTypeID = 'C'";

            return SingleOrDefault<UserInfo>(sql, new { UserId = userId });
        }

        public List<UserProgramInfo> Select_UserProgramInfos(string userId)
        {
            string sql = "";
            sql = @"select distinct B.FuncName as ProgName, A.FuncID, B.FuncValue1 as ProgID ";
            sql += "\n" + ", A.IsInsert as ProgAdd, A.IsDelete as ProgDel ";
            sql += "\n" + ", A.IsEdit as ProgMod, A.IsRun as ProgExec, A.IsView as ProgView ";
            //sql += "\n" + "--, A.ProgDo, A.ProgUndo ";
            sql += "\n" + "from cRoleFunc A ";
            sql += "\n" + "join cFuncInfo B on B.FuncID = A.FuncID AND B.DeviceTypeID = 'C' ";
            sql += "\n" + "join cAccountRole C on C.RoleID = A.RoleID ";
            sql += "\n" + "join cRoleInfo D on D.RoleID = C.RoleID ";
            sql += "\n" + "join cAFCAccount E on rtrim(E.ACCOUNT) = rtrim(C.ACCOUNT) and E.AccStatus = 'Y' and rtrim(E.ACCOUNT) = @0 ";

            sql += "\n" + " Where 1 = 1";
            return Fetch<UserProgramInfo>(sql, userId);
        }

        public void UpdateErrCount(AuthLogin_EditEntities sm)
        {
            string sql = "";
            sql += "Update cAFCAccount ";
            sql += "  Set PwdErr = ISNULL(PwdErr, 0) + 1";
            sql += "    , LastLogin = GetDate()";
            sql += "Where 1 = 1";
            sql += "  And ACCOUNT = @UserAAA";
            sql += "  AND DeviceTypeID = 'C'";

            Execute(@sql, sm);
        }

        public void ResetErrCount(AuthLogin_EditEntities sm)
        {
            string sql = "";
            sql += "Update cAFCAccount ";
            sql += "  Set PwdErr = 0";
            //sql += "    , LastChanged = GetDate()";
            sql += "    , LastLogin = GetDate()";
            sql += "Where 1 = 1";
            sql += "  And ACCOUNT = @UserAAA";
            sql += "  AND DeviceTypeID = 'C'";

            Execute(@sql, sm);
        }

        public AuthLogin_LoginLog Select_Login_Log(AuthLogin_EditEntities vm)
        {
            string sql = "";
            sql += "Select";
            sql += " Convert(Varchar, LOG_DATE, 120) as LOG_DATE";
            sql += " , ERR_CNT";

            sql += " From Login_Log A ";
            sql += "Where 1 = 1";
            sql += "  And ACCOUNT = @UserAAA";

            return SingleOrDefault<AuthLogin_LoginLog>(@sql, vm);
        }

        public void Insert_Login_Log(AuthLogin_EditEntities vm)
        {
            string sql = "";

            sql += "insert into Login_Log (";
            sql += "ACCOUNT";
            sql += ", LOG_DATE";
            sql += ", ERR_CNT";

            sql += ") Select";
            sql += " @UserAAA";
            sql += " , GETDATE()";
            sql += " , 0 ";

            Execute(@sql, vm);
        }

        public void Update_Login_Log(AuthLogin_EditEntities vm, AuthLogin_LoginLog loginLog)
        {
            string sql = "";

            sql += "Update Login_Log Set";

            sql += " ERR_CNT = " + loginLog.ErrCnt;

            sql += " , LOG_DATE = GETDATE()";

            sql += " Where ACCOUNT = @UserAAA";

            Execute(@sql, vm);
        }
    }
}

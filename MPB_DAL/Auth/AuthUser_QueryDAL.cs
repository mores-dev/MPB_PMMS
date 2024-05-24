using MPB_Entities.Auth;
using MPB_Entities.Helper;

namespace MPB_DAL.Auth
{
    public class AuthUser_QueryDAL : DALBase
    {
        public AuthUser_QueryDAL() { }

        public AuthUser_QueryDAL(DbManager db) : base(db) { }

        public PageList<AuthUser_QueryResult> GetPageList(AuthUser_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";
            //使用者代號
            sql += " A.ACCOUNT as UserID";
            //單位別
            sql += " , CASE WHEN A.UserType = 'B' THEN ISNULL(D.CodeDesc, '') ";
            sql += "        WHEN A.UserType = 'C' THEN ISNULL(E.C_NAME, '') ";
            sql += "        ELSE ''  ";
            sql += "   END AS  CAT";

            //使用者名稱
            sql += " ,A.NAME as UserName";
            //使用者類型
            //sql += " ,C.CodeDesc as UserType";
            sql += " , R.RoleName as UserType";
            //是否啟用  
            sql += ", CASE WHEN A.AccStatus='Y' THEN '啟用' ELSE '' END";
            sql += " AS Status";
            //FROM AND LEFT JOIN 
            sql += " FROM cAFCAccount   A    "; //使用者檔 
            sql += " Left Join cAccountRole B ON A.ACCOUNT = B.ACCOUNT AND B.DeviceTypeID = 'C'";
            sql += " Left Join cRoleInfo R ON B.RoleID = R.RoleID";

            //sql += " LEFT JOIN cCodeList C ON C.CodeID = A.UserType AND C.CodeKey = 'SYS01002'    "; //使用者檔 

            sql += " Left Join cCodeList D ON D.CodeKey = 'SYS01003' AND A.DEPT = D.CodeID "; // 單位檔
            sql += " Left Join cCompany E ON A.C_ID = E.C_ID "; //航商設定
            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   AND A.ACCOUNT NOT IN ('admin', 'mdsmgr')";

            //查詢條件 使用者代號
            if (!string.IsNullOrWhiteSpace(qc.UserId))
            {
                sql += " and A.ACCOUNT Like '%'+@UserId+'%'";
            }
            //查詢條件 使用者名稱
            if (!string.IsNullOrWhiteSpace(qc.UserName))
            {
                sql += " and A.NAME Like '%'+@UserName+'%'";
            }
            //查詢條件 使用者名稱
            if (!string.IsNullOrWhiteSpace(qc.UserType))
            {
                //sql += " and A.UserType = @UserType";
                sql += " and R.RoleID = @UserType";
            }
            //查詢條件 是否啟用
            if (!string.IsNullOrWhiteSpace(qc.Status))
            {
                sql += " and rtrim(A.AccStatus) = @Status";
            }
            //ORDER BY (排序欄位)
            sql += " ORDER BY A.DeviceTypeID ASC, A.ACCOUNT ASC";

            return PageList<AuthUser_QueryResult>(qc.ToPage, @sql, qc);
        }
    }
}

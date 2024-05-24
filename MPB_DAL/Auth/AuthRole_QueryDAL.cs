
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Auth
{
    public class AuthRole_QueryDAL : DALBase
    {
        public AuthRole_QueryDAL() { }

        public AuthRole_QueryDAL(DbManager db) : base(db) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="page"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PageList<AuthRole_QueryResult> GetPageList(AuthRole_QueryCondition qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";
            //群組代號
            sql += " A.RoleID";
            //群組名稱
            sql += " ,A.RoleName";
            //群組說明
            sql += " , C.CodeDesc as RoleType";
            //是否啟用  
            sql += ", CASE WHEN A.RoleStatus = 'Y' Then '啟用' ELSE '' END";
            sql += " AS Status";
            //FROM AND LEFT JOIN 
            sql += " FROM cRoleInfo   A    "; //群組檔 
            sql += " Left Join cCodeList C ON A.RoleType = C.CodeID AND C.CodeKey = 'SYS01002' ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   AND A.RoleID != '0000' ";

            //限定群組代號
            sql += "   AND A.RoleID IN ('1000', '1002', '2000', '2001', '4000') ";
            sql += "";
            //查詢條件 群組代號
            if (!string.IsNullOrWhiteSpace(qc.RoleId))
            {
                sql += " and A.RoleID Like '%'+@RoleId+'%'";
            }
            //查詢條件 群組名稱
            if (!string.IsNullOrWhiteSpace(qc.RoleName))
            {
                sql += " and A.RoleName Like '%'+@RoleName+'%'";
            }
            //查詢條件 是否啟用
            if (!string.IsNullOrWhiteSpace(qc.Status))
            {
                sql += " and rtrim(A.RoleStatus) = @Status";
            }
            //ORDER BY (排序欄位)
            sql += " ORDER BY A.RoleID ASC";

            return PageList<AuthRole_QueryResult>(qc.ToPage, @sql, qc);
        }

    }
}

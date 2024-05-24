using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPB_Entities.Home;
using MPB_Entities.Helper;

namespace MPB_DAL.Home
{
    /// <summary>
    /// Ltree DAL
    /// </summary>
    public class LtreeDAL : DALBase
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public LtreeDAL() { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="db">DbManager</param>
        public LtreeDAL(DbManager db) : base(db) { }

        /// <summary>
        /// 取得Ltree清單資料
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="sys">sys</param>
        /// <returns>LtreeQueryResult</returns>
        public List<LtreeQueryResult> GetList(string userId)
        {
            string sql = "";

            sql += "Select Distinct ";
            sql += "\n" + "C.FuncValue1 as PROG_ID ";
            sql += "\n" + ", C.FuncName as PROG_NAME ";
            sql += "\n" + ", C.DispOrder as PROG_SEQ ";
            sql += "\n" + ", D.FuncValue1 as MENU_ID ";
            sql += "\n" + ", D.FuncName as MENU_NAME ";
            sql += "\n" + ", C.DispOrder ";
            sql += "\n" + ", D.FuncID ";
            sql += "\n" + ", D.DispOrder ";
            sql += "\n" + "From cAccountRole A ";
            sql += "\n" + "Join cRoleFunc B On A.RoleID = B.RoleID ";
            sql += "\n" + "Join cFuncInfo C On B.FuncID = C.FuncID ";
            sql += "\n" + "Join (Select FuncID, FuncName, DispOrder, FuncValue1 ";

            sql += "\n" + "From cFuncInfo F ";

            sql += "\n" + "Where F.DeviceTypeID = 'C' And F.ParentFuncID = '000' ";
            sql += "\n" + ") D On C.ParentFuncID = D.FuncID ";

            sql += "\n" + "Where Rtrim(A.ACCOUNT) = @0 ";
            sql += "\n" + "Order by D.FuncID, C.DispOrder ";

            return Fetch<LtreeQueryResult>(@sql, userId);
        }
    }
}

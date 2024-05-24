using MPB_Entities.COMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_DAL.COMMON
{
    public class VendorDAL : DALBase
    {
        /// <summary>
        /// 取得航商下拉選單
        /// </summary>
        /// <returns></returns>
        public List<CodeName> GetVendorDropDown()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT ");
            sql.AppendLine("C_ID AS Code, ");
            sql.AppendLine("C_NAME AS [NAME] ");
            sql.AppendLine("FROM cCompany ");
            sql.AppendLine("WHERE ");
            sql.AppendLine("C_ID <>'C00000' AND ");
            sql.AppendLine("C_STATUS = '1'");
            return Fetch<CodeName>(sql.ToString());
        }
    }
}

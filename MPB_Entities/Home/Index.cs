using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MPB_Entities.COMMON;

namespace MPB_Entities.Home
{
    /// <summary>
    /// 子系統代碼查詢明細 Entity
    /// </summary>
    public class IndexQueryResult
    {
        /// <summary>
        /// 登入者資訊
        /// </summary>
        public UserInfo LoginUser { get; set; }
        /// <summary>
        /// 可使用的子系統
        /// </summary>
        public List<CodeName> SubSystem { get; set; }

        /// <summary>
        /// 可用的選單功能
        /// </summary>
        public List<LtreeQueryResult> LtreeMenu { get; set; }

        //[Column("CODE_ID")]
        //public string SysId { get; set; }

        //[Column("CODE_DESC")]
        //public string SysName { get; set; }

    }
}

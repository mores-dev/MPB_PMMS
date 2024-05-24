using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_Entities.COMMON
{
    /// <summary>
    /// 執行結果資訊
    /// </summary>
    public class ProcessResult
    {
        public ProcessResult()
        {
            ReturnId = 0;
            ReturnMessage = "";
        }
        /// <summary>
        /// 執行結果代碼，0表示執行成功，其餘為失敗。
        /// </summary>
        public int ReturnId { get; set; }
        /// <summary>
        /// 執行結果訊息
        /// </summary>
        public string ReturnMessage { get; set; }
        /// <summary>
        /// 執行後返回，空白表示停留在原頁面
        /// </summary>
        public string ReturnModule { get; set; }
        /// <summary>
        /// 執行後返回頁面，空白表示停留在原頁面
        /// </summary>
        public string ReturnPage { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;

namespace MPB_Entities.COMMON
{
    /// <summary>
    /// 執行結果資訊
    /// </summary>
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AjaxKeyCountResult
    {
        /// <summary>
        /// 符合條件筆數
        /// </summary>
        [Column("KEY_COUNT")]
        public int KeyCount { get; set; }
    }

    public class AjaxImageUploadResult
    {
        [Column("Source")]
        public string Source { get; set; }

        [Column("ImgPath")]
        public string ImgPath { get; set; }
    }
}

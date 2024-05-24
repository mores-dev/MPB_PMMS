using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_Entities.COMMON
{
    public class ImageDir
    {
        /// <summary>
        /// 暫存資料夾路徑
        /// </summary>
        public string TempDir { get; set; }

        /// <summary>
        /// 實際存檔路徑
        /// </summary>
        public string SaveDir { get; set; }

        /// <summary>
        /// 網路路徑
        /// </summary>
        public string WebDir { get; set; }
    }
}

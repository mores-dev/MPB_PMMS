using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;

namespace MPB_Entities.COMMON
{
    public class UserLog
    {
        /// <summary>
        /// 類別
        /// </summary>
        [Column("LogType")]
        public string LogType { get; set; }
        /// <summary>
        /// 異動日期
        /// </summary>
        [Column("LogDT")]
        public string LogDT { get; set; }
        /// <summary>
        /// 使用者代號
        /// </summary>
        [Column("UserID")]
        public string UserId { get; set; }
        /// <summary>
        /// 程式代號
        /// </summary>
        [Column("ProgID")]
        public string ProgId { get; set; }
        /// <summary>
        /// 程式執行功能
        /// </summary>
        [Column("ProgFunc")]
        public string ProgFunc { get; set; }
        /// <summary>
        /// 資料Key值
        /// </summary>
        [Column("KeyData")]
        public string KeyData { get; set; }
        /// <summary>
        /// 登入IP
        /// </summary>
        [Column("LoginIP")]
        public string LoginIp { get; set; }
    }
}

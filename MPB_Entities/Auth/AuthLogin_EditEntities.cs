using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthLogin_EditEntities : EditBase
    {
        /// <summary>
        /// 帳號
        /// </summary>
        [Required]
        [Display(Name = "帳號")]
        [Column("ACCOUNT")]
        [StringLength(20)]
        public string UserAAA { get; set; }//Fix 2023/10/04 CheckMarx原碼檢測: Excessive Data Exposure\路徑 1:
        /// <summary>
        /// 密碼
        /// </summary>
        [Required]
        [Display(Name = "密碼")]
        [Column("PASSWORD")]
        public string UserPD { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        [Column("AccStatus")]
        public string Status { get; set; }

        [Display(Name = "錯誤次數")]
        [Column("PwdErr")]
        public int PppErr { get; set; }

        [Display(Name = "最近登入時間")]
        [Column("LastLogin")]
        public string LastLogin { get; set; }

        [Column("ID")]
        public string ID { get; set; }

        [Display(Name = "Token")]
        [Column("token")]
        public string token { get; set; }

    }

    public class AuthLogin_LoginLog
    {
        [Column("LOG_DATE")]
        public string LogDate { get; set; }

        [Column("ERR_CNT")]
        public int ErrCnt { get; set; }

        public DateTime LogDT { get; set; }
    }
}

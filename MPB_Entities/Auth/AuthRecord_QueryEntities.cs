using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthRecord_QueryCondition : QueryBase
    {
        [DisplayName("記錄日期-起")]
        public string LogOn { get; set; }

        [DisplayName("記錄日期-迄")]
        public string LogOff { get; set; }
    }

    public class AuthRecord_QueryResult
    {
        [DisplayName("日期")]
        [Column("LogDT")]
        public string LogDt { get; set; }

        [DisplayName("系統別")]
        [Column("LogSys")]
        public string LogSys { get; set; }

        [DisplayName("使用者")]
        [Column("UserID")]
        public string Account { get; set; }

        [DisplayName("類別")]
        [Column("LogType")]
        public string LogType { get; set; }

        [DisplayName("程式名稱")]
        [Column("ProgName")]
        public string ProgName { get; set; }

        [DisplayName("IP")]
        [Column("IP")]
        public string Ip { get; set; }

    }
}

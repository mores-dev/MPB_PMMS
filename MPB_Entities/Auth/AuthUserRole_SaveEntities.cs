using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthUserRole_SaveMain : SaveBase
    {
        [DisplayName("使用者代號")]
        [Column("UserID")]
        public string UserId { get; set; }
    }
    /// <summary>
    /// 多筆區Grid1欄位--資料表：AuthRoleProg (群組程式檔)
    /// </summary>
    public class AuthUserRole_SaveDetailGrid1 : SaveBase
    {
        [DisplayName("群組代號")]
        [Column("RoleID")]
        public string RoleId { get; set; }

        [DisplayName("使用者代號")]
        [Column("UserID")]
        public string UserId { get; set; }

        [DisplayName("系統對應")]
        [Column("DeviceTypeID")]
        public string DeviceTypeId { get; set; }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthUserRole_EditMain : EditBase
    {
        [DisplayName("使用者代號")]
        [Column("UserID")]
        public string UserId { get; set; }

        [DisplayName("使用者名稱")]
        [Column("UserName")]
        public string UserName { get; set; }
    }

    
    /// <summary>
    /// 多筆區Grid1欄位--資料表：AuthUserRole (群組程式檔)
    /// </summary>
    public class AuthUserRole_EditDetailGrid1 : EditBase
    {
        [DisplayName("群組代號")]
        [Column("RoleID")]
        public string RoleId { get; set; }

        [DisplayName("使用者代號")]
        [Column("UserID")]
        public string UserId { get; set; }
    }
}

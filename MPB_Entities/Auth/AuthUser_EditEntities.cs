using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthUser_EditMain : EditBase
    {
        [DisplayName("使用者代號")]
        [Column("UserID")]
        public string UserId { get; set; }

        [DisplayName("使用者類型")]
        [Column("UserType")]
        public string UserType { get; set; }

        [DisplayName("使用者名稱")]
        [Column("UserName")]
        public string UserName { get; set; }

        [DisplayName("密碼")]
        [Column("Pwd")]
        public string Pd { get; set; }

        [DisplayName("單位")]
        [Column("DEPT")]
        public string DEPT { get; set; }

        [DisplayName("狀態")]
        [Column("Status")]
        public string Status { get; set; }

        [DisplayName("航商")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        [DisplayName("角色代號")]
        [Column("RoleID")]
        public string RoleId { get; set; }
    }
}

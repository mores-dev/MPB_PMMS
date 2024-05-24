using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthRole_QueryCondition : QueryBase
    {
        //群組代號
        [DisplayName("群組代號")]
        public string RoleId { get; set; }

        //群組名稱
        [DisplayName("群組名稱")]
        public string RoleName { get; set; }

        //狀態
        [DisplayName("狀態")]
        public string Status { get; set; }
    }

    public class AuthRole_QueryResult
    {
        [DisplayName("群組代號")]
        [Column("RoleID")]
        public string RoleId { get; set; }

        [DisplayName("群組名稱")]
        [Column("RoleName")]
        public string RoleName { get; set; }

        [DisplayName("群組類型")]
        [Column("RoleType")]
        public string RoleType { get; set; }

        [DisplayName("狀態")]
        [Column("Status")]
        public string Status { get; set; }
    }
}

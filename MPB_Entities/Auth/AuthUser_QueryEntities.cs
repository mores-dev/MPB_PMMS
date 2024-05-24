using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthUser_QueryCondition : QueryBase
    {
        //使用者代號
        [DisplayName("使用者代號")]
        public string UserId { get; set; }

        //使用者名稱
        [DisplayName("使用者名稱")]
        public string UserName { get; set; }

        [DisplayName("使用者類型")]
        public string UserType { get; set; }

        //狀態
        [DisplayName("狀態")]
        public string Status { get; set; }
    }

    public class AuthUser_QueryResult
    {
        [DisplayName("使用者代號")]
        [Column("UserID")]
        public string UserId { get; set; }

        [DisplayName("類別")]
        [Column("CAT")]
        public string Cat { get; set; }

        [DisplayName("使用者名稱")]
        [Column("UserName")]
        public string UserName { get; set; }

        [DisplayName("狀態")]
        [Column("Status")]
        public string Status { get; set; }

        [DisplayName("使用者類別")]
        [Column("UserType")]
        public string UserType { get; set; }
    }
}

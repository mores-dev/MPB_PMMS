using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Auth
{
    /// <summary>
    /// 單筆區欄位 - AuthRole
    /// </summary>
    [System.Web.Mvc.Bind(Exclude = "")]
    public class AuthRole_SaveMain : SaveBase
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

    /// <summary>
    /// 多筆區Grid1欄位--資料表：AuthRoleProg (群組程式檔)
    /// </summary>
    public class AuthRole_SaveDetailGrid1 : SaveBase
    {
        [DisplayName("系統代號")]
        [Column("DeviceTypeID")]
        public string DeviceTypeId { get; set; }

        [DisplayName("群組代號")]
        [Column("RoleID")]
        public string RoleId { get; set; }

        [DisplayName("程式代號")]
        [Column("ProgID")]
        public string ProgId { get; set; }

        [DisplayName("是否有執行權限")]
        [Column("ProgExec")]
        public string ProgExec { get; set; }

        [DisplayName("是否有查詢權限")]
        [Column("ProgQuery")]
        public string ProgQuery { get; set; }

        [DisplayName("是否有新增權限")]
        [Column("ProgAdd")]
        public string ProgAdd { get; set; }

        [DisplayName("是否有修改權限")]
        [Column("ProgMod")]
        public string ProgMod { get; set; }

        [DisplayName("是否有刪除權限")]
        [Column("ProgDel")]
        public string ProgDel { get; set; }

        [DisplayName("是否有檢視權限")]
        [Column("ProgView")]
        public string ProgView { get; set; }

        //[DisplayName("是否有處理權限")]
        //[Column("ProgDo")]
        //public string ProgDo { get; set; }

        //[DisplayName("是否有取消權限")]
        //[Column("ProgUndo")]
        //public string ProgUndo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;

namespace MPB_Entities.COMMON
{
    /// <summary>
    /// 使用者資訊
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        [Column("ACCOUNT")]
        public string Id { get; set; }
        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 使用者Email
        /// </summary>
        [Column("Email")]
        public string Email { get; set; }
        /// <summary>
        /// 使用者是否啟用
        /// </summary>
        [Column("AccStatus")]
        public string Status { get; set; }
        /// <summary>
        /// 部門ID
        /// </summary>
        [Column("DeptID")]
        public string DeptID { get; set; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        [Column("DeptName")]
        public string DeptName { get; set; }

        //[Column("PASSWORD")]
        //public string Password { get; set; }

        [Column("C_ID")]
        public string C_ID { get; set; }

        [Column("C_NAME")]
        public string C_NAME { get; set; }

        [Column("LastChanged")]
        public string LastChanged { get; set; }

        /// <summary>
        /// 登入者代碼
        /// </summary>
        public string LoginUserId { get; set; }
        /// <summary>
        /// 登入者名稱
        /// </summary>
        public string LoginUserName { get; set; }
        /// <summary>
        /// 登入者資訊
        /// </summary>
        public UserInfo LoginUser { get; set; }
        /// <summary>
        /// 程式功能
        /// </summary>
        public List<UserProgramInfo> UserProgramInfos { get; set; }
    }

    public class UserProgramInfo
    {
        /// <summary>
        /// 程式編號
        /// </summary>
        [Column("ProgID")]
        public string ProgId { get; set; }

        /// <summary>
        /// 程式名稱
        /// </summary>
        [Column("ProgName")]
        public string ProgName { get; set; }

        /// <summary>
        /// 是否有新增權限
        /// </summary>
        [Column("ProgAdd")]
        public string ProgAdd { get; set; }

        /// <summary>
        /// 是否有刪除權限
        /// </summary>
        [Column("ProgDel")]
        public string ProgDel { get; set; }

        /// <summary>
        /// 是否有檢視權限
        /// </summary>
        [Column("ProgView")]
        public string ProgView { get; set; }

        /// <summary>
        /// 是否有修改權限
        /// </summary>
        [Column("ProgMod")]
        public string ProgMod { get; set; }

        /// <summary>
        /// 是否有執行權限
        /// </summary>
        [Column("ProgExec")]
        public string ProgExec { get; set; }

        ///// <summary>
        ///// 是否有正向執行權限
        ///// </summary>
        //[Column("ProgDo")]
        //public string ProgDo { get; set; }

        ///// <summary>
        ///// 是否有負向執行權限
        ///// </summary>
        //[Column("ProgUndo")]
        //public string ProgUndo { get; set; }
    }
}

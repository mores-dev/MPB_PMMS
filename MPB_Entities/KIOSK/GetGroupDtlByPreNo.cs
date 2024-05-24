using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/// <summary>
/// 
/// </summary>
namespace MPB_Entities.KIOSK
{
    /// <summary>
    /// 團客預登單登入
    /// </summary>
    public class GroupPreLoginModel
    {
        /// <summary>
        /// 預登號碼
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string prNo { get; set; }

        /// <summary>
        /// 連絡電話後三碼
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string contactTel { get; set; }

        /// <summary>
        /// 設備編號(內部API使用)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string DEVICE_ID { get; set; }

        ///// <summary>
        ///// kiosk選取功能 
        ///// M:團體多人1票
        ///// S:民宿1人1票
        ///// </summary>
        //[Required]
        //[StringLength(1)]
        //public string FuncCode { get; set; }
    }

    /// <summary>
    /// 團客預登單
    /// </summary>
    public class GroupModel
    {
        /// <summary>
        /// 人數
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 出發日期
        /// </summary>
        public string orderDate { get; set; }

    }

    /// <summary>
    /// 乘客名單
    /// </summary>
    public class PassengersModel
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int rowSeq { get; set; }

        /// <summary>
        /// 證件種類
        /// </summary>
        public string idTypeStr { get; set; }
        /// <summary>
        /// 身分證字號/證件號碼
        /// </summary>
        public string idNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string birthday { get; set; }

        ///// <summary>
        ///// 手機
        ///// </summary>
        //public string phone { get; set; }


    }

    /// <summary>
    /// 團體資料+乘客名單
    /// </summary>
    public class GroupPassengerModel
    {
        /// <summary>
        /// 團體
        /// </summary>
        public GroupModel group { get; set; }

        /// <summary>
        /// 乘客名單
        /// </summary>
        public List<PassengersModel> passengers { get; set; }
     }
}
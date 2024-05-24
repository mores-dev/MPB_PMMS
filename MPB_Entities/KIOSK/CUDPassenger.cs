using System.ComponentModel.DataAnnotations;

namespace MPB_Entities.KIOSK
{
    //public class CUDPassenger
    //{
    //    /// <summary>
    //    /// 預登號碼
    //    /// </summary>
    //    [Required]
    //    [StringLength(6)]
    //    public string prNo { get; set; }

    //    /// <summary>
    //    /// 連絡電話後三碼
    //    /// </summary>
    //    [Required]
    //    [StringLength(3)]
    //    public string contactTel { get; set; }

    //    /// <summary>
    //    /// 證件種類
    //    /// </summary>
    //    [StringLength(1)]
    //    public string idTypeStr { get; set; }

    //    /// <summary>
    //    /// 身分證字號/證件號碼
    //    /// </summary>
    //    [StringLength(50)]
    //    public string idNo { get; set; }

    //    /// <summary>
    //    /// 姓名
    //    /// </summary>
    //    [StringLength(50)]
    //    public string name { get; set; }

    //    /// <summary>
    //    /// 修改後證件種類
    //    /// </summary>
    //    [StringLength(1)]
    //    public string newIdTypeStr { get; set; }

    //    /// <summary>
    //    ///  修改後身分證字號/證件號碼
    //    /// </summary>
    //    [StringLength(50)]
    //    public string newIdNo { get; set; }


    //}


    public class BasePassenger
    {
        /// <summary>
        /// 預登號碼
        /// </summary>
        [Required]
        [StringLength(6)]
        public string prNo { get; set; }

        /// <summary>
        /// 連絡電話後三碼
        /// </summary>
        [Required]
        [StringLength(3)]
        public string contactTel { get; set; }

        /// <summary>
        /// 證件種類
        /// </summary>
        [StringLength(10)]
        public string idTypeStr { get; set; }

        /// <summary>
        /// 身分證字號/證件號碼
        /// </summary>
        [StringLength(50)]
        public string idNo { get; set; }

        /// <summary>
        /// 設備編號(內部API使用)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string DEVICE_ID { get; set; }
    }

    public class InsertPassenger : BasePassenger
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(50)]
        public string name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Required]
        [StringLength(10)]
        public string birthday { get; set; }
    }

    public class DeletePassenger : BasePassenger
    {
    }

    public class UpdatePassenger : BasePassenger
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(50)]
        public string name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Required]
        [StringLength(10)]
        public string birthday { get; set; }

        /// <summary>
        /// 修改後證件種類
        /// </summary>
        [StringLength(10)]
        public string newIdTypeStr { get; set; }

        /// <summary>
        ///  修改後身分證字號/證件號碼
        /// </summary>
        [StringLength(50)]
        public string newIdNo { get; set; }
    }
}

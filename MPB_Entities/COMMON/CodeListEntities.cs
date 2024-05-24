using System.ComponentModel;
using DataAccessUtility;

namespace MPB_Entities.COMMON
{
    /// <summary>
    /// 代碼檔資訊
    /// </summary>
    public class CodeName
    {
        /// <summary>
        /// 代碼
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// 代碼檔修改資訊
    /// </summary>
    public class CodeListEdit : CodeName
    {
        /// <summary>
        /// 是否啟用
        /// </summary>
        public string KeycodeEnable { get; set; }
        [Column("CREATEID")]
        [DisplayName("資料建立者")]
        public string CreateId { get; set; }

        [DisplayName("資料建立日期時間")]
        [Column("CREATEDT")]
        public string CreateDt { get; set; }
    }
    /// <summary>
    /// 代碼檔修改資訊
    /// </summary>
    public class CodeListSave : CodeListEdit
    {
        /// <summary>
        /// 排序
        /// </summary>
        public int DataSeq { get; set; }
        [Column("MODIFYID")]
        [DisplayName("資料修改者")]
        public string ModifyId { get; set; }
    }

    /// <summary>
    /// 包含分類之代碼檔資訊
    /// </summary>
    /// 
    public class CatCodeName
    {
        // 分類
        public string Cat { get; set; }
        // 代碼
        public string Code { get; set; }
        // 名稱
        public string Name { get; set; }
    }
}

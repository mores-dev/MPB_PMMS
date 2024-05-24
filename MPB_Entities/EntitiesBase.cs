using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MPB_Entities
{
    /// <summary>
    /// 查詢頁面共用
    /// </summary>
    public class QueryBase
    {
        //分頁
        public long ToPage { get; set; }

    }

    /// <summary>
    /// 編輯頁面共用(單筆區)
    /// </summary>
    public class EditBase
    {

        [DisplayName("資料建立者：")]
        [Column("CREATEID")]
        public string CreateId { get; set; }

        [DisplayName("資料建立時間：")]
        [Column("CREATEDT")]
        public string CreateDt { get; set; }

        [DisplayName("資料修改者：")]
        [Column("MODIFYID")]
        public string ModifyId { get; set; }

        [DisplayName("資料修改時間：")]
        [Column("MODIFYDT")]
        public string ModifyDt { get; set; }
    }

    /// <summary>
    /// 存檔頁面共用(單筆區)
    /// </summary>
    public class SaveBase
    {
        [DisplayName("資料建立者")]
        [Column("CREATEID")]
        public string CreateId { get; set; }

        [DisplayName("資料修改者")]
        [Column("MODIFYID")]
        public string ModifyId { get; set; }
    }

    /// <summary>
    /// 編輯頁面共用(多筆區)
    /// </summary>
    public class EditDetailBase
    {
        [DisplayName("資料建立者：")]
        [Column("CREATEID")]
        public string CreateId { get; set; }

        [DisplayName("資料建立時間：")]
        [Column("CREATEDT")]
        public string CreateDt { get; set; }
    }

    /// <summary>
    /// 存檔頁面共用(多筆區)
    /// </summary>
    public class SaveDetailBase
    {
        [DisplayName("資料建立者：")]
        [Column("CREATEID")]
        public string CreateId { get; set; }     

        [DisplayName("資料建立時間：")]
        [Column("CREATEDT")]
        public string CreateDt { get; set; }

        [DisplayName("資料修改者")]
        [Column("MODIFYID")]
        public string ModifyId { get; set; }
    }
}

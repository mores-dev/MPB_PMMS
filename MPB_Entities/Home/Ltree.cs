using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using DataAccessUtility;

namespace MPB_Entities.Home
{
    /// <summary>
    /// Ltree查詢明細 Entity
    /// </summary>
    public class LtreeQueryResult
    {
        [Column("LEVEL_CODE")]
        public int LevelCode { get; set; }

        [Column("PROG_ID")]
        public string ProgId { get; set; }

        [Column("PROG_NAME")]
        public string ProgName { get; set; }

        [Column("PROG_SEQ")]
        public int ProgSeq { get; set; }

        [Column("REAL_ID")]
        public string RealId { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("MENU_SEQ")]
        public int MenuSeq { get; set; }

        [Column("MENU_NAME")]
        public string MenuName { get; set; }

        [Column("MENU_ID")]
        public string MenuId { get; set; }

        /// <summary>
        /// 是否群組
        /// </summary>
        [Column("PROG_FOLDER")]
        public string ProgFolder { get; set; }

        /// <summary>
        /// 是否選單顯示
        /// </summary>
        [Column("PROG_VISIABLE")]
        public string ProgVisiable { get; set; }

    }

}

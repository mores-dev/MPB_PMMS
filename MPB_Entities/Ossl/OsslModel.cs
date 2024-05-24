using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Ossl
{
    public class OsslModel
    {

        [Required]
        [Display(Name = "加密票號")]
        [Column("TKNO")]
        public string TkNo { get; set; }

        [Display(Name = "乘客編號")]
        [Column("PGNO")]
        public string PgNo { get; set; }

        [Display(Name = "證件編號")]
        [Column("IDNO")]
        public string IdNo { get; set; }

        [Display(Name = "連絡電話")]
        [Column("PHONE")]
        public string Phone { get; set; }

        public DateTime EnterTime { get; set; }
    }
}

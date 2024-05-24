using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace MPB_Entities.Ossl
{
    public class OsslPersonalTicket
    {
        [Required]
        [Column("PG_ID")]
        public string PG_ID { get; set; }

        [Required]
        [DisplayName("票號")]
        [Column("TK_NO")]
        public string TK_NO { get; set; }

        [Required]
        [DisplayName("姓名")]
        [Column("NAME")]
        public string NAME { get; set; }

        [Required]
        [DisplayName("證件種類")]
        [Column("ID_TYPE")]
        public string ID_TYPE { get; set; }

        [Required]
        [DisplayName("身分證字號/證件編號")]
        [Column("ID_NO")]
        public string ID_NO { get; set; }

        [Required]
        [DisplayName("生日")]
        [Column("BIRTHDAY")]
        public string BIRTHDAY { get; set; }

        [Required]
        [DisplayName("連絡電話")]
        [Column("PHONE")]
        public string PHONE { get; set; }

        [Required]
        [DisplayName("來源")]
        [Column("SOURCE")]
        public string SOURCE { get; set; }

        /// <summary>
        /// 設備編號(內部API使用)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string DEVICE_ID { get; set; }

        public string CheckFlag { get; set; }

        private string _VaildString = "";

        [Required]
        [DisplayName("驗證文字")]
        [Column("VaildString")]
        public string VaildString
        {
            get
            {
                if (string.IsNullOrEmpty(this._VaildString))
                    return "";
                return this._VaildString.ToUpper();
            }
            set
            {
                this._VaildString = value;
            }
        }
    }

    public class OsslPersonalTicketRequest
    {
        [Required]
        [DisplayName("票號")]
        [Column("TK_NO")]
        public string TK_NO { get; set; }

        /// <summary>
        /// 設備編號(內部API使用)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string DEVICE_ID { get; set; }

    }

    public class OsslPersonalTicketResponse
    {
        [Required]
        [DisplayName("票號")]
        [Column("TK_NO")]
        public string TK_NO { get; set; }

        [Required]
        [DisplayName("姓名")]
        [Column("NAME")]
        public string NAME { get; set; }

        [Required]
        [DisplayName("證件種類")]
        [Column("ID_TYPE")]
        public string ID_TYPE { get; set; }

        [Required]
        [DisplayName("身分證字號/證件編號")]
        [Column("ID_NO")]
        public string ID_NO { get; set; }

        [Required]
        [DisplayName("生日")]
        [Column("BIRTHDAY")]
        public string BIRTHDAY { get; set; }


    }

    public class OsslPersonalTicketEntites : SaveBase
    {
        [Required]
        [DisplayName("票號")]
        [Column("TK_NO")]
        public string TK_NO { get; set; }

        [Required]
        [DisplayName("姓名")]
        [Column("NAME")]
        public string NAME { get; set; }

        [Required]
        [DisplayName("證件種類")]
        [Column("ID_TYPE")]
        public string ID_TYPE { get; set; }

        [Required]
        [DisplayName("身分證字號/證件編號")]
        [Column("ID_NO")]
        public string ID_NO { get; set; }

        [Required]
        [DisplayName("生日")]
        [Column("BIRTHDAY")]
        public string BIRTHDAY { get; set; }

        [Required]
        [DisplayName("連絡電話")]
        [Column("PHONE")]
        public string PHONE { get; set; }

        [Required]
        [DisplayName("航商")]
        [Column("C_ID")]
        public string C_ID { get; set; }

        [Required]
        [DisplayName("去程日期")]
        [Column("ORDER_DATE")]
        public string ORDER_DATE { get; set; }

        [Required]
        [DisplayName("來源")]
        [Column("SOURCE")]
        public string SOURCE { get; set; }
    }
}

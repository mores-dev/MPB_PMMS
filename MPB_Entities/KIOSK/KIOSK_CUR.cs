using DataAccessUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_Entities.KIOSK
{
    public class KIOSK_CUR : SaveBase
    {
        public KIOSK_CUR() { }

        public KIOSK_CUR(int grdId, string grId, string name, string idType, string idNo, string birthday, string deviceId)
        {
            this.GRD_ID = grdId;
            this.GR_ID = int.Parse(grId);
            this.NAME = name;
            this.ID_TYPE = idType;
            this.ID_NO = idNo;
            this.BIRTHDAY = birthday;
            this.CreateId = deviceId;
            this.ModifyId = deviceId;
        }

        public KIOSK_CUR(string grId, string name, string idType, string idNo, string birthday, string deviceId)
        {
            this.GR_ID = int.Parse(grId) ;
            this.NAME = name;
            this.ID_TYPE = idType;
            this.ID_NO = idNo;
            this.BIRTHDAY = birthday;
            this.CreateId = deviceId;
            this.ModifyId = deviceId;
        }

        public KIOSK_CUR(int grdId, string grId, string deviceId)
        {
            this.GR_ID = int.Parse(grId);
            this.GRD_ID = grdId; 
            this.CreateId = deviceId;
            this.ModifyId = deviceId;
        }
        /// <summary>
        /// 預登編號
        /// </summary>
        [DisplayName("預登編號")]
        [Column("GR_NO")]
        public string GR_NO { get; set; }

        /// <summary>
        /// 團體流水號
        /// </summary>
        [DisplayName("團體流水號")]
        [Column("GR_ID")]
        public int GR_ID { get; set; }

        /// <summary>
        /// 團體流水號
        /// </summary>
        [DisplayName("乘客流水號")]
        [Column("GRD_ID")]
        public int GRD_ID { get; set; }

        /// <summary>
        /// 排序號碼
        /// </summary>
        [DisplayName("排序號碼")]
        [Column("SEQNO")]
        public int SEQNO { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DisplayName("姓名")]
        [Column("NAME")]
        public string NAME { get; set; }

        /// <summary>
        /// 證件總類
        /// </summary>
        [DisplayName("證件總類")]
        [Column("ID_TYPE")]
        public string ID_TYPE { get; set; }

        [DisplayName("證件號碼")]
        [Column("ID_NO")]
        public string ID_NO { get; set; }

        [DisplayName("生日")]
        [Column("BIRTHDAY")]
        public string BIRTHDAY { get; set; }

        [DisplayName("乘客筆數")]
        [Column("DTL_COUNT")]
        public int DTL_COUNT { get; set; }

        [DisplayName("聯絡號碼")]
        [Column("PHONE")]
        public string PHONE { get; set; }

        [DisplayName("團體單狀態")]
        [Column("CLOSE_STATUS")]
        public string CLOSE_STATUS { get; set; }

        [DisplayName("乘船日期")]
        [Column("ORDER_DATE")]
        public string ORDER_DATE { get; set; }
    }
}

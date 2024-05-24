using System;
using System.Collections.Generic;
using System.ComponentModel;
using DataAccessUtility;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_Entities.KIOSK
{
    public class GroupDecord
    {
        /// <summary>
        /// 團體流水號
        /// </summary>
        [DisplayName("團體流水號")]
        [Column("GR_ID")]
        public string GR_ID { get; set; }

        /// <summary>
        /// 預登號碼
        /// </summary>
        [DisplayName("預登號碼")]
        [Column("GR_NO")]
        public string GR_NO { get; set; }

        /// <summary>
        /// 預登號碼
        /// </summary>
        [DisplayName("認證碼3碼")]
        [Column("THREE_CODE")]
        public string THREE_CODE { get; set; }

        /// <summary>
        /// 去程日期
        /// </summary>
        [DisplayName("去程日期")]
        [Column("ORDER_DATE")]
        public DateTime ORDER_DATE { get; set; }

        /// <summary>
        /// 乘客筆數
        /// </summary>
        [DisplayName("乘客筆數")]
        [Column("DTL_COUNT")]
        public int DTL_COUNT { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        [DisplayName("連絡電話")]
        [Column("CONTACT_PHONE")]
        public string CONTACT_PHONE { get; set; }

        /// <summary>
        /// 旅宿業種類
        /// </summary>
        [DisplayName("旅宿業種類")]
        [Column("GA_TYPE")]
        public string GA_TYPE { get; set; }

        /// <summary>
        /// 團體單狀態
        /// </summary>
        [DisplayName("團體單狀態")]
        [Column("CLOSE_STATUS")]
        public string CLOSE_STATUS { get; set; }

        /// <summary>
        /// 團體單狀態
        /// </summary>
        [DisplayName("放行")]
        [Column("EDIT_LOCK")]
        public string EDIT_LOCK { get; set; }

        /// <summary>
        /// 是否啟用KIOSK取票
        /// </summary>
        [DisplayName("是否啟用KIOSK取票")]
        [Column("USE_KIOSK")]
        public string USE_KIOSK { get; set; }
    }

    public class GroupDecordDtl
    {
        /// <summary>
        /// 團體流水號
        /// </summary>
        [DisplayName("團體流水號")]
        [Column("GR_ID")]
        public int GR_ID { get; set; }

        /// <summary>
        /// 乘客流水號
        /// </summary>
        [DisplayName("乘客流水號")]
        [Column("GRD_ID")]
        public int GRD_ID { get; set; }

        /// <summary>
        /// 乘客流水號
        /// </summary>
        [DisplayName("排序")]
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

        [DisplayName("證件總類")]
        [Column("id_type_str")]
        public string ID_TYPE_STR { get; set; }

        [DisplayName("證件號碼")]
        [Column("ID_NO")]
        public string ID_NO { get; set; }

        [DisplayName("生日")]
        [Column("BIRTHDAY")]
        public string BIRTHDAY { get; set; }
    }

    public class GroupPassengerDecordParam
    {
        public GroupPassengerDecordParam() { }

        public GroupPassengerDecordParam(string grNo, string contactPhone, string idType, string idNo)
        {
            this.GR_NO = grNo;
            this.CONTACT_PHONE = contactPhone;
            this.ID_TYPE = idType;
            this.ID_NO = idNo;
        }

        public GroupPassengerDecordParam(string grId, string grNo, string contactPhone, string idType, string idNo)
        {
            this.GR_ID = grId;
            this.GR_NO = grNo;
            this.CONTACT_PHONE = contactPhone;
            this.ID_TYPE = idType;
            this.ID_NO = idNo;
        }
        /// <summary>
        /// 團體流水號
        /// </summary>
        [DisplayName("團體流水號")]
        [Column("GR_ID")]
        public string GR_ID { get; set; }
        /// <summary>
        /// 預登號碼
        /// </summary>
        [DisplayName("預登號碼")]
        [Column("GR_NO")]
        public string GR_NO { get; set; }

        /// <summary>
        /// 連絡電話後三碼
        /// </summary>
        [DisplayName("連絡電話後三碼")]
        [Column("CONTACT_PHONE")]
        public string CONTACT_PHONE { get; set; }

        [DisplayName("證件總類")]
        [Column("ID_TYPE")]
        public string ID_TYPE { get; set; }

        [DisplayName("證件號碼")]
        [Column("ID_NO")]
        public string ID_NO { get; set; }

        //[DisplayName("kiosk選取功能 ")]
        //[Column("FuncCode")]
        //public string FuncCode { get; set; }
    }

    public class GroupPassengerDecord
    {
        /// <summary>
        /// 團體流水號
        /// </summary>
        [DisplayName("團體流水號")]
        [Column("GR_ID")]
        public string GR_ID { get; set; }

        /// <summary>
        /// 乘客流水號
        /// </summary>
        [DisplayName("乘客流水號")]
        [Column("GRD_ID")]
        public int GRD_ID { get; set; }

        /// <summary>
        /// 預登號碼
        /// </summary>
        [DisplayName("預登號碼")]
        [Column("GR_NO")]
        public string GR_NO { get; set; }

        /// <summary>
        /// 船商
        /// </summary>
        [DisplayName("船商")]
        [Column("C_ID")]
        public int C_ID { get; set; }

        /// <summary>
        /// 去程日期
        /// </summary>
        [DisplayName("去程日期")]
        [Column("ORDER_DATE")]
        public DateTime ORDER_DATE { get; set; }

        /// <summary>
        /// 乘客筆數
        /// </summary>
        [DisplayName("乘客筆數")]
        [Column("DTL_COUNT")]
        public int DTL_COUNT { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        [DisplayName("連絡電話")]
        [Column("CONTACT_PHONE")]
        public string CONTACT_PHONE { get; set; }

        /// <summary>
        /// 團體單狀態
        /// </summary>
        [DisplayName("團體單狀態")]
        [Column("CLOSE_STATUS")]
        public string CLOSE_STATUS { get; set; }

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
    }
}

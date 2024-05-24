using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MPB_Entities.Api
{
    /// <summary>
    ///  請求資料
    /// </summary>
    public class QueryOrder_FUNC_Entities
    {
        /// <summary>
        /// 出發日期
        /// </summary>
        public string SHIPPING_DATE { get; set; }
        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string ORDER_ID { get; set; }
        /// <summary>
        /// 驗證碼 (3碼)
        /// </summary>
        public string CONTACT_PHONE { get; set; }

        /// <summary>
        /// 證件號碼
        /// </summary>
        public string ID_NO { get; set; }

        /// <summary>
        /// 航商代號
        /// </summary>
        [JsonIgnore]
        public string C_ID { get; set; }
    }

    public class QueryOrder_RSPN_Entities
    {
        /// <summary>
        /// 航班日期
        /// </summary>
        public string SHIPPING_DATE { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        public string ORDER_ID { get; set; }

        /// <summary>
        /// 預登驗證碼
        /// </summary>
        [JsonIgnore]
        public string THREE_CODE { get; set; }

        /// <summary>
        /// 訂單人數
        /// </summary>
        public int PEOLPLE_CNT { get; set; }

        /// <summary>
        /// 訂單類型
        /// </summary>
        public string ORDER_TYPE { get; set; }

        /// <summary>
        /// 旅行社/民宿 代碼
        /// </summary>
        public object AGENT_ID { get; set; }

        /// <summary>
        /// 旅行社/民宿/團體名稱
        /// </summary>
        public string AGENT_NAME { get; set; }

        /// <summary>
        /// 取票狀態
        /// </summary>
        public string REDEEM { get; set; }

        /// <summary>
        /// 訂單明細(陣列)
        /// </summary>
        public JArray ORDER_DTL { get; set; }
    }


    public class QUERY_ORDER_DTL
    {
        /// <summary>
        /// 航班日期
        /// </summary>
        [JsonIgnore]
        public string SHIPPING_DATE { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        [JsonIgnore]
        public string ORDER_ID { get; set; }

        /// <summary>
        /// 訂單人數
        /// </summary>
        [JsonIgnore]
        public int PEOLPLE_CNT { get; set; }

        /// <summary>
        /// 訂單類型
        /// </summary>
        [JsonIgnore]
        public string ORDER_TYPE { get; set; }

        /// <summary>
        /// 旅行社/民宿 代碼
        /// </summary>
        [JsonIgnore]
        public object AGENT_ID { get; set; }

        /// <summary>
        /// 旅行社/民宿/團體名稱
        /// </summary>
        [JsonIgnore]
        public string AGENT_NAME { get; set; }

        /// <summary>
        /// 解密後ID
        /// </summary>
        [JsonIgnore]
        public string DECODE_ID_NO { get; set; }

        /// <summary>
        /// 預登驗證碼
        /// </summary>
        [JsonIgnore]
        public string CONTACT_PHONE { get; set; }

        /// <summary>
        /// 預登驗證碼
        /// </summary>
        [JsonIgnore]
        public string THREE_CODE { get; set; }

        /// <summary>
        /// 證件類別
        /// </summary>
        public string ID_TYPE { get; set; }

        /// <summary>
        /// 證件號碼
        /// </summary>
        public string ID_NO { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string BIRTH { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        //[JsonIgnore]
        public string SEX { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        public string PHONE { get; set; }

        /// <summary>
        /// 取票狀態
        /// </summary>
        [JsonIgnore]
        public string REDEEM { get; set; }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_Entities.Api
{
    /// <summary>
    ///  請求資料
    /// </summary>
    public class QueryRegister_FUNC
    {
        /// <summary>
        /// 出發日期
        /// </summary>
        public string SHIPPING_DATE { get; set; }
    }

    /// <summary>
    /// 回覆資訊
    /// </summary>
    public class QueryRegister_RSPN
    {
        /// <summary>
        /// 出發日期
        /// </summary>
        public string SHIPPING_DATE { get; set; }

        /// <summary>
        /// 訂單筆數
        /// </summary>
        public int ORDER_CNT { get; set; }

        /// <summary>
        /// 訂單資料(陣列)
        /// </summary>
        public JArray ORDERS { get; set; }

        //public string ORDERS_STR { get; set; }        

        //public List<ORDER> ORDERS {
        //    get
        //    {
        //        return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ORDER>>(ORDERS_STR);
        //    }

        //    set
        //    {
        //        ORDERS_STR = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        //    }

        //}
    }

    /// <summary>
    /// 訂單資料
    /// </summary>
    public class ORDER
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string ORDER_ID { get; set; }

        /// <summary>
        /// 乘客人數
        /// </summary>
        public int PEOPLE_CNT { get; set; }

        /// <summary>
        /// 取票狀態
        /// </summary>
        public string REDEEM { get; set; }

        /// <summary>
        /// 聯絡人
        /// </summary>
        public string CONTACT_NAME { get; set; }

        /// <summary>
        /// 聯絡電話
        /// </summary>
        public string CONTACT_PHONE { get; set; }

        /// <summary>
        /// 聯絡EMAIL
        /// </summary>
        public string CONTACT_EMAIL { get; set; }

        /// <summary>
        /// 訂單來源
        /// </summary>
        public string CONTACT_TYPE { get; set; }

        /// <summary>
        /// 旅行社/民宿 代碼
        /// </summary>
        public string AGENT_ID { get; set; }

        /// <summary>
        /// 旅行社/民宿/團體名稱
        /// </summary>
        public string AGENT_NAME { get; set; }
 
    }    


}

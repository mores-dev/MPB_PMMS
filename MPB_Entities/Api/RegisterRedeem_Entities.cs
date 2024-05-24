using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_Entities.Api
{
    /// <summary>
    /// 民宿預登取票 回覆
    /// </summary>
    public class RegisterRedeem_Entities
    {
        /// <summary>
        /// 航班日期
        /// </summary>
        public string SHIPPING_DATE { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string ORDER_ID { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAccessUtility;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class WriteAPILog_Entities
    {
        public string DeviceType { get; set; }

        public string DEVICE_ID { get; set; }

        public string FuncId { get; set; }

        public string sJson { get; set; }

        public string RspnCode { get; set; }

        public string RspnMsg { get; set; }

        public string rJson { get; set; }

    }

    public class WriteAPILog_RTN_Entities
    {

        //API參數
        [DisplayName("執行結果")]
        public string RSPN_CODE { get; set; }

        //API參數
        [DisplayName("執行結果說明")]
        public string RSPN_MSG { get; set; }

    }
}

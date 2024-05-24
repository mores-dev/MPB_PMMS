using System.Collections.Generic;
using System.ComponentModel;
using DataAccessUtility;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class InsertTransData_TVS_FUNC_Entities
    {
        public string BUSINESS_DATE { get; set; }
        public string R_ID { get; set; }
        public string SC_TIME { get; set; }
        public string V_ID { get; set; }
        public string C_ID { get; set; }
        public string SC_ID { get; set; }
        public string ST_ID { get; set; }
        public string DEVICE_ID { get; set; }
        public string QRCODE { get; set; }
        public string QR_TYPE { get; set; }
        public int ALLOW_CNT { get; set; }
        public int SEAT_CNT { get; set; }
        public string TICKET_TYPE { get; set; }
        public string TICKET_NAME { get; set; }
        public string PSGR_NAME { get; set; }
        public string SEX { get; set; }
        public string ID_TYPE { get; set; }
        public string ID_NO { get; set; }
        public string BIRTHDAY { get; set; }
        public string ODR_NO { get; set; }
        public string CALL_TYPE { get; set; }
        public string CREATEDT { get; set; }
        public string CREATEID { get; set; }
        public string MODIFYDT { get; set; }
        public string MODIFYID { get; set; }
    }

    public class InsertTransData_TVS_RSPN_Entities
    {
        
    }
}

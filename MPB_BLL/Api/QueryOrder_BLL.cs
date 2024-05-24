using MPB_BLL.COMMON;
using MPB_DAL.Api;
using MPB_Entities.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MPB_BLL.Api
{
    public class QueryOrder_BLL : BLLBase
    {
        private QueryOrder_DAL dal = new QueryOrder_DAL();


        /// <summary>
        /// 簽名驗證
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool CheckSign(APIRequest request)
        {
            string sign = "FUNC_ID={0}&SYS_DATE={1}&ACNT_NO={2}&API_KEY={3}";
            sign = string.Format(sign, request.FUNC_ID, request.SYS_DATE, request.ACNT_NO, ConfigurationManager.AppSettings["QueryOrderKey"]);
            sign = EnDeCode.EncryptSHA1(sign);
            return request.FUNC_SIG == sign;
        }


        public APIResponse<QueryOrder_RSPN_Entities> GetQueryOrder(APIRequest request)
        {
            APIResponse<QueryOrder_RSPN_Entities> response = new APIResponse<QueryOrder_RSPN_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            QueryOrder_RSPN_Entities rspn = new QueryOrder_RSPN_Entities();
            QueryOrder_FUNC_Entities func = new QueryOrder_FUNC_Entities();
            string step = "0";
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);
                bool chkId = false;

                if (string.IsNullOrWhiteSpace(func.SHIPPING_DATE))
                    return response.Error("15", "出發日期不得為空");

                if (string.IsNullOrWhiteSpace(func.ID_NO) && string.IsNullOrWhiteSpace(func.ORDER_ID))
                    return response.Error("16", "必須輸入訂單號碼");
                else if (string.IsNullOrWhiteSpace(func.ORDER_ID))
                    chkId = true;

                func.C_ID = VendorBLL.GetCIdByNo(request.ACNT_NO);
                if (string.IsNullOrEmpty(func.C_ID))
                    return response.Error("17", "無此航商編號");

                List<QUERY_ORDER_DTL> dtls = dal.GetOrderDtl(func);
                
                step = "1";
                if (dtls?.Count > 0)
                {
                    if(dtls[0].ORDER_TYPE != "H")
                        return response.Error("18", "請使用團客報到功能");

                    if(dtls[0].SHIPPING_DATE != func.SHIPPING_DATE)
                        return response.Error("19", "限去程當日取票");

                    foreach (QUERY_ORDER_DTL dtl in dtls)
                    {
                        if (EnDeCode.DecryptAES256(dtl.ID_NO, out string dcrpt, KeyType.IDNO))
                        {
                            if (chkId)
                                dtl.DECODE_ID_NO = dcrpt;

                            if (string.IsNullOrWhiteSpace(dtl.SEX) && dtl.ID_TYPE == "0")
                                dtl.SEX = dcrpt.Substring(1, 1) == "1" ? "M" : "F";
                        }
                    }

                    //查詢條件為證件編號
                    if (chkId)
                    {
                        var ds = dtls.Where(x => x.DECODE_ID_NO == func.ID_NO);
                        QUERY_ORDER_DTL od = ds.FirstOrDefault();
                        if (ds.Count() <= 0)
                        {
                            return response.Error("乘客名冊無此證件編號");
                        }
                        if (ds.Count() > 1)
                        {
                            return response.Error("同日多筆乘客名冊內有此證件編號，請改用其他證件編號或是預登編號查詢");
                        }
                        dtls.RemoveAll(x => x.ORDER_ID != od.ORDER_ID);
                        ModelBLL.Convert(od, ref rspn);
                    }
                    else
                        ModelBLL.Convert(dtls.FirstOrDefault(), ref rspn);

                    if (rspn.THREE_CODE != func.CONTACT_PHONE)
                    {
                        logger.Warn(rspn.ORDER_ID + " 驗證碼(3碼)錯誤");
                        return response.Error("10", "驗證碼(3碼)錯誤");
                    }

                    step = "2";
                    rspn.ORDER_DTL = JArray.FromObject(dtls);
                    rspn.PEOLPLE_CNT = dtls.Count;
                }
                else
                {
                    logger.Warn(func.ORDER_ID + " 查無此預登編號");
                    return response.Error("10", "查無此預登編號");
                    //rspn.PEOLPLE_CNT = 0;
                    //rspn.ORDER_DTL = new JArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                //return response.Error(ex.Message);
                return response.Error("預約名冊明細取得失敗! Code:" + step);
            }

            return response.OK(rspn);

        }
    }


}

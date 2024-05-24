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
    public class QueryRegister_BLL : BLLBase
    {
        private QueryRegister_DAL dal = new QueryRegister_DAL();


        /// <summary>
        /// 簽名驗證
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool CheckSign(APIRequest request)
        {
            string sign = "FUNC_ID={0}&SYS_DATE={1}&ACNT_NO={2}&API_KEY={3}";
            sign = string.Format(sign, request.FUNC_ID, request.SYS_DATE, request.ACNT_NO, ConfigurationManager.AppSettings["QueryRegisterKey"]);
            sign = EnDeCode.EncryptSHA1(sign);
            return request.FUNC_SIG == sign;
        }

        public APIResponse<QueryRegister_RSPN> GetQueryRegister(APIRequest request)
        {
            APIResponse<QueryRegister_RSPN> response = new APIResponse<QueryRegister_RSPN>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            QueryRegister_RSPN rspn = new QueryRegister_RSPN();
            string step = "0";
            try
            {

                JObject jObject = (JObject)request.FUNC_DATA;
                rspn.SHIPPING_DATE = jObject["SHIPPING_DATE"].ToString();
                if (string.IsNullOrWhiteSpace(rspn.SHIPPING_DATE))
                    return response.Error("出發日期不得為空");

                string cId = VendorBLL.GetCIdByNo(request.ACNT_NO);
                if (string.IsNullOrEmpty(cId))
                    return response.Error("無此航商編號");

                List<ORDER> orders = dal.GetOrders(rspn.SHIPPING_DATE, cId);
                step = "1";
                if (orders != null)
                {
                    rspn.ORDERS = JArray.FromObject(orders);
                    rspn.ORDER_CNT = orders.Count;
                }
                else
                {
                    rspn.ORDER_CNT = 0;
                    rspn.ORDERS = new JArray();
                }
                step = "2";
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                //return response.Error(ex.Message);
                return response.Error("預約名冊取得失敗! Code:" + step);
            }

            return response.OK(rspn);

        }


        public APIResponse<QueryRegisterDtl_RSPN> GetQueryRegisterDtl(APIRequest request)
        {
            APIResponse<QueryRegisterDtl_RSPN> response = new APIResponse<QueryRegisterDtl_RSPN>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            QueryRegisterDtl_RSPN rspn = new QueryRegisterDtl_RSPN();
            QueryRegisterDtl_FUNC func = new QueryRegisterDtl_FUNC();
            string step = "0";
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);
                bool chkId = false;

                if (string.IsNullOrWhiteSpace(func.SHIPPING_DATE))
                    return response.Error("出發日期不得為空");

                if (string.IsNullOrWhiteSpace(func.ID_NO) && string.IsNullOrWhiteSpace(func.ORDER_ID))
                    return response.Error("必須選填證件號碼或訂單號碼");
                else if (string.IsNullOrWhiteSpace(func.ORDER_ID))
                    chkId = true;

                func.C_ID = VendorBLL.GetCIdByNo(request.ACNT_NO);
                if (string.IsNullOrEmpty(func.C_ID))
                    return response.Error("無此航商編號");

                List<ORDER_DTL> dtls = dal.GetOrderDtl(func);
                
                step = "1";
                //if (dtls != null)
                if (dtls?.Count > 0)
                {      
                    //ModelBLL.Convert(dtls.FirstOrDefault(), ref rspn);
                    //List<string> keys = new List<string>();
                    foreach (ORDER_DTL dtl in dtls)
                    {
                        if (EnDeCode.DecryptAES256(dtl.ID_NO, out string dcrpt, KeyType.IDNO))
                        {
                            if (chkId)
                                dtl.DECODE_ID_NO = dcrpt;
                            else
                            {
                                ModelBLL.Convert(dtls.FirstOrDefault(), ref rspn);
                            }
                            //keys.Add(dcrpt);
                            //if (keys.Where(x=>x == func.ID_NO).Count()>1)
                            //{
                            //        return response.Error("同日多筆乘客名冊內有此證件編號，請改用其他證件編號或是預登編號查詢");                                
                            //}
                            
                            //if (chkId && func.ID_NO == dcrpt)
                            //    rspn.ORDER_ID = dtl.ORDER_ID;
                            if (string.IsNullOrWhiteSpace(dtl.SEX) && dtl.ID_TYPE == "0")
                                dtl.SEX = dcrpt.Substring(1, 1) == "1" ? "M" : "F";
                        }
                    }

                    
                    //查詢條件為證件編號
                    if (chkId)
                    {
                        var ds = dtls.Where(x => x.DECODE_ID_NO == func.ID_NO);
                        ORDER_DTL od = ds.FirstOrDefault();
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
                        
                    step = "2";
                    rspn.ORDER_DTL = JArray.FromObject(dtls);
                    rspn.PEOLPLE_CNT = dtls.Count;
                }
                else
                {
                    rspn.PEOLPLE_CNT = 0;
                    rspn.ORDER_DTL = new JArray();
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

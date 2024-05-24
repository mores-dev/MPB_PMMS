using MPB_BLL.COMMON;
using MPB_DAL.KIOSK;
using MPB_Entities.Api;
using MPB_Entities.KIOSK;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_BLL.Api
{
    public class RegisterRedeem_BLL : BLLBase
    {
        KIOSK_CURDAL dal = new KIOSK_CURDAL();
        public APIResponse<RegisterRedeem_Entities> RegisterRedeem(APIRequest request)
        {
            APIResponse<RegisterRedeem_Entities> response = new APIResponse<RegisterRedeem_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            RegisterRedeem_Entities registerRedeem = new RegisterRedeem_Entities();
            
            
            try
            {
                JObject jObj = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObj, ref registerRedeem);
                int grid = 0;
                if(string.IsNullOrWhiteSpace(registerRedeem.ORDER_ID))
                {
                    return response.Error("訂單編號不能為空");
                }

                if (string.IsNullOrWhiteSpace(registerRedeem.SHIPPING_DATE))
                {
                    return response.Error("航班日期不能為空");
                }

                KIOSK_CUR cur = new KIOSK_CUR();
                cur.GR_NO = registerRedeem.ORDER_ID;
                cur.ORDER_DATE = registerRedeem.SHIPPING_DATE;
                cur.ModifyId = "API";
                cur.CLOSE_STATUS = "Y";
                if (dal.Update_Group(cur)<1)
                {
                    return response.Error("更新乘客名單取票狀態失敗 ");
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                //return response.Error(ex.Message);
                return response.Error("更新乘客名單取票狀態錯誤 ");
            }

            return response.OK(registerRedeem);
        }
    }
}

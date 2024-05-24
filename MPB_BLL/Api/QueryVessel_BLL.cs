using MPB_BLL.COMMON;
using MPB_DAL.Api;
using MPB_Entities.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MPB_BLL.Api
{
    public class QueryVessel_BLL : BLLBase
    {
        QueryVessel_DAL _dal = new QueryVessel_DAL();

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public APIResponse<QueryVessel_RSPN_Entities> GetDataMain(APIRequest request)
        {
            APIResponse<QueryVessel_RSPN_Entities> response = new APIResponse<QueryVessel_RSPN_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            QueryVessel_RSPN_Entities rtn = new QueryVessel_RSPN_Entities();
            QueryVessel_FUNC_Entities func = new QueryVessel_FUNC_Entities();
            string step = "0";
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);
                step = "1";
                rtn.SHIPPING_DATE = func.SHIPPING_DATE;

                List<QueryVessel_VESSEL_Entities> vessels = _dal.Select_Vessel(func);
                step = "2";
                rtn.VESSELS = vessels;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                //return response.Error(ex.Message);
                return response.Error("航班取得失敗! Code:" + step);
            }

            return response.OK(rtn);
        }
    }
}

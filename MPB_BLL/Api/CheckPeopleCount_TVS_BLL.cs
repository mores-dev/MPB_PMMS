using MPB_BLL.COMMON;
using MPB_DAL.Api;
using MPB_Entities.Api;
using Newtonsoft.Json.Linq;
using System;

namespace MPB_BLL.Api
{
    public class CheckPeopleCount_TVS_BLL : BLLBase
    {
        CheckPeopleCount_TVS_DAL _dal = new CheckPeopleCount_TVS_DAL();

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public APIResponse<CheckPeopleCount_TVS_RSPN_Entities> GetDataMain(APIRequest request)
        {
            APIResponse<CheckPeopleCount_TVS_RSPN_Entities> response = new APIResponse<CheckPeopleCount_TVS_RSPN_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            CheckPeopleCount_TVS_RSPN_Entities rtn = new CheckPeopleCount_TVS_RSPN_Entities();
            CheckPeopleCount_TVS_FUNC_Entities func = new CheckPeopleCount_TVS_FUNC_Entities();
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);

                //1. 取得該航班占用座位人數
                rtn.PEOPLE_CNT = _dal.Get_People_Count(func);
                //2. 取得該航班幼兒人數
                rtn.BABY_CNT = _dal.Get_Baby_Count(func);
                //3. 其餘回傳參數
                rtn.BUSINESS_DATE = func.BUSINESS_DATE;
                rtn.R_ID = func.R_ID;
                rtn.SC_TIME = func.SC_TIME;
                rtn.V_ID = func.V_ID;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return response.Error(ex.Message);
            }

            return response.OK(rtn);
        }
    }
}

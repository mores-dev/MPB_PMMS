using MPB_BLL.COMMON;
using MPB_DAL.Api;
using MPB_Entities.Api;
using Newtonsoft.Json.Linq;
using System;

namespace MPB_BLL.Api
{
    public class CheckGroupStatus_TVS_BLL : BLLBase
    {
        CheckGroupStatus_TVS_DAL _dal = new CheckGroupStatus_TVS_DAL();

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public APIResponse<CheckGroupStatus_TVS_RSPN_Entities> GetDataMain(APIRequest request)
        {
            APIResponse<CheckGroupStatus_TVS_RSPN_Entities> response = new APIResponse<CheckGroupStatus_TVS_RSPN_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            CheckGroupStatus_TVS_RSPN_Entities rtn = new CheckGroupStatus_TVS_RSPN_Entities();
            CheckGroupStatus_TVS_FUNC_Entities func = new CheckGroupStatus_TVS_FUNC_Entities();
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);

                CheckGroupStatus_TVS_RSPN_Entities queryResult = _dal.Get_Group_Status(func);
                if (queryResult != null)
                    rtn = queryResult;
                else
                    throw new Exception("查無此團客訂單");
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

using MPB_BLL.COMMON;
using MPB_DAL.Api;
using MPB_Entities.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MPB_BLL.Api
{
    public class SyncOperData_TVS_BLL : BLLBase
    {
        SyncOperData_TVS_DAL _dal = new SyncOperData_TVS_DAL();

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public APIResponse<SyncOperData_TVS_RSPN_Entities> GetDataMain(APIRequest request)
        {
            APIResponse<SyncOperData_TVS_RSPN_Entities> response = new APIResponse<SyncOperData_TVS_RSPN_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            SyncOperData_TVS_RSPN_Entities rtn = new SyncOperData_TVS_RSPN_Entities();
            SyncOperData_TVS_FUNC_Entities func = new SyncOperData_TVS_FUNC_Entities();
            string step = "0";
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);
                step = "1";
                //1. 透過帳號取得航商資訊
                //func.C_ID = _dal.Get_C_ID(func);
                //if (string.IsNullOrEmpty(func.C_ID))
                //    throw new Exception("無此使用者!");
                //2. 取得航線資訊
                List<SyncOperData_TVS_Route_Entities> routes = _dal.Select_Route();
                step = "2";
                rtn.ROUTES = routes;
                //3. 取得船隻資訊
                List<SyncOperData_TVS_Vessel_Entities> vessels = _dal.Select_Vessel(func);
                step = "3";
                rtn.VESSELS = vessels;
                //4. 取得航班資訊
                List<SyncOperData_TVS_Schedule_Entities> schedules = _dal.Select_Schedule(func);
                step = "4";
                rtn.SCHEDULES = schedules;
                //5. 取得場站資訊
                List<SyncOperData_TVS_Station_Entities> stations = _dal.Select_Station();
                step = "5";
                rtn.STATIONS = stations;
                //6. 取得航商資訊
                List<SyncOperData_TVS_Company_Entities> companies = _dal.Select_Company(func);
                step = "6";
                rtn.COMPANIES = companies;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                //return response.Error(ex.Message);
                return response.Error("營運資料取得失敗! Code:" + step);
            }

            return response.OK(rtn);
        }
    }
}

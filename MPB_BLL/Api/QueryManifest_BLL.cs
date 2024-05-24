using MPB_BLL.COMMON;
using MPB_DAL.Api;
using MPB_Entities.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MPB_BLL.Api
{
    public class QueryManifest_BLL : BLLBase
    {
        QueryManifest_DAL _dal = new QueryManifest_DAL();

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public APIResponse<QueryManifest_RSPN_Entities> GetDataMain(APIRequest request)
        {
            APIResponse<QueryManifest_RSPN_Entities> response = new APIResponse<QueryManifest_RSPN_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            QueryManifest_RSPN_Entities rtn = new QueryManifest_RSPN_Entities();
            QueryManifest_FUNC_Entities func = new QueryManifest_FUNC_Entities();
            string step = "0";
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);
                step = "1";
                //1. 取主檔資料
                rtn = _dal.Select_Manifest(func);
                step = "2";
                //2. 取明細檔資料
                List<QueryManifestDtl_Entities> dtls = _dal.Select_ManifestDtl(func);
                step = "3";
                rtn.Dtls = dtls;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return response.Error("乘客名冊取得失敗! Code:" + step);
            }

            return response.OK(rtn);
        }
    }
}

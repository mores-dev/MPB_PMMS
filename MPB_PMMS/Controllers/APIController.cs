using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MPB_Entities.Api;
using System;
using System.IO;
using System.Web.Mvc;
using NLog;
using MPB__BLL.Api;
using MPB_BLL.Api;
using MPB_BLL.KIOSK;
using MPB_Entities.KIOSK;
using MPB_BLL.Ossl;
using MPB_Entities.COMMON;
using System.Collections.Generic;
using MPB_Entities.Ossl;
using MPB_BLL.COMMON;

namespace MPB_PMMS.Controllers
{
    public class APIController : JsonNetController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Blank");
        }

        public ActionResult Blank()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(object o)
        {
            Request.InputStream.Position = 0;
            Stream req = Request.InputStream;
            string data = new StreamReader(req).ReadToEnd();
            APIRequest fh = new APIRequest();
            APIResponse<object> fhRtn = new APIResponse<object>();
            try
            {
                var jo = JObject.Parse(data);
                if (jo["FUNC_ID"] == null || jo["SYS_DATE"] == null || jo["ACNT_NO"] == null || jo["FUNC_SIG"] == null)
                {
                    fhRtn = new APIResponse<object>("", "", "");
                    fhRtn.Error("傳入參數錯誤");
                    return Json(fhRtn);
                }

                fh.FUNC_ID = jo["FUNC_ID"].ToString();
                fh.SYS_DATE = jo["SYS_DATE"].ToString();
                fh.ACNT_NO = jo["ACNT_NO"].ToString();
                fh.FUNC_SIG = jo["FUNC_SIG"].ToString();
                if (jo["FUNC_DATA"] == null)
                {
                    fhRtn = new APIResponse<object>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
                    fhRtn.Error("FUNC_DATA is Null");
                }
                else
                {
                    fh.FUNC_DATA = (JObject)jo["FUNC_DATA"];
                }
            }
            catch (Exception e)
            {
                fhRtn = new APIResponse<object>("", "", "");
                fhRtn.Error("Json 格式錯誤");
                return Json(fhRtn);
            }

            //Fix 2023/10/04 CheckMarx原碼檢測: Stored XSS\路徑 2:
            APIRequest fhNew = new APIRequest();
            ModelBLL.Convert(fh, ref fhNew);
            fhRtn = Check_Func_Header(fhNew);

            if (fhRtn.RSPN_CODE == "0")
            {
                return Json(fhRtn);
            }

            object result;
            bool insRec = true;
            int apiType = 0;
            switch (fhNew.FUNC_ID)
            {
                case "QueryRegister":
                    result = QueryRegister(fhNew);
                    break;
                case "QueryRegisterDtl":
                    result = QueryRegisterDtl(fhNew);
                    break;
                case "QueryVessel":
                    result = QueryVessel(fhNew);
                    break;
                case "QueryManifest":
                    result = QueryManifest(fhNew);
                    break;
                case "SyncOperData_TVS":
                    result = SyncOperData_TVS(fhNew);
                    break;
                case "InsertTransData_TVS":
                    result = InsertTransData_TVS(fhNew);
                    break;
                case "CheckGroupStatus_TVS":
                    result = CheckGroupStatus_TVS(fhNew);
                    insRec = false;
                    break;
                case "CheckPeopleCount_TVS":
                    result = CheckPeopleCount_TVS(fhNew);
                    insRec = false;
                    break;
                case "KIOSKInsertPassenger":
                    result = KIOSKInsertPassenger(fhNew);
                    break;
                case "KIOSKDeletePassenger":
                    result = KIOSKDeletePassenger(fhNew);
                    break;
                case "KIOSKUpdatePassenger":
                    result = KIOSKUpdatePassenger(fhNew);
                    break;
                case "KIOSKGetPassengers":
                    result = KIOSKGetPassengers(fhNew);
                    break;
                case "KIOSKInsertOsslTicket":
                    result = KIOSKInsertOsslTicket(fhNew);
                    break;
                case "KIOSKGetOsslTicket":
                    result = KIOSKGetOsslTicket(fhNew);
                    break;
                case "RegisterRedeem":
                    result = RegisterRedeem(fhNew);
                    break;
                case "QueryOrder":
                    result = QueryOrder(fhNew);
                    break;
                default:
                    result = Miss_Func(fhNew);
                    apiType = -1;
                    break;

            }
            if (insRec)
                InsertAPILog(data, fhNew, result, apiType);

            return Json(result);
        }

        private void InsertAPILog(string data, APIRequest fh, object result, int ApiType)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(fh.FUNC_ID))
                {
                    WriteAPILog_BLL bll = new WriteAPILog_BLL();
                    //get DeviceID
                    WriteAPILog_Entities ent = JsonConvert.DeserializeObject<WriteAPILog_Entities>(fh.FUNC_DATA.ToString());
                    ent.DeviceType = ApiType == 0 ? "O" : (ApiType == 1 ? "S" : "");
                    ent.FuncId = fh.FUNC_ID;
                    ent.sJson = data;
                    WriteAPILog_RTN_Entities rtnEnt = JsonConvert.DeserializeObject<WriteAPILog_RTN_Entities>(JsonConvert.SerializeObject(result));
                    ent.RspnCode = string.IsNullOrWhiteSpace(rtnEnt.RSPN_CODE) ? "" : rtnEnt.RSPN_CODE;
                    ent.RspnMsg = string.IsNullOrWhiteSpace(rtnEnt.RSPN_MSG) ? "" : rtnEnt.RSPN_MSG;
                    ent.rJson = JsonConvert.SerializeObject(result);
                    bll.InsertData(ent);
                }
            }
            catch (Exception e)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Error(e, "InsertAPILog Error: " + e.ToString());
                Console.WriteLine(e.ToString());
            }
        }

        private APIResponse<string> Miss_Func(APIRequest fh)
        {
            APIResponse<string> rtn = new APIResponse<string>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            return rtn.Error("無對應功能");
        }

        /// <summary>
        /// 團體預登資料查詢
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<QueryRegister_RSPN> QueryRegister(APIRequest fh)
        {
            QueryRegister_BLL bll = new QueryRegister_BLL();
            return bll.GetQueryRegister(fh);
        }

        /// <summary>
        /// 團體預登明細資料查詢
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<QueryRegisterDtl_RSPN> QueryRegisterDtl(APIRequest fh)
        {
            QueryRegister_BLL bll = new QueryRegister_BLL();
            return bll.GetQueryRegisterDtl(fh);
        }

        /* 航班查詢 */
        private APIResponse<QueryVessel_RSPN_Entities> QueryVessel(APIRequest fh)
        {
            APIResponse<QueryVessel_RSPN_Entities> rtn = new APIResponse<QueryVessel_RSPN_Entities>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            QueryVessel_BLL bll = new QueryVessel_BLL();

            return bll.GetDataMain(fh);
        }

        /* 乘客名冊查詢 */
        private APIResponse<QueryManifest_RSPN_Entities> QueryManifest(APIRequest fh)
        {
            APIResponse<QueryManifest_FUNC_Entities> rtn = new APIResponse<QueryManifest_FUNC_Entities>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            QueryManifest_BLL bll = new QueryManifest_BLL();

            return bll.GetDataMain(fh);
        }

        /* TVS 營運資料同步 */
        private APIResponse<SyncOperData_TVS_RSPN_Entities> SyncOperData_TVS(APIRequest fh)
        {
            APIResponse<SyncOperData_TVS_FUNC_Entities> rtn = new APIResponse<SyncOperData_TVS_FUNC_Entities>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            SyncOperData_TVS_BLL bll = new SyncOperData_TVS_BLL();

            return bll.GetDataMain(fh);
        }

        /* TVS 寫入驗票紀錄 */
        private APIResponse<InsertTransData_TVS_RSPN_Entities> InsertTransData_TVS(APIRequest fh)
        {
            APIResponse<InsertTransData_TVS_RSPN_Entities> rtn = new APIResponse<InsertTransData_TVS_RSPN_Entities>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            InsertTransData_TVS_BLL bll = new InsertTransData_TVS_BLL();

            return bll.InsertTransData(fh);
        }

        /* TVS 計算驗票人數 */
        private APIResponse<CheckPeopleCount_TVS_RSPN_Entities> CheckPeopleCount_TVS(APIRequest fh)
        {
            APIResponse<CheckPeopleCount_TVS_FUNC_Entities> rtn = new APIResponse<CheckPeopleCount_TVS_FUNC_Entities>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            CheckPeopleCount_TVS_BLL bll = new CheckPeopleCount_TVS_BLL();

            return bll.GetDataMain(fh);
        }

        /* TVS 確認團客放行狀態 */
        private APIResponse<CheckGroupStatus_TVS_RSPN_Entities> CheckGroupStatus_TVS(APIRequest fh)
        {
            APIResponse<CheckGroupStatus_TVS_FUNC_Entities> rtn = new APIResponse<CheckGroupStatus_TVS_FUNC_Entities>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            CheckGroupStatus_TVS_BLL bll = new CheckGroupStatus_TVS_BLL();

            return bll.GetDataMain(fh);
        }


        /// <summary>
        /// 新增一筆乘客
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<string> KIOSKInsertPassenger(APIRequest fh)
        {
            KIOSK_CURBLL bll = new KIOSK_CURBLL();
            APIResponse<string> responseModel = bll.InsertRequiredVaild(fh);
            if (responseModel.RSPN_CODE != "1")
                return responseModel;
            return bll.InsertPassenger(fh);
        }

        /// <summary>
        /// 刪除一筆乘客
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<string> KIOSKDeletePassenger(APIRequest fh)
        {
            KIOSK_CURBLL bll = new KIOSK_CURBLL();
            APIResponse<string> responseModel = bll.DeleteRequiredVaild(fh);
            if (responseModel.RSPN_CODE != "1")
                return responseModel;
            return bll.DeletePassenger(fh);
        }

        /// <summary>
        /// 修改一筆乘客
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<string> KIOSKUpdatePassenger(APIRequest fh)
        {
            KIOSK_CURBLL bll = new KIOSK_CURBLL();
            APIResponse<string> responseModel = bll.UpdateRequiredVaild(fh);
            if (responseModel.RSPN_CODE != "1")
                return responseModel;
            return bll.UpdatePassenger(fh);
        }

        /// <summary>
        /// 使用聯絡人電話後三碼與預登編號6碼查團體單全部乘客
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<GroupPassengerModel> KIOSKGetPassengers(APIRequest fh)
        {
            KIOSK_QueryBLL bll = new KIOSK_QueryBLL();
            APIResponse<GroupPassengerModel> responseModel = bll.GroupPreLoginVaild(fh);
            if (responseModel.RSPN_CODE != "1")
                return responseModel;

            return bll.GetGroupByPreNo(fh);
        }

        /// <summary>
        /// 新增修改一筆小蜜蜂購票乘客
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<string> KIOSKInsertOsslTicket(APIRequest fh)
        {
            OsslTicket_BLL bll = new OsslTicket_BLL();
            APIResponse<string> response = new APIResponse<string>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            JObject jObject = (JObject)fh.FUNC_DATA;
            OsslPersonalTicket newData = jObject.ToObject<OsslPersonalTicket>();
            ResponseModel respModel = new ResponseModel();
            List<ErrorModel> errors = new List<ErrorModel>();

            string idNo = "";
            string tkNo = "";
            string msg = "";
            if (!VaildBLL.VaildIDTypeCode(newData.ID_TYPE, out msg))
            {
                return response.Error(msg);
            }


            if (!EnDeCode.DecryptAES256(newData.ID_NO, out idNo))
            {
                return response.Error("證件號碼解密失敗");
            }

            newData.ID_NO = idNo;

            if (!VaildBLL.VaildTicketNo(newData.TK_NO, out tkNo))
            {
                return response.Error("船票號碼格式錯誤");
            }
            //if (!EnDeCode.DecryptAES256(newData.TK_NO, out tkNo))
            //{
            //    return response.Error("船票號碼解密失敗");
            //}

            //newData.TK_NO = tkNo;
            newData.SOURCE = "K";

            //修改 資料庫存在票號
            #region 修改
            OsslPersonalTicket oldData = new OsslPersonalTicket();
            if (!bll.CheckTicketNo(newData.TK_NO, out oldData))
            {
                errors = bll.CheckUpdatePersonalTicket(newData, oldData);
                if (errors.Count > 0)
                    return response.Error(errors[0].msg);

                respModel = bll.Update_PersonalTicket(newData, oldData);

                if (respModel.code != "00")
                    return response.Error(respModel.msg);

                return response.OK("");
            }
            #endregion

            //新增 資料庫內無此票號
            #region 新增
            errors = bll.CheckPersonalTicket(newData);
            if (errors.Count > 0)
                return response.Error(errors[0].msg);
            respModel = bll.Insert_PersonalTicket(newData);
            if (respModel.code != "00")
                return response.Error(respModel.msg);
            return response.OK("");
            #endregion

        }

        /// <summary>
        /// 查詢一筆小蜜蜂購票乘客 
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<OsslPersonalTicketResponse> KIOSKGetOsslTicket(APIRequest fh)
        {
            OsslTicket_BLL bll = new OsslTicket_BLL();
            APIResponse<OsslPersonalTicketResponse> response = new APIResponse<OsslPersonalTicketResponse>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            JObject jObject = (JObject)fh.FUNC_DATA;
            OsslPersonalTicketRequest requestData = jObject.ToObject<OsslPersonalTicketRequest>();
            OsslPersonalTicketResponse resultData = new OsslPersonalTicketResponse();
            OsslPersonalTicket ticket = new OsslPersonalTicket();
            ResponseModel respModel = new ResponseModel();

            //string tkNo = requestData.TK_NO;
            //if (!EnDeCode.DecryptAES256(requestData.TK_NO, out tkNo))
            //    return response.Error("船票號碼解密失敗");
            //requestData.TK_NO = tkNo;

            if (bll.CheckTicketNo(requestData.TK_NO, out ticket))
                return response.Error("查無此船票資料");

            resultData.ID_NO = EnDeCode.EncryptAES256(ticket.ID_NO);
            resultData.TK_NO = ticket.TK_NO;
            resultData.NAME = ticket.NAME;
            resultData.ID_TYPE = ticket.ID_TYPE;
            resultData.BIRTHDAY = ticket.BIRTHDAY;
            return response.OK(resultData);
        }

        /// <summary>
        /// 民宿預登取票
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<RegisterRedeem_Entities> RegisterRedeem(APIRequest fh)
        {
            RegisterRedeem_BLL bll = new RegisterRedeem_BLL();
            return bll.RegisterRedeem(fh);
        }

        private APIResponse<QueryOrder_RSPN_Entities> QueryOrder(APIRequest fh)
        {
            QueryOrder_BLL bll = new QueryOrder_BLL();
            return bll.GetQueryOrder(fh);
        }

        /// <summary>
        /// 針對有FUNC_SIG的API進行檢查
        /// </summary>
        /// <param name="fh"></param>
        /// <returns></returns>
        private APIResponse<object> Check_Func_Header(APIRequest fh)
        {
            APIResponse<object> fhRtn = new APIResponse<object>(fh.FUNC_ID, fh.SYS_DATE, fh.ACNT_NO);
            //檢查系統時間
            DateTime dt;
            if (!DateTime.TryParseExact(fh.SYS_DATE, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out dt))
                return fhRtn.Error("系統時間錯誤!");

            if (dt == null || DateTime.Compare(dt, DateTime.Now.AddHours(-1)) < 0 || DateTime.Compare(dt, DateTime.Now.AddHours(1)) > 0)
                return fhRtn.Error("系統時間錯誤!");

            //檢查簽章
            if (!QueryRegister_BLL.CheckSign(fh))
                return fhRtn.Error("API認證失敗!");

            return fhRtn.OK("");

        }
    }

}


using MPB_BLL.COMMON;
using MPB_DAL.KIOSK;
using MPB_Entities.Api;
using MPB_Entities.COMMON;
using MPB_Entities.KIOSK;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_BLL.KIOSK
{
    public class KIOSK_QueryBLL : BLLBase
    {
        private KIOSK_QueryDAL dal = new KIOSK_QueryDAL();

        public ResponseModel<GroupDecord> CheckGroup(GroupPassengerDecordParam decordPar)
        {
            ResponseModel<GroupDecord> responseModel = new ResponseModel<GroupDecord>();
            GroupDecord decord = dal.GetGroup(decordPar);
            if (decord == null)
            {
                logger.Warn(decordPar.GR_NO + " 查無此團體單");
                return responseModel.Error("10", "查無此團體單");
            }

            if(decord.THREE_CODE != decordPar.CONTACT_PHONE)
            {
                logger.Warn(decordPar.GR_NO + " 驗證碼(3碼)錯誤");
                return responseModel.Error("10", "驗證碼(3碼)錯誤");
            }
                

    
            DateTime now = DateTime.Now;
            DateTime today = new DateTime(now.Year, now.Month, now.Day);
            if (decord.ORDER_DATE.Subtract(today).Ticks < 0)
            {
                logger.Warn(decordPar.GR_NO + " 去程日期小於今日");
                return responseModel.Error("13", "去程日期小於今日");
            }
                

            if (decord.DTL_COUNT <= 0)
            {
                logger.Warn(decordPar.GR_NO + " 此團體單無乘客");
                return responseModel.Error("14", "此團體單無乘客");
            }
                

            if (string.IsNullOrWhiteSpace(decord.GR_ID))
            {
                logger.Warn(decordPar.GR_NO + " 此團體單無乘客");
                return responseModel.Error("11", "查無此團體編號");
            }
                

            return responseModel.OK(decord);
        }

        public ResponseModel<GroupDecord> CheckPassengerCount(GroupPassengerDecordParam decordPar)
        {
            ResponseModel<GroupDecord> responseModel = new ResponseModel<GroupDecord>();
            GroupDecord decord = dal.GetGroup(decordPar);
            if (decord == null)
            {
                logger.Warn(decordPar.GR_NO + " 查無此團體單");
                return responseModel.Error("10", "查無此團體單");
            }

            if (decord.THREE_CODE != decordPar.CONTACT_PHONE)
            {
                logger.Warn(decordPar.GR_NO + " 驗證碼(3碼)錯誤");
                return responseModel.Error("10", "驗證碼(3碼)錯誤");
            }

            //驗證多人一票類型與一人一票類型對應使用者類別是否正確
            if(decord.GA_TYPE == "H")
            {
                logger.Warn(decordPar.GR_NO + " 請使用民宿取票功能");
                return responseModel.Error("15", "請使用民宿取票功能");
            }

            //CodeListBLL clbll = new CodeListBLL();
            //List<CodeName> funcType = clbll.GetCodeList("000", "SYS01008");
            //var ftStr = funcType.Where(x => x.Code == decordPar.FuncCode).FirstOrDefault();
            //if (ftStr == null)
            //{
            //    logger.Warn(" Codelist查無SYS01008");
            //    return responseModel.Error("15", "Codelist查無SYS01008");
            //}
            //if (!ftStr.Name.Contains(decord.GA_TYPE))
            //{
            //    string msg = "";
            //    if (ftStr.Code == "S")
            //    {
            //        msg = "此團體名單請使用團客報到功能";
            //    }
            //    else
            //    {
            //        msg = "此團體名單請使用民宿取票功能";
            //    }
            //    logger.Warn(msg);
            //    return responseModel.Error("15", msg);
            //}
          
            if(decord.EDIT_LOCK == "Y")
            {
                logger.Warn(decord.EDIT_LOCK + "此團體名單已放行");
                return responseModel.Error("10", "此團體名單已放行");
            }

            //int defaultPassengerCount = int.Parse(ConfigurationManager.AppSettings["defaultPassengerCount"].ToString());
            //if (decord.DTL_COUNT < defaultPassengerCount)
            //{
            //    logger.Warn(decordPar.GR_NO + "人數低於" + defaultPassengerCount.ToString() + "人請至櫃台處理");
            //    return responseModel.Error("12", "人數低於" + defaultPassengerCount.ToString() + "人請至櫃台處理");
            //}
            string defaultPassengerCount = ConfigurationManager.AppSettings["defaultPassengerCount"].ToString();
            if (decord.USE_KIOSK == "N")
            {
                logger.Warn(decordPar.GR_NO + "人數低於" + defaultPassengerCount.ToString() + "人請至櫃台處理");
                return responseModel.Error("12", "人數低於" + defaultPassengerCount.ToString() + "人請至櫃台處理");
            }

            return responseModel.OK(decord);
        }

        public GroupPassengerDecord GetPassenger(GroupPassengerDecordParam decordPar)
        {
            List<GroupPassengerDecord> list = dal.GetGroupDtl(decordPar);
            foreach(GroupPassengerDecord p in list)
            {
                string idno1 = "";
                string idno2 = "";
                EnDeCode.DecryptAES256(p.ID_NO, out idno1);
                EnDeCode.DecryptAES256(decordPar.ID_NO, out idno2);
                if (idno1 == idno2)
                {
                    return p;
                }
            }
            return null;
        }

        public List<PassengersModel> GetPassengers(GroupPassengerDecordParam decordPar)
        {
            List<GroupDecordDtl> decordDtls = dal.GetPassengers(decordPar);
            List<PassengersModel> passengers = new List<PassengersModel>();
            foreach (GroupDecordDtl dtl in decordDtls)
            {
                PassengersModel passengersModel = new PassengersModel();
                passengersModel.rowSeq = dtl.SEQNO;
                //passengersModel.idNo = EnDeCode.EncryptAES256(dtl.ID_NO);
                passengersModel.idNo = dtl.ID_NO;
                passengersModel.idTypeStr = dtl.ID_TYPE;
                passengersModel.name = dtl.NAME;
                passengersModel.birthday = dtl.BIRTHDAY;
                passengers.Add(passengersModel);
            }
            return passengers;
        }

        /// <summary>
        /// 預登號碼+手機後三碼 取團體單與乘客資料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public APIResponse<GroupPassengerModel> GetGroupByPreNo(APIRequest request)
        {
            APIResponse<GroupPassengerModel> responseModel = new APIResponse<GroupPassengerModel>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            JObject jObject = (JObject)request.FUNC_DATA;
            GroupPreLoginModel groupPreLogin = jObject.ToObject<GroupPreLoginModel>();
            GroupPassengerDecordParam decordPar = new GroupPassengerDecordParam();
            decordPar.CONTACT_PHONE = groupPreLogin.contactTel;
            decordPar.GR_NO = groupPreLogin.prNo;
            //decordPar.FuncCode = groupPreLogin.FuncCode;
            ResponseModel<GroupDecord> gDResponseModel = CheckGroup(decordPar);
            if(gDResponseModel.code != "00")
                return responseModel.Error(gDResponseModel.msg);
            gDResponseModel = CheckPassengerCount(decordPar);
            if (gDResponseModel.code != "00")
                return responseModel.Error(gDResponseModel.msg);
            GroupDecord decord = gDResponseModel.data;
            decordPar.GR_ID = decord.GR_ID;
            GroupModel group = new GroupModel();
            group.count = decord.DTL_COUNT;
            group.orderDate = decord.ORDER_DATE.ToString("yyyy/MM/dd");

            GroupPassengerModel groupPassenger = new GroupPassengerModel();
            groupPassenger.group = group;

            List<PassengersModel> passengers = GetPassengers(decordPar);
            if (passengers.Count <= 0)
                return responseModel.Error("查無旅客");
            groupPassenger.passengers = passengers;

            return responseModel.OK(groupPassenger);
        }

        /// <summary>
        /// 預登號碼+手機後三碼驗證
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public APIResponse<GroupPassengerModel> GroupPreLoginVaild(APIRequest request)
        {
            APIResponse<GroupPassengerModel> responseModel = new APIResponse<GroupPassengerModel>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);

            JObject jObject = (JObject)request.FUNC_DATA;
            GroupPreLoginModel groupPreLogin = jObject.ToObject<GroupPreLoginModel>();
            string res = "";
            if (!VaildBLL.VaildRequired(groupPreLogin.prNo, out res))
            {
                logger.Warn("prNo" + res);
                return responseModel.Error(res);
            }
                

            if (!VaildBLL.VaildSPChar(groupPreLogin.prNo, out res))
            {
                logger.Warn("prNo" + res);
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildRequired(groupPreLogin.contactTel, out res))
            {
                logger.Warn("contactTel" + res);
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildSPChar(groupPreLogin.contactTel, out res))
            {
                logger.Warn("contactTel" + res);
                return responseModel.Error(res);
            }

            return responseModel.OK(null);
        }
    }
}

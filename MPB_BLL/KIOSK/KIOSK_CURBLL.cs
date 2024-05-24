using DataAccessUtility;
using MPB_BLL.COMMON;
using MPB_DAL;
using MPB_DAL.KIOSK;
using MPB_Entities.Api;
using MPB_Entities.COMMON;
using MPB_Entities.KIOSK;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_BLL.KIOSK
{
    public class KIOSK_CURBLL : BLLBase
    {
        private KIOSK_CURDAL dal = new KIOSK_CURDAL();
        private KIOSK_QueryDAL qryDal = new KIOSK_QueryDAL();

        /// <summary>
        /// 新增一筆旅客
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public APIResponse<string> InsertPassenger(APIRequest request)
        {
            APIResponse<string> response = new APIResponse<string>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            DbManager db = DbManager.GetInstance();

            JObject jObject = (JObject)request.FUNC_DATA;
            InsertPassenger passenger = jObject.ToObject<InsertPassenger>();
            //string idNo = "";
            //if(!EnDeCode.DecryptAES256(passenger.idNo, out idNo))
            //{
            //    logger.Error("身分證字號解密失敗 : "+passenger.idNo);
            //    return response.Error("身分證字號解密失敗");
            //}

            KIOSK_CURDAL dalTra = new KIOSK_CURDAL(db);
            CodeListBLL clbll = new CodeListBLL();

            GroupPassengerDecordParam decordPar = new GroupPassengerDecordParam(passenger.prNo, passenger.contactTel, passenger.idTypeStr, passenger.idNo);

            KIOSK_QueryBLL queryBLL = new KIOSK_QueryBLL();
            ResponseModel<GroupDecord> gdResponseModel = queryBLL.CheckGroup(decordPar);
            if (gdResponseModel.code != "00")
                return response.Error(gdResponseModel.msg);

            decordPar.GR_ID = gdResponseModel.data.GR_ID;
            GroupPassengerDecord passengerDecord = queryBLL.GetPassenger(decordPar);
            if (passengerDecord != null)
            {
                logger.Warn("重複證件 GRNO : " + passenger.prNo);
                return response.Error("重複證件");
            }
                

            KIOSK_CUR insertModel = new KIOSK_CUR(decordPar.GR_ID, passenger.name, passenger.idTypeStr, passenger.idNo, passenger.birthday, passenger.DEVICE_ID);

            using (ITransaction scpoe = db.GetTransaction())
            {
                insertModel.SEQNO = dalTra.Get_NewSeqNo(insertModel);
                insertModel.GRD_ID = dalTra.Insert_Passenger(insertModel);
                if (insertModel.GRD_ID <= 0)
                {
                    logger.Error("乘客新增失敗 GRNO : " + passenger.prNo);
                    return response.Error("乘客新增失敗");
                }
                    
                insertModel.DTL_COUNT = dalTra.Update_DtlCount(insertModel);
                if (insertModel.DTL_COUNT <= 0)
                {
                    logger.Error("更新乘客筆數失敗 GRNO : " + passenger.prNo);
                    return response.Error("更新乘客筆數失敗");
                }
                    
                scpoe.Complete();
            }
            return response.OK("InsertPassenger Success");
        }

        /// <summary>
        /// 刪除一筆旅客
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public APIResponse<string> DeletePassenger(APIRequest request)
        {
            APIResponse<string> response = new APIResponse<string>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            DbManager db = DbManager.GetInstance();

            JObject jObject = (JObject)request.FUNC_DATA;
            DeletePassenger passenger = jObject.ToObject<DeletePassenger>();
            //string idNo = "";
            // if(!EnDeCode.DecryptAES256(passenger.idNo, out idNo))
            //{
            //    logger.Error("身分證字號解密失敗 : "+passenger.idNo);
            //    return response.Error("身分證字號解密失敗");
            //}
            KIOSK_CURDAL dalTra = new KIOSK_CURDAL(db);
            CodeListBLL clbll = new CodeListBLL();

            GroupPassengerDecordParam decordPar = new GroupPassengerDecordParam(passenger.prNo, passenger.contactTel, passenger.idTypeStr, passenger.idNo);

            KIOSK_QueryBLL queryBLL = new KIOSK_QueryBLL();
            ResponseModel<GroupDecord> gdResponseModel = queryBLL.CheckGroup(decordPar);
            if (gdResponseModel.code != "00")
                return response.Error(gdResponseModel.msg);

            decordPar.GR_ID = gdResponseModel.data.GR_ID;
            GroupPassengerDecord passengerDecord = queryBLL.GetPassenger(decordPar);
            if (passengerDecord == null)
            {
                logger.Warn("無此乘客 GRNO : " + passenger.prNo);
                return response.Error("無此乘客");
            }
                

            KIOSK_CUR deleteModel = new KIOSK_CUR(passengerDecord.GRD_ID, decordPar.GR_ID, passenger.DEVICE_ID);

            int eff = -1;
            using (ITransaction scpoe = db.GetTransaction())
            {
                eff = dalTra.Delete_Passenger(deleteModel);
                if (eff <= 0)
                {
                    logger.Error("刪除乘客失敗 GRNO : " + passenger.prNo);
                    return response.Error("刪除乘客失敗");
                }
                    

                deleteModel.DTL_COUNT = dalTra.Update_DtlCount(deleteModel);
                if (deleteModel.DTL_COUNT <= 0)
                {
                    logger.Error("更新乘客筆數失敗 GRNO : " + passenger.prNo);
                    return response.Error("更新乘客筆數失敗");
                }
                    
                scpoe.Complete();
            }
            return response.OK("DeletePassenger Success");
        }

        /// <summary>
        /// 修改一筆乘客
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public APIResponse<string> UpdatePassenger(APIRequest request)
        {
            APIResponse<string> response = new APIResponse<string>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            DbManager db = DbManager.GetInstance();

            JObject jObject = (JObject)request.FUNC_DATA;
            UpdatePassenger passenger = jObject.ToObject<UpdatePassenger>();
            //string idNo = "";
            //string newIdno = "";
            //if (!EnDeCode.DecryptAES256(passenger.idNo, out idNo))
            //{
            //    logger.Error("身分證字號解密失敗 : " + passenger.idNo);
            //    return response.Error("身分證字號解密失敗");
            //}

            //if (!string.IsNullOrWhiteSpace(passenger.newIdNo))
            //{
            //    if (!EnDeCode.DecryptAES256(passenger.newIdNo, out newIdno))
            //    {
            //        logger.Error("欲修改身分證字號解密失敗 : " + passenger.newIdNo);
            //        return response.Error("欲修改身分證字號解密失敗");
            //    }
            //}
          

            KIOSK_CURDAL dalTra = new KIOSK_CURDAL(db);
            CodeListBLL clbll = new CodeListBLL();
            GroupPassengerDecordParam decordPar = new GroupPassengerDecordParam(passenger.prNo, passenger.contactTel, passenger.idTypeStr, passenger.idNo);

            KIOSK_QueryBLL queryBLL = new KIOSK_QueryBLL();
            ResponseModel<GroupDecord> gdResponseModel = queryBLL.CheckGroup(decordPar);
            if (gdResponseModel.code != "00")
                return response.Error(gdResponseModel.code, gdResponseModel.msg);
            //檢查原乘客資料
            decordPar.GR_ID = gdResponseModel.data.GR_ID;
            GroupPassengerDecord passengerDecord = queryBLL.GetPassenger(decordPar);
            if (passengerDecord == null)
            {
                logger.Warn("無此乘客 GRNO : " + passenger.prNo);
                return response.Error("無此乘客");
            }
               

            //檢查新輸入乘客資料
            if(!string.IsNullOrEmpty(passenger.newIdNo))
                decordPar.ID_NO = passenger.newIdNo;

            if (!string.IsNullOrEmpty(passenger.newIdTypeStr))
                decordPar.ID_TYPE = passenger.newIdTypeStr;

            if (!string.IsNullOrEmpty(passenger.newIdNo)|| !string.IsNullOrEmpty(passenger.newIdTypeStr))
            {
                GroupPassengerDecord nPassengerDecord = queryBLL.GetPassenger(decordPar);
                if (nPassengerDecord != null)
                { 
                    logger.Warn("重複證件 GRNO : " + passenger.prNo);
                    return response.Error("重複證件");
                }
                    
            }
                
            KIOSK_CUR updateModel = new KIOSK_CUR(passengerDecord.GRD_ID, decordPar.GR_ID, passenger.name, passenger.newIdTypeStr, passenger.newIdNo, passenger.birthday, passenger.DEVICE_ID);

            int eff = 0;
            using (ITransaction scpoe = db.GetTransaction())
            {
                eff = dalTra.Update_Passenger(updateModel);
                if (eff <= 0)
                {
                    logger.Error("乘客修改失敗 GRNO : " + passenger.prNo);
                    return response.Error("乘客修改失敗");
                }
                   
                scpoe.Complete();
            }
            return response.OK("UpdatePassenger Success");
        }

        /// <summary>
        /// 基本欄位驗證
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public APIResponse<string> BaseVaild(BasePassenger passenger)
        {
            APIResponse<string> responseModel = new APIResponse<string>();
            string res = "";
            if (!VaildBLL.VaildRequired(passenger.DEVICE_ID, out res))
            {
                logger.Warn("DEVICE_ID" + res);
                return responseModel.Error(res);
            }
                

            if (!VaildBLL.VaildSPChar(passenger.DEVICE_ID, out res))
            {
                logger.Warn("DEVICE_ID" + res); 
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildRequired(passenger.prNo, out res))
            {
                logger.Warn("prNo" + res); 
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildSPChar(passenger.prNo, out res))
            {
                logger.Warn("prNo" + res); 
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildRequired(passenger.contactTel, out res))
            {
                logger.Warn("contactTel" + res); 
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildSPChar(passenger.contactTel, out res))
             {
                logger.Warn("contactTel" + res); 
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildRequired(passenger.idTypeStr, out res))
            {
                logger.Warn("idTypeStr" + res); 
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildSPChar(passenger.idTypeStr, out res))
            {
                logger.Warn("idTypeStr" + res); 
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildIDTypeCode(passenger.idTypeStr, out res))
            {
                logger.Warn("idTypeStr" + res); 
                return responseModel.Error(res);
            }

            responseModel = IdDecode(ref passenger);
            if (responseModel.RSPN_CODE != "1")
                return responseModel;

            if (passenger.idTypeStr == "0")
            {
                if (!VaildBLL.VaildID(passenger.idNo, out res))
                {
                    logger.Warn("idNo" + res); 
                    return responseModel.Error(res);
                }
            }
            else
            {
                if (!VaildBLL.VaildRequired(passenger.idNo, out res))
                {
                    logger.Warn("idNo" + res); 
                    return responseModel.Error(res);
                }
                if (!VaildBLL.VaildSPChar(passenger.idNo, out res))
                {
                    logger.Warn("idNo" + res); 
                    return responseModel.Error(res);
                }
            }

            return responseModel.OK("");
        }

        /// <summary>
        /// 新增驗證
        /// </summary>
        /// <param name="rqPassenger"></param>
        /// <returns></returns>
        public APIResponse<string> InsertRequiredVaild(APIRequest request)
        {
            APIResponse<string> responseModel = new APIResponse<string>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            
            JObject jObject = (JObject)request.FUNC_DATA;
            InsertPassenger passenger = jObject.ToObject<InsertPassenger>();

            responseModel = BaseVaild(passenger);
            if (responseModel.RSPN_CODE != "1")
                return responseModel;

            string res = "";

            if (!VaildBLL.VaildRequired(passenger.name, out res))
            {
                logger.Warn("name" + res);
                return responseModel.Error(res);
            }


            if (!VaildBLL.VaildSPChar(passenger.name, out res))
            {
                logger.Warn("name" + res);
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildBirthday(passenger.birthday, out res))
            {
                logger.Warn("birthday" + res);
                return responseModel.Error(res);
            }

            return responseModel.OK("");
        }

        /// <summary>
        /// 刪除驗證
        /// </summary>
        /// <param name="rqPassenger"></param>
        /// <returns></returns>
        public APIResponse<string> DeleteRequiredVaild(APIRequest request)
        {
            APIResponse<string> responseModel = new APIResponse<string>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            JObject jObject = (JObject)request.FUNC_DATA;
            DeletePassenger passenger = jObject.ToObject<DeletePassenger>();            
            return BaseVaild(passenger);
           
        }

        /// <summary>
        /// 修改驗證
        /// </summary>
        /// <param name="rqPassenger"></param>
        /// <returns></returns>
        public APIResponse<string> UpdateRequiredVaild(APIRequest request)
        {
            APIResponse<string> responseModel = new APIResponse<string>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            JObject jObject = (JObject)request.FUNC_DATA;
            UpdatePassenger passenger = jObject.ToObject<UpdatePassenger>();
            responseModel = BaseVaild(passenger);
            if (responseModel.RSPN_CODE != "1")
                return responseModel;

            string res = "";

            if (!VaildBLL.VaildSPChar(passenger.newIdTypeStr, out res))
            {
                logger.Warn("newIdTypeStr" + res);
                return responseModel.Error(res);
            }

            if (!VaildBLL.VaildIDTypeCode(passenger.newIdTypeStr, out res))
            {
                logger.Warn("newIdTypeStr" + res);
                return responseModel.Error(res);
            }

            if (!string.IsNullOrWhiteSpace(passenger.birthday))
            {
                if (!VaildBLL.VaildBirthday(passenger.birthday, out res))
                {
                    logger.Warn("birthday" + res);
                    return responseModel.Error(res);
                }
            }
            
            if (!string.IsNullOrWhiteSpace(passenger.newIdNo))
            {
                responseModel = NewIdDecode(ref passenger);
                if (responseModel.RSPN_CODE != "1")
                    return responseModel;

                if (!string.IsNullOrWhiteSpace(passenger.newIdTypeStr))
                {
                    if (passenger.newIdTypeStr == "0")
                    {
                        if (!VaildBLL.VaildID(passenger.newIdNo, out res))
                        {
                            logger.Warn("newIdNo" + res);
                            return responseModel.Error(res);
                        }
                    }
                    else
                    {
                        if (!VaildBLL.VaildRequired(passenger.newIdNo, out res))
                        {
                            logger.Warn("newIdNo" + res);
                            return responseModel.Error(res);
                        }

                        if (!VaildBLL.VaildSPChar(passenger.newIdNo, out res))
                        {
                            logger.Warn("newIdNo" + res);
                            return responseModel.Error(res);
                        }
                    }

                }
                else
                {
                    if (passenger.idTypeStr == "0")
                    {
                        if (!VaildBLL.VaildID(passenger.newIdNo, out res))
                        {
                            logger.Warn("newIdNo" + res);
                            return responseModel.Error(res);
                        }
                    }
                    else
                    {
                        if (!VaildBLL.VaildRequired(passenger.newIdNo, out res))
                        {
                            logger.Warn("newIdNo" + res);
                            return responseModel.Error(res);
                        }

                        if (!VaildBLL.VaildSPChar(passenger.newIdNo, out res))
                        {
                            logger.Warn("newIdNo" + res);
                            return responseModel.Error(res);
                        }
                    }
                }
            }
            return responseModel.OK("");
        }

        /// <summary>
        /// ID解密
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public APIResponse<string> IdDecode(ref BasePassenger passenger)
        {
            APIResponse<string> responseModel = new APIResponse<string>();

            if (!string.IsNullOrWhiteSpace(passenger.idNo))
            {
                string idno = "";
                EnDeCode code = new EnDeCode();
                if(!EnDeCode.DecryptAES256(passenger.idNo, out idno))
                {
                    logger.Warn("idNo" + "解密失敗");
                    return responseModel.Error("0", "解密失敗");
                }
                passenger.idNo = idno;
            }
            return responseModel.OK(passenger.idNo);
        }

        /// <summary>
        /// 新ID解密
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public APIResponse<string> NewIdDecode(ref UpdatePassenger passenger)
        {
            APIResponse<string> responseModel = new APIResponse<string>();
            string idno = "";
            EnDeCode code = new EnDeCode();
            if (!EnDeCode.DecryptAES256(passenger.newIdNo, out idno))
            {
                logger.Warn("newIdNo" + "解密失敗");
                return responseModel.Error("0", "解密失敗");
            }
            passenger.newIdNo = idno;
            return responseModel.OK(passenger.newIdNo);
        }
    }
}

using MPB_BLL.COMMON;
using MPB_DAL.Ossl;
using MPB_Entities.COMMON;
using MPB_Entities.Ossl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_BLL.Ossl
{
    public class OsslTicket_BLL : BLLBase
    {
        /// <summary>
        /// 檢核輸入
        /// </summary>
        /// <param name="passenges"></param>
        /// <returns></returns>
        public List<ErrorModel> CheckPersonalTicket(OsslPersonalTicket qc)
        {
            List<ErrorModel> errors = new List<ErrorModel>();
            ErrorModel error = new ErrorModel();
            ResponseModel model = new ResponseModel();

            string errMsg = "";
            if (!VaildBLL.VaildName(qc.NAME, out errMsg))
            {
                errors.Add(new ErrorModel("#NAME", errMsg));
            }

            if (!VaildBLL.VaildIDTypeCode(qc.ID_TYPE, out errMsg))
            {
                errors.Add(new ErrorModel("#ID_TYPE", errMsg));
            }
            else
            {
                if (qc.ID_TYPE == "0")
                {
                    if (!VaildBLL.VaildRequired(qc.ID_NO, out errMsg))
                    {
                        errors.Add(new ErrorModel("#ID_NO", "身分證字號" + errMsg));
                    }

                    if (!VaildBLL.VaildID(qc.ID_NO, out errMsg))
                    {
                        errors.Add(new ErrorModel("#ID_NO",   errMsg));
                    }
                }
                else
                {
                    if (!VaildBLL.VaildName(qc.ID_NO, out errMsg))
                    {
                        errors.Add(new ErrorModel("#ID_NO", errMsg));
                    }
                }
            }

            if (!VaildBLL.VaildTel(qc.PHONE, out errMsg))
            {
                errors.Add(new ErrorModel("#PHONE", errMsg));
            }

            if (!VaildBLL.VaildBirthday(qc.BIRTHDAY, out errMsg))
            {
                errors.Add(new ErrorModel("#BIRTHDAY", errMsg));
            }
            if(errors.Count()>0)
            {
                foreach(ErrorModel em in errors)
                {
                    logger.Warn(em.key + " : " + em.msg);
                }
            }
            return errors;
        }

        public List<ErrorModel> CheckUpdatePersonalTicket(OsslPersonalTicket qc, OsslPersonalTicket oldData)
        {
            List<ErrorModel> errors = new List<ErrorModel>();
            ErrorModel error = new ErrorModel();
            ResponseModel model = new ResponseModel();
            
            string errMsg = "";
     
            if (qc.NAME != DataMask.MaskValue(oldData.NAME, MaskType.Name))
            {
                if (!VaildBLL.VaildName(qc.NAME, out errMsg))
                {
                    errors.Add(new ErrorModel("#NAME", errMsg));
                }
            }
            

            if (!VaildBLL.VaildIDTypeCode(qc.ID_TYPE, out errMsg))
            {
                errors.Add(new ErrorModel("#ID_TYPE", errMsg));
            }
            else
            {
                
                if (qc.ID_TYPE == "0")
                {
                    //if (qc.ID_NO != oldData.ID_NO)
                    //{
                    //    if (!VaildBLL.VaildID(qc.ID_NO, out errMsg))
                    //    {
                    //        errors.Add(new ErrorModel("#ID_NO", errMsg));
                    //    }
                    //}
                    if (!VaildBLL.VaildRequired(qc.ID_NO, out errMsg))
                    {
                        errors.Add(new ErrorModel("#ID_NO", "身分證字號"+errMsg));
                    }

                    if (!VaildBLL.VaildID(qc.ID_NO, out errMsg))
                    {
                        errors.Add(new ErrorModel("#ID_NO", errMsg));
                    }

                }
                else
                {
                    //if (qc.ID_NO != oldData.ID_NO)
                    //{
                    //    if (!VaildBLL.VaildName(qc.ID_NO, out errMsg))
                    //    {
                    //        errors.Add(new ErrorModel("#ID_NO", errMsg));
                    //    }
                    //}

                    if (!VaildBLL.VaildName(qc.ID_NO, out errMsg))
                    {
                        errors.Add(new ErrorModel("#ID_NO", errMsg));
                    }
                }
            }

            if (qc.PHONE != oldData.PHONE)
            {
                if (!VaildBLL.VaildTel(qc.PHONE, out errMsg))
                {
                    errors.Add(new ErrorModel("#PHONE", errMsg));
                }
            }
                

            if (!VaildBLL.VaildBirthday(qc.BIRTHDAY, out errMsg))
            {
                errors.Add(new ErrorModel("#BIRTHDAY", errMsg));
            }


            if(errors.Count()>0)
            {
                foreach(ErrorModel em in errors)
                {
                    logger.Warn(em.key + " : " + em.msg);
                }
            }
            return errors;
        }

        public bool CheckTicketNo(string tkNo, out OsslPersonalTicket osslPersonal)
        {
            OsslTicket_DAL dll = new OsslTicket_DAL();
            osslPersonal = dll.Select_PersonalTicket(tkNo);
            if(osslPersonal != null)
            {
                string idno = "";
                EnDeCode.DecryptAES256(osslPersonal.ID_NO, out idno);
                osslPersonal.ID_NO = idno;
            }            
            return osslPersonal == null;
        }

        public ResponseModel Insert_PersonalTicket(OsslPersonalTicket qc)
        {
            ResponseModel responseModel = new ResponseModel();
            OsslPersonalTicketEntites entites = new OsslPersonalTicketEntites();
            ModelBLL.Convert(qc, ref entites);
            entites.CreateId = qc.DEVICE_ID;
            entites.ModifyId = qc.DEVICE_ID;
            //entites.SOURCE = "M";
            entites.ID_NO = EnDeCode.EncryptAES256(entites.ID_NO);
            
            entites.C_ID = VendorBLL.GetCIdByEng(qc.TK_NO.Substring(0, 2));

            OsslTicket_DAL dll = new OsslTicket_DAL();
            int res = dll.Insert_PersonalTicket(entites);
            if (res < 1)
            {
                logger.Error(qc.TK_NO +" : 新增失敗");
                return responseModel.Error("99", "新增失敗");
            }
            
            return responseModel.Ok(res);
        }

        public ResponseModel Update_PersonalTicket(OsslPersonalTicket newData, OsslPersonalTicket oldData)
        {
            ResponseModel responseModel = new ResponseModel();
            OsslPersonalTicketEntites entites = new OsslPersonalTicketEntites();
            if (newData.BIRTHDAY != oldData.BIRTHDAY)
                entites.BIRTHDAY = newData.BIRTHDAY;
            if (newData.SOURCE != oldData.SOURCE)
                entites.SOURCE = newData.SOURCE;
            if (newData.ID_NO != oldData.ID_NO)
                entites.ID_NO = EnDeCode.EncryptAES256(newData.ID_NO);
            if (newData.ID_TYPE != oldData.ID_TYPE)
                entites.ID_TYPE = newData.ID_TYPE;
            if (newData.NAME != oldData.NAME)
                entites.NAME = newData.NAME;
            if (newData.PHONE != oldData.PHONE)
                entites.PHONE = newData.PHONE;

            entites.TK_NO = oldData.TK_NO;
            entites.ModifyId = newData.DEVICE_ID;
            OsslTicket_DAL dll = new OsslTicket_DAL();
            int res = dll.Update_PersonalTicket(entites);
            if (res < 1)
            {
                logger.Error(oldData.TK_NO + " : 修改失敗");
                return responseModel.Error("99", "修改失敗");
            }
                

            return responseModel.Ok(res);
        }

        
    }
}

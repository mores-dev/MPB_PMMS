using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MPB_DAL;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;
using DataAccessUtility;

namespace MPB_BLL.FCM
{
    public class FCMVessel_SaveBLL : BLLBase
    {
        public void AddData(ref ProcessResult pr, FCMVessel_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                FCMVessel_SaveDAL dal = new FCMVessel_SaveDAL(db);

                string SN = (dal.GetMaxV_ID() + 1).ToString("D5");
                sm.V_ID = "V" + SN;

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Insert_cVessel(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void UpdateData(ref ProcessResult pr, FCMVessel_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                FCMVessel_SaveDAL dal = new FCMVessel_SaveDAL(db);

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Update_cVessel(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void DeleteData(ref ProcessResult pr, FCMVessel_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                FCMVessel_SaveDAL dal = new FCMVessel_SaveDAL(db);

                int effectCount = -1;

                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Delete_cVessel(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }
    }
}

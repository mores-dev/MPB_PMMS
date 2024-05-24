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
    public class FCMSchedule_SaveBLL : BLLBase
    {
        public void AddData(ref ProcessResult pr, FCMSchedule_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                FCMSchedule_SaveDAL dal = new FCMSchedule_SaveDAL(db);

                string SN = (dal.GetMaxSC_ID() + 1).ToString("D5");
                sm.SC_ID = "SC" + SN;

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Insert_cSchedule(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void UpdateData(ref ProcessResult pr, FCMSchedule_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                FCMSchedule_SaveDAL dal = new FCMSchedule_SaveDAL(db);

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Update_cSchedule(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void DeleteData(ref ProcessResult pr, FCMSchedule_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                FCMSchedule_SaveDAL dal = new FCMSchedule_SaveDAL(db);

                int effectCount = -1;

                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Delete_cSchedule(sm);
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

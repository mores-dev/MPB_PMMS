using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MPB_DAL;
using MPB_DAL.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Sys;
using DataAccessUtility;

namespace MPB_BLL.Sys
{
    public class SysBulletin_SaveBLL : BLLBase
    {
        public void AddData(ref ProcessResult pr, SysBulletin_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                SysBulletin_SaveDAL dal = new SysBulletin_SaveDAL(db);

                sm.SB_ID = DateTime.Now.ToString("yyMMdd") + (dal.GetMaxSB_ID() + 1).ToString("D4");

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Insert_cSysBulletin(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void UpdateData(ref ProcessResult pr, SysBulletin_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                SysBulletin_SaveDAL dal = new SysBulletin_SaveDAL(db);

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Update_cSysBulletin(sm);
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

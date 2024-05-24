using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MPB_DAL;
using MPB_DAL.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Mgmt;
using DataAccessUtility;

namespace MPB_BLL.Mgmt
{
    public class MgmtStation_SaveBLL : BLLBase
    {
        public void AddData(ref ProcessResult pr, MgmtStation_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtStation_SaveDAL dal = new MgmtStation_SaveDAL(db);

                if (string.IsNullOrWhiteSpace(sm.ST_MEMO)) sm.ST_MEMO = "";

                string SN = (dal.GetMaxST_ID() + 1).ToString("D5");
                sm.ST_ID = "ST" + SN;

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Insert_cStation(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void UpdateData(ref ProcessResult pr, MgmtStation_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtStation_SaveDAL dal = new MgmtStation_SaveDAL(db);

                if (string.IsNullOrWhiteSpace(sm.ST_MEMO)) sm.ST_MEMO = "";

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Update_cStation(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void DeleteData(ref ProcessResult pr, MgmtStation_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtStation_SaveDAL dal = new MgmtStation_SaveDAL(db);

                int effectCount = -1;

                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Delete_cStation(sm);
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

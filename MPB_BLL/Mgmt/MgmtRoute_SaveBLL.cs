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
    public class MgmtRoute_SaveBLL : BLLBase
    {
        public void AddData(ref ProcessResult pr, MgmtRoute_SaveMain sm, List<MgmtRoute_SaveDetailGrid> sdGrid1)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtRoute_SaveDAL dal = new MgmtRoute_SaveDAL(db);

                string SN = (dal.GetMaxR_ID() + 1).ToString("D5");
                sm.R_ID = "R" + SN;

                int effectCount = -1;
                int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    sm.ST_ID_START = sdGrid1[0].ST_ID;
                    sm.ST_ID_END = sdGrid1[sdGrid1.Count - 1].ST_ID;
                    effectCount = dal.Insert_cRoute(sm);
                    foreach (MgmtRoute_SaveDetailGrid item in sdGrid1)
                    {
                        i++;
                        item.R_ID = sm.R_ID;
                        item.ST_ODR = i;
                        effectCount = dal.Insert_cRouteDtl(item);
                    }
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }
        public void UpdateData(ref ProcessResult pr, MgmtRoute_SaveMain sm, List<MgmtRoute_SaveDetailGrid> sdGrid1)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtRoute_SaveDAL dal = new MgmtRoute_SaveDAL(db);

                int effectCount = -1;
                int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    sm.ST_ID_START = sdGrid1[0].ST_ID;
                    sm.ST_ID_END = sdGrid1[sdGrid1.Count - 1].ST_ID;
                    effectCount = dal.Update_cRoute(sm);
                    effectCount = dal.Delete_cRouteDtl(sm);
                    foreach (MgmtRoute_SaveDetailGrid item in sdGrid1)
                    {
                        i++;
                        item.R_ID = sm.R_ID;
                        item.ST_ODR = i;
                        effectCount = dal.Insert_cRouteDtl(item);
                    }
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void DeleteData(ref ProcessResult pr, MgmtRoute_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtRoute_SaveDAL dal = new MgmtRoute_SaveDAL(db);

                int effectCount = -1;

                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Delete_cRoute(sm);
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

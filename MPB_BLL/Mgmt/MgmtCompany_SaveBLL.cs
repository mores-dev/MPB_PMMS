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
    public class MgmtCompany_SaveBLL : BLLBase
    {
        public void AddData(ref ProcessResult pr, MgmtCompany_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtCompany_SaveDAL dal = new MgmtCompany_SaveDAL(db);

                if (string.IsNullOrWhiteSpace(sm.C_MEMO)) sm.C_MEMO = "";
                if (string.IsNullOrWhiteSpace(sm.C_TAX_ID)) sm.C_TAX_ID = "";

                string SN = (dal.GetMaxC_ID() + 1).ToString("D5");
                sm.C_ID = "C" + SN;

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Insert_cCompany(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void UpdateData(ref ProcessResult pr, MgmtCompany_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtCompany_SaveDAL dal = new MgmtCompany_SaveDAL(db);

                int effectCount = -1;

                if (string.IsNullOrWhiteSpace(sm.C_MEMO)) sm.C_MEMO = "";
                if (string.IsNullOrWhiteSpace(sm.C_TAX_ID)) sm.C_TAX_ID = "";

                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Update_cCompany(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        public void DeleteData(ref ProcessResult pr, MgmtCompany_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                MgmtCompany_SaveDAL dal = new MgmtCompany_SaveDAL(db);

                int effectCount = -1;

                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Delete_cCompany(sm);
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

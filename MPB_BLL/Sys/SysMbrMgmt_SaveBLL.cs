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
using MPB_BLL.Auth;
using MPB_BLL.COMMON;

namespace MPB_BLL.Sys
{
    public class SysMbrMgmt_SaveBLL : BLLBase
    {
        public void InsertData(ref ProcessResult pr, SysMbrMgmt_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                SysMbrMgmt_SaveDAL dal = new SysMbrMgmt_SaveDAL(db);

                pr.ReturnMessage = CheckData(sm, "A");
                if (!string.IsNullOrWhiteSpace(pr.ReturnMessage))
                {
                    pr.ReturnId = -1;
                    return;
                }

                int effectCount = -1;
                sm.GA_PPP = AuthLogin_BLL.Encode(sm.GA_PPP);

                if (string.IsNullOrWhiteSpace(sm.C_ID))
                    sm.C_ID = dal.Select_Company();

                if (sm.PhoneType == "T")
                {
                    sm.Phone = sm.AreaNumber + "-" + sm.Phone;
                    sm.Phone += string.IsNullOrWhiteSpace(sm.Ext) ? "" : "#" + sm.Ext;
                }

                using (ITransaction scope = db.GetTransaction())
                {
                    effectCount = dal.Insert_GAA(sm);//Fix 2023/10/24 CheckMarx原碼檢測: Privacy Violation\路徑 2:

                    effectCount += dal.Insert_ARole(sm);//Fix 2023/10/24 CheckMarx原碼檢測: Privacy Violation\路徑 4:

                    scope.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                pr.ReturnMessage = "新增失敗!";
            }
        }

        private string CheckData(SysMbrMgmt_SaveMain sm, string mode)
        {
            string msg = "";

            if (mode == "A")
            {
                if (!VaildBLL.ValidPassword(sm.GA_PPP, out string r1))
                {
                    msg = r1;
                    return msg;
                }
            }
            else if (!string.IsNullOrWhiteSpace(sm.GA_PPP))
            {
                if (!VaildBLL.ValidPassword(sm.GA_PPP, out string r1))
                {
                    msg = r1;
                    return msg;
                }
            }

            if (!string.IsNullOrWhiteSpace(sm.UniId))
            {
                if (!VaildBLL.ValidUniID(sm.UniId, out string r2))
                {
                    msg = "統一編號" + r2;
                    return msg;
                }
            }

            if (!VaildBLL.VaildEMail(sm.Email, out string r3))
            {
                msg = r3;
                return msg;
            }

            return msg;
        }

        public void UpdateData(ref ProcessResult pr, SysMbrMgmt_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                SysMbrMgmt_SaveDAL dal = new SysMbrMgmt_SaveDAL(db);

                pr.ReturnMessage = CheckData(sm, "M");
                if (!string.IsNullOrWhiteSpace(pr.ReturnMessage))
                {
                    pr.ReturnId = -1;
                    return;
                }

                int effectCount = -1;

                if (sm.flag_PPP == "PPP")
                    sm.GA_PPP = "123456";

                if (sm.PhoneType == "T")
                {
                    sm.Phone = sm.AreaNumber + "-" + sm.Phone;
                    sm.Phone += string.IsNullOrWhiteSpace(sm.Ext) ? "" : "#" + sm.Ext;
                }

                if (!string.IsNullOrWhiteSpace(sm.GA_PPP))
                    sm.GA_PPP = AuthLogin_BLL.Encode(sm.GA_PPP);
                
                using (ITransaction scope = db.GetTransaction())
                {
                    effectCount = dal.Update_cGAAccount(sm);

                    //if (!string.IsNullOrWhiteSpace(sm.GA_PPP))
                    //    effectCount = dal.Update_PPP(sm);

                    scope.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                pr.ReturnMessage = "更新失敗!";
            }
        }

        public List<AjaxKeyCountResult> Check_InputValid(SysMbrMgmt_SaveMain qc)
        {
            List<AjaxKeyCountResult> rtn = new List<AjaxKeyCountResult>();

            switch (qc.ValidType)
            {
                case "UniId":
                    if (VaildBLL.ValidUniID(qc.ValidData, out string r1))
                        rtn.Add(new AjaxKeyCountResult { KeyCount = 0 });
                    break;
                case "Email":
                    if (VaildBLL.VaildEMail(qc.ValidData, out string r2))
                        rtn.Add(new AjaxKeyCountResult { KeyCount = 0 });
                    break;
            }
            if (rtn.Count == 0)
                rtn.Add(new AjaxKeyCountResult { KeyCount = 1 });

            return rtn;
        }
    }
}

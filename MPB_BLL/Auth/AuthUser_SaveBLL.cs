using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MPB_DAL;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;
using DataAccessUtility;

namespace MPB_BLL.Auth
{
    public class AuthUser_SaveBLL : BLLBase
    {
        public void AddData(ref ProcessResult pr, AuthUser_SaveMain sm)
        {
            try
            {
                //密碼加密
                AuthLogin_BLL bll = new AuthLogin_BLL();
                var encryptionPd = AuthLogin_BLL.Encode(sm.Pd);

                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthUser_SaveDAL dal = new AuthUser_SaveDAL(db);

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    if (!string.IsNullOrEmpty(sm.Pd))
                    {
                        CheckPasswordRule(pr, sm);
                    }
                    if (pr.ReturnMessage == "")
                    {
                        if (string.IsNullOrEmpty(sm.DEPT)) sm.DEPT = "";
                        if (string.IsNullOrEmpty(sm.C_ID)) sm.C_ID = "";
                        //sm.UserType = sm.RoleId.StartsWith("1") ? "B" : "C";
                        if (sm.RoleId.StartsWith("1"))
                            sm.UserType = "B";
                        else if(sm.RoleId.StartsWith("2"))
                            sm.UserType = "C";
                        else if (sm.RoleId.StartsWith("4"))
                            sm.UserType = "D";

                        sm.Pd = encryptionPd;
                        effectCount = dal.Insert_cAFCA(sm);
                    }
                    if (effectCount > 0)
                    {
                        AuthUserRole_SaveDetailGrid1 sd = new AuthUserRole_SaveDetailGrid1();
                        sd.UserId = sm.UserId;
                        //sd.RoleId = sm.UserType == "B" ? "1000" : "2000";
                        sd.RoleId = sm.RoleId;
                        effectCount = dal.Insert_Grid1_AccountRole(sd);
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
        public void UpdateData(ref ProcessResult pr, AuthUser_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthUser_SaveDAL dal = new AuthUser_SaveDAL(db);

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scope = db.GetTransaction())
                {
                    if (!string.IsNullOrEmpty(sm.Pd))
                    {
                        CheckPasswordRule(pr, sm);
                        sm.Pd = AuthLogin_BLL.Encode(sm.Pd);
                    }
                    if (pr.ReturnMessage == "")
                    {
                        if (string.IsNullOrEmpty(sm.DEPT))
                            sm.DEPT = "";
                        sm.UserType = sm.RoleId.StartsWith("1") ? "B" : "C";

                        effectCount = dal.Update_cAFCAccount(sm);
                    }
                    scope.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }
        public void DeleteData(ref ProcessResult pr, AuthUser_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthUser_SaveDAL dal = new AuthUser_SaveDAL(db);

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Delete_AuthUser(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        private void CheckPasswordRule(ProcessResult pr, AuthUser_SaveMain sm)
        {
            Regex notAllBeCharacters = new Regex("[^A-Za-z]");
            Regex notAllBeNumbers = new Regex("[^0-9]");

            if (sm.Pd.Trim().Length < 8)
            {
                pr.ReturnMessage += "密碼不足8碼\n";
            }
            else if (sm.Pd.Trim().Length > 16)
            {
                pr.ReturnMessage += "密碼超過16碼\n";
            }
            else if (!notAllBeCharacters.Match(sm.Pd).Success)
            {
                pr.ReturnMessage += "密碼不得皆為英文\n";
            }
            else if (!notAllBeNumbers.Match(sm.Pd).Success)
            {
                pr.ReturnMessage += "密碼不得皆為數字\n";
            }
            if (!string.IsNullOrWhiteSpace(pr.ReturnMessage))
                pr.ReturnId = -1;
        }

        public void UpdatePwd(ref ProcessResult pr, AuthUser_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthUser_SaveDAL dal = new AuthUser_SaveDAL(db);
                //密碼加密
                AuthLogin_BLL bll = new AuthLogin_BLL();
                sm.Pd = AuthLogin_BLL.Encode(sm.Pd);

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Update_AuthUserPwd(sm);
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

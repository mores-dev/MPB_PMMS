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
    public class AuthChgPwd_SaveBLL : BLLBase
    {
        public void UpdatePwd(ref ProcessResult pr, AuthUser_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthUser_SaveDAL dal = new AuthUser_SaveDAL(db);
                //密碼加密
                AuthLogin_BLL bll = new AuthLogin_BLL();
                var encryptPd = AuthLogin_BLL.Encode(sm.NewPd);

                CheckPasswordRule(pr, sm);

                if (!string.IsNullOrWhiteSpace(pr.ReturnMessage))
                {
                    pr.ReturnId = -1;
                    return;
                }
                sm.Pd = encryptPd;
                int effectCount = -1;
                //int i = 0;
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

        private static void CheckPasswordRule(ProcessResult pr, AuthUser_SaveMain sm)
        {
            var encryptPdOld = AuthLogin_BLL.Encode(sm.Pd);
            var encryptPdNew = AuthLogin_BLL.Encode(sm.NewPd);

            //密碼驗證
            DbManager db = DbManager.GetInstance();
            AuthUser_SaveDAL dal = new AuthUser_SaveDAL();
            var user = dal.Select_AuthUser(sm);

            Regex notAllBeCharacters = new Regex("[^A-Za-z]");
            Regex notAllBeNumbers = new Regex("[^0-9]");
            if (user == null)
            {
                pr.ReturnMessage += "使用者不存在!";
            }
            else if (user.Pd != encryptPdOld)
            {
                pr.ReturnMessage += "舊密碼輸入錯誤，請重新輸入";
            }
            else if (user.Pd == encryptPdNew)
            {
                pr.ReturnMessage += "密碼不可重複使用";
            }
            else if (sm.NewPd != sm.ConfirmPd)
            {
                pr.ReturnMessage += "密碼不相符";
            }
            else if (sm.NewPd.Trim().Length < 8)
            {
                pr.ReturnMessage += "密碼不足8碼";
            }
            else if (sm.NewPd.Trim().Length > 16)
            {
                pr.ReturnMessage += "密碼超過16碼";
            }
            else if (!notAllBeCharacters.Match(sm.NewPd).Success)
            {
                pr.ReturnMessage += "密碼不得皆為英文";
            }
            else if (!notAllBeNumbers.Match(sm.NewPd).Success)
            {
                pr.ReturnMessage += "密碼不得皆為數字";
            }
            else if (encryptPdNew == user.Pd || encryptPdNew == user.Pd1 || encryptPdNew == user.Pd2)
                pr.ReturnMessage += "密碼不得與前三次相同";
        }
    }
}

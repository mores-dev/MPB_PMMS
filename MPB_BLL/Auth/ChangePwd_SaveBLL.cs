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
    public class ChangePwd_SaveBLL : BLLBase
    {
        public void UpdateData(ref ProcessResult pr, ChangePwd_SaveMain sm)
        {
            try
            {
                //密碼加密
                AuthLogin_BLL bll = new AuthLogin_BLL();
                var encryptPd = AuthLogin_BLL.Encode(sm.NewPd);
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                ChangePwd_SaveDAL dal = new ChangePwd_SaveDAL(db);

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
                    effectCount = dal.Update_UserPwd(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        private static void CheckPasswordRule(ProcessResult pr, ChangePwd_SaveMain sm)
        {
            var encryptPdNew = AuthLogin_BLL.Encode(sm.NewPd);

            //密碼驗證
            DbManager db = DbManager.GetInstance();
            ChangePwd_SaveDAL dal = new ChangePwd_SaveDAL();
            var user = dal.Select_AFCAccount(sm);

            Regex notAllBeCharacters = new Regex("[^A-Za-z]");
            Regex notAllBeNumbers = new Regex("[^0-9]");
            if (user == null)
            {
                pr.ReturnMessage += "使用者不存在!";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;
using MPB_DAL;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Auth;


namespace MPB_BLL.Auth
{
    public class AuthLogin_BLL : BLLBase
    {
        public UserInfo GetUserInfo(ref ProcessResult pr, AuthLogin_EditEntities sm)
        {
            UserInfo loginUserInfo = null;

            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthLogin_DAL dal = new AuthLogin_DAL(db);

                var encryptionPd = Encode(sm.UserPD);
                using (ITransaction scpoe = db.GetTransaction())
                {
                    var checkResult = dal.Select_ATH_USER(sm, encryptionPd);
                    //驗証失敗
                    if (checkResult == null || checkResult.UserAAA == string.Empty)
                    {
                        scpoe.Complete();
                        throw new ApplicationException("帳號或密碼錯誤!");
                    }
                    if (checkResult.Status == "N")
                        throw new ApplicationException("帳號未啟用!");
                    else if (checkResult.Status != "Y")
                        throw new ApplicationException("帳號狀態異常，請通知系統管理員!");
                    if (checkResult.UserPD != encryptionPd)
                    {
                        if (checkResult.PppErr >= 3)
                        {
                            TimeSpan nowDt = new TimeSpan(DateTime.Now.Ticks);
                            TimeSpan loginDt = new TimeSpan(Convert.ToDateTime(checkResult.LastLogin).Ticks);
                            TimeSpan ts = nowDt.Subtract(loginDt).Duration();
                            if (ts.Minutes < 15)
                                throw new ApplicationException("密碼錯誤次數過多，請通知帳號管理員重設密碼，或15分鐘後重新登入!");
                        }
                        dal.UpdateErrCount(sm);
                        scpoe.Complete();
                        throw new ApplicationException("帳號或密碼錯誤!");
                    }

                    dal.ResetErrCount(sm);

                    //登入者資訊
                    loginUserInfo = dal.Select_UserInfo(sm.UserAAA);
                    //登入者可使用功能
                    loginUserInfo.UserProgramInfos = dal.Select_UserProgramInfos(sm.UserAAA);

                    scpoe.Complete();
                }

                if (loginUserInfo != null)
                {
                    loginUserInfo.LoginUserId = loginUserInfo.Id;
                    loginUserInfo.LoginUserName = loginUserInfo.Name;
                    loginUserInfo.LoginUser = loginUserInfo;
                }
            }
            catch (ApplicationException ex)
            {
                pr.ReturnId = -1;
                logger.Error(ex.ToString());
                //pr.ReturnMessage = ex.Message;
                pr.ReturnMessage = "登入失敗" + ex.Message;
            }
            catch (Exception ex)
            {
                pr.ReturnId = -1;
                pr.ReturnMessage = "系統發生錯誤";
                Console.WriteLine(ex.ToString());
                logger.Error(ex.ToString());
            }

            return loginUserInfo;
        }

        /// <summary>
        /// 客船乘客名冊管理系統加密方式 SHA256
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encode(string value)
        {
            HashAlgorithm hash = null;
            string result = "";
            try
            {
                hash = SHA256.Create();

                //SHA256 hash = new SHA256CryptoServiceProvider();
                var combinded = Encoding.UTF8.GetBytes(value ?? "");
                result = BitConverter.ToString(hash.ComputeHash(combinded)).ToLower().Replace("-", "");
            }
            finally
            {
                if (hash != null)
                {
                    hash.Clear();
                    hash.Dispose();
                }
            }
            return result;
        }

        public void ResetLoginLog(AuthLogin_EditEntities vm, AuthLogin_LoginLog loginLog)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthLogin_DAL dal = new AuthLogin_DAL(db);

                if (loginLog == null)
                    dal.Insert_Login_Log(vm);
                else
                {
                    loginLog.ErrCnt = 0;
                    dal.Update_Login_Log(vm, loginLog);
                }
            }
            catch
            {

            }
        }

        public AuthLogin_LoginLog GetLoginCount(AuthLogin_EditEntities vm)
        {
            //商業邏輯、輸入檢查
            DbManager db = DbManager.GetInstance();
            AuthLogin_DAL dal = new AuthLogin_DAL(db);

            AuthLogin_LoginLog rtn = dal.Select_Login_Log(vm);
            DateTime tmp;
            if (rtn != null && DateTime.TryParse(rtn.LogDate, out tmp))
                rtn.LogDT = tmp;
            return rtn;
        }

        public void Add_LoginErrorCount(AuthLogin_EditEntities vm, AuthLogin_LoginLog loginLog)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthLogin_DAL dal = new AuthLogin_DAL(db);
                if (loginLog == null)
                    dal.Insert_Login_Log(vm);
                else
                {
                    if (DateTime.Now > loginLog.LogDT.AddMinutes(15))
                        loginLog.ErrCnt = 1;
                    else
                        loginLog.ErrCnt += 1;

                    dal.Update_Login_Log(vm, loginLog);
                }
            }
            catch
            {

            }
        }
    }
}

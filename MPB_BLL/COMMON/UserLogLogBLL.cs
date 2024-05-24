using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPB_DAL;
using MPB_DAL.COMMON;
using MPB_Entities.COMMON;

namespace MPB_BLL.COMMON
{
    public class UserLogBLL : BLLBase
    {

        /// <summary>
        /// 加入使用者登入LOG
        /// </summary>
        /// <returns></returns>
        public void AddUserLogLog(UserLog userLoglog)
        {
            try
            {
                DbManager db = DbManager.GetInstance();
                UserLogDAL dal = new UserLogDAL(db);

                int effectCount = -1;

                effectCount = dal.Insert_LogRecord(userLoglog);

            }
            catch (Exception ex)
            {
                ExceptionLog(ex);
                throw;
            }
        }
    }
}

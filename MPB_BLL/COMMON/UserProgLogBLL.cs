using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MPB_DAL;
using MPB_DAL.COMMON;
using MPB_Entities.COMMON;

namespace MPB_BLL.COMMON
{
    public class UserProgLogBLL : BLLBase
    {

        /// <summary>
        /// 加入使用者操作記錄LOG
        /// </summary>
        /// <returns></returns>
        public void AddUserProgLog(UserLog userProglog)
        {
            try
            {
                DbManager db = DbManager.GetInstance();
                UserProgLogDAL dal = new UserProgLogDAL(db);

                int effectCount = -1;

                effectCount = dal.Insert_LogRecord(userProglog);

            }
            catch (Exception ex)
            {
                ExceptionLog(ex);
                throw;
            }
        }
    }
}

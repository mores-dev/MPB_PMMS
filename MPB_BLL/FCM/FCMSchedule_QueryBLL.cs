using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;

namespace MPB_BLL.FCM
{
    public class FCMSchedule_QueryBLL : BLLBase
    {
        FCMSchedule_QueryDAL _dal = new FCMSchedule_QueryDAL();

        public PageList<FCMSchedule_QueryResult> GetPageList(FCMSchedule_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;

namespace MPB_BLL.FCM
{
    public class FCMVessel_QueryBLL : BLLBase
    {
        FCMVessel_QueryDAL _dal = new FCMVessel_QueryDAL();

        public PageList<FCMVessel_QueryResult> GetPageList(FCMVessel_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

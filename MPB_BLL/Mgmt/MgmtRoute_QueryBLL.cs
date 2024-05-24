using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Mgmt;

namespace MPB_BLL.Mgmt
{
    public class MgmtRoute_QueryBLL : BLLBase
    {
        MgmtRoute_QueryDAL _dal = new MgmtRoute_QueryDAL();

        public PageList<MgmtRoute_QueryResult> GetPageList(MgmtRoute_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

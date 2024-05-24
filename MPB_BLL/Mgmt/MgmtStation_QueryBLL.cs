using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Mgmt;

namespace MPB_BLL.Mgmt
{
    public class MgmtStation_QueryBLL : BLLBase
    {
        MgmtStation_QueryDAL _dal = new MgmtStation_QueryDAL();

        public PageList<MgmtStation_QueryResult> GetPageList(MgmtStation_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

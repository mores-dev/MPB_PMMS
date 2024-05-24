using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Mgmt;

namespace MPB_BLL.Mgmt
{
    public class MgmtCompany_QueryBLL : BLLBase
    {
        MgmtCompany_QueryDAL _dal = new MgmtCompany_QueryDAL();

        public PageList<MgmtCompany_QueryResult> GetPageList(MgmtCompany_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

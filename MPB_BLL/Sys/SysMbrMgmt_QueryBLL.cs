using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Sys;

namespace MPB_BLL.Sys
{
    public class SysMbrMgmt_QueryBLL : BLLBase
    {
        SysMbrMgmt_QueryDAL _dal = new SysMbrMgmt_QueryDAL();

        public PageList<SysMbrMgmt_QueryResult> GetPageList(SysMbrMgmt_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

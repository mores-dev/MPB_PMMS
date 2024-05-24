using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Sys;

namespace MPB_BLL.Sys
{
    public class SysConsRec_QueryBLL : BLLBase
    {
        SysConsRec_QueryDAL _dal = new SysConsRec_QueryDAL();

        public PageList<SysConsRec_QueryResult> GetPageList(SysConsRec_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

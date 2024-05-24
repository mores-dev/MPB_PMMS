using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Sys;

namespace MPB_BLL.Sys
{
    public class SysBulletin_QueryBLL : BLLBase
    {
        SysBulletin_QueryDAL _dal = new SysBulletin_QueryDAL();

        public PageList<SysBulletin_QueryResult> GetPageList(SysBulletin_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

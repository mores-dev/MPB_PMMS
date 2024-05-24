using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;

namespace MPB_BLL.Auth
{
    public class AuthRecord_QueryBLL : BLLBase
    {
        AuthRecord_QueryDAL _dal = new AuthRecord_QueryDAL();

        public PageList<AuthRecord_QueryResult> GetPageList(AuthRecord_QueryCondition qc)
        {
            qc.LogOn = ColumnCheck_Datetime(qc.LogOn);
            qc.LogOff = ColumnCheck_Datetime(qc.LogOff);
            return _dal.GetPageList(qc);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;

namespace MPB_BLL.Auth
{
    public class AuthRole_QueryBLL : BLLBase
    {
        AuthRole_QueryDAL _dal = new AuthRole_QueryDAL();

        public PageList<AuthRole_QueryResult> GetPageList(AuthRole_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

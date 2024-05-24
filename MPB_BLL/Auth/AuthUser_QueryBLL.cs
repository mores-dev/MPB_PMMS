using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;

namespace MPB_BLL.Auth
{
    public class AuthUser_QueryBLL : BLLBase
    {
        AuthUser_QueryDAL _dal = new AuthUser_QueryDAL();

        public PageList<AuthUser_QueryResult> GetPageList(AuthUser_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

    }
}

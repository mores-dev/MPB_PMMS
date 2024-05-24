using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;

namespace MPB_BLL.Auth
{
    public class AuthUser_EditBLL : BLLBase
    {
        AuthUser_EditDAL _dal = new AuthUser_EditDAL();

        public AuthUser_EditMain GetDataMain(AuthUser_EditMain qc)
        {
            return _dal.Select_cAFCAccount(qc);
        }
        //新增資料 檢查Key值 Function
        public List<AjaxKeyCountResult> Check_Key(AuthUser_EditMain qc)
        {
            return _dal.Check_Key(qc);

        }

        public List<AuthUserRole_EditDetailGrid1> GetDataDetailGrid1(AuthUser_EditMain em)
        {
            return _dal.Select_Grid1_AccountRole(em);
        }

        public List<CodeName> GetRoleId()
        {
            return _dal.GetRoleId();
        }
    }
}

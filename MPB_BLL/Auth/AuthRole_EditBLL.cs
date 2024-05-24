using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;

namespace MPB_BLL.Auth
{
    public class AuthRole_EditBLL : BLLBase
    {
        AuthRole_EditDAL _dal = new AuthRole_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public AuthRole_EditMain GetDataMain(AuthRole_EditMain qc)
        {
            return _dal.Select_RoleInfo(qc);
        }
        
        /// <summary>
        /// 查詢 Grid1 資料
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AuthRole_EditDetailGrid1> GetDataDetailGrid1(AuthRole_EditMain qc)
        {
            return _dal.Select_Grid1_RoleFunc(qc);
        }

        /// <summary>
        /// 新增資料 檢查Key值 Function
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(AuthRole_EditMain qc)
        {
            return _dal.Check_Key(qc);
        }

        /// <summary>
        /// 下拉選單 選單代號
        /// </summary>
        /// <returns></returns>
        public List<CatCodeName> GetMenuId()
        {
            return _dal.GetMenuId();
        }

        /// <summary>
        /// 下拉連動選單 程式代號
        /// </summary>
        /// <returns></returns>
        public List<CatCodeName> GetProgId()
        {
            return _dal.GetProgId();
        }

        public List<CodeName> GetDeviceType()
        {
            return _dal.GetDeviceType();
        }
    }
}

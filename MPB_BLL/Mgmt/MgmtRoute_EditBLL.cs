using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Mgmt;

namespace MPB_BLL.Mgmt
{
    public class MgmtRoute_EditBLL : BLLBase
    {
        MgmtRoute_EditDAL _dal = new MgmtRoute_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public MgmtRoute_EditMain GetDataMain(MgmtRoute_EditMain qc)
        {
            return _dal.Select_cRoute(qc);
        }

        /// <summary>
        /// 航線的場站查詢
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<MgmtRoute_EditDetailGrid> GetDataDetailGrid(MgmtRoute_EditMain qc)
        {
            return _dal.Select_cRouteDtl(qc);
        }

        /// <summary>
        /// 場站下拉選單
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<CodeName> GetStation()
        {
            return _dal.Select_cStation();
        }

        /// <summary>
        /// 新增資料 檢查Key值 Function
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(MgmtRoute_EditMain qc)
        {
            return _dal.Check_Key(qc);
        }

    }
}

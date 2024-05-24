using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Mgmt;

namespace MPB_BLL.Mgmt
{
    public class MgmtStation_EditBLL : BLLBase
    {
        MgmtStation_EditDAL _dal = new MgmtStation_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public MgmtStation_EditMain GetDataMain(MgmtStation_EditMain qc)
        {
            return _dal.Select_cStation(qc);
        }      
        

        /// <summary>
        /// 新增資料 檢查Key值 Function
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(MgmtStation_EditMain qc)
        {
            return _dal.Check_Key(qc);
        }

    }
}

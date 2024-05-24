using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Sys;

namespace MPB_BLL.Sys
{
    public class SysBulletin_EditBLL : BLLBase
    {
        SysBulletin_EditDAL _dal = new SysBulletin_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public SysBulletin_EditMain GetDataMain(SysBulletin_EditMain qc)
        {
            return _dal.Select_cSysBulletin(qc);
        }
    }
}

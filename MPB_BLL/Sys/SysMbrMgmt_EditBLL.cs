using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Sys;

namespace MPB_BLL.Sys
{
    public class SysMbrMgmt_EditBLL : BLLBase
    {
        SysMbrMgmt_EditDAL _dal = new SysMbrMgmt_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public SysMbrMgmt_EditMain GetDataMain(SysMbrMgmt_EditMain qc)
        {
            SysMbrMgmt_EditMain rtn = _dal.Select_cGAAccount(qc);

            if (rtn.PhoneType != "M")
            {
                string[] part1 = rtn.Phone.Split('-');
                rtn.AreaNumber = part1[0];
                if (part1.Length == 2)
                {
                    string[] part2 = part1[1].Split('#');
                    rtn.Phone = part2[0];
                    rtn.Ext = part2.Length == 2 ? part2[1] : "";
                }
            }

            return rtn;
        }

        public List<AjaxKeyCountResult> Check_Key(SysMbrMgmt_EditMain qc)
        {
            return _dal.Check_Key(qc);
        }
    }
}

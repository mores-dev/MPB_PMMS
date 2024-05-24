using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;

namespace MPB_BLL.FCM
{
    public class FCMSchedule_EditBLL : BLLBase
    {
        FCMSchedule_EditDAL _dal = new FCMSchedule_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public FCMSchedule_EditMain GetDataMain(FCMSchedule_EditMain qc)
        {
            return _dal.Select_cSchedule(qc);
        }

        /// <summary>
        /// 取得套用航線
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<CodeName> GetRoute()
        {
            return _dal.Select_cRoute();
        }

        /// <summary>
        /// 新增資料 檢查Key值 Function
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(FCMSchedule_EditMain qc)
        {
            return _dal.Check_Key(qc);
        }

    }
}

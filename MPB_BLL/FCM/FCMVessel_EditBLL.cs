using System;
using System.Collections.Generic;
using System.Linq;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;

namespace MPB_BLL.FCM
{
    public class FCMVessel_EditBLL : BLLBase
    {
        FCMVessel_EditDAL _dal = new FCMVessel_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public FCMVessel_EditMain GetDataMain(FCMVessel_EditMain qc)
        {
            return _dal.Select_cVessel(qc);
        }

        /// <summary>
        /// 新增資料 檢查Key值 Function
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(FCMVessel_EditMain qc)
        {
            return _dal.Check_Key(qc);
        }

    }
}

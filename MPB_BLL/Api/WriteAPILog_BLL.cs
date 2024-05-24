using DataAccessUtility;
using MPB__DAL.Api;
using MPB_BLL;
using MPB_DAL;
using MPB_Entities.Api;
using System;
using System.Collections.Generic;

namespace MPB__BLL.Api
{
    public class WriteAPILog_BLL : BLLBase
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public void InsertData(WriteAPILog_Entities ent)
        {
            //商業邏輯、輸入檢查
            DbManager db = DbManager.GetInstance();
            WriteAPILog_DAL dal = new WriteAPILog_DAL(db);
            dal.InsertData(ent);
        }

    }
}

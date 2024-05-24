using DataAccessUtility;
using MPB_BLL.COMMON;
using MPB_DAL;
using MPB_DAL.Api;
using MPB_Entities.Api;
using Newtonsoft.Json.Linq;
using System;

namespace MPB_BLL.Api
{
    public class InsertTransData_TVS_BLL : BLLBase
    {
        InsertTransData_TVS_DAL _dal = new InsertTransData_TVS_DAL();

        /// <summary>
        /// 寫入
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public APIResponse<InsertTransData_TVS_RSPN_Entities> InsertTransData(APIRequest request)
        {
            APIResponse<InsertTransData_TVS_RSPN_Entities> response = new APIResponse<InsertTransData_TVS_RSPN_Entities>(request.FUNC_ID, request.SYS_DATE, request.ACNT_NO);
            InsertTransData_TVS_RSPN_Entities rtn = new InsertTransData_TVS_RSPN_Entities();
            InsertTransData_TVS_FUNC_Entities func = new InsertTransData_TVS_FUNC_Entities();
            try
            {
                JObject jObject = (JObject)request.FUNC_DATA;
                ModelBLL.JObjToModel(jObject, ref func);

                DbManager db = DbManager.GetInstance();
                int effectCount = -1;

                //CALL_TYPE 呼叫類別   0:補個資(部分欄位UPDATE)  1:寫入完整資料  2:刪除
                if (func.CALL_TYPE == "1")
                {
                    //1. 確認是否有此驗票明細
                    int count = _dal.CheckDetail(func);
                    if (count > 0)
                        throw new Exception("重複使用!");

                    using (ITransaction scpoe = db.GetTransaction())
                    {
                        //2. 確認是否有此驗票主檔
                        count = _dal.CheckMaster(func);

                        if (count == 0)
                        {
                            //3. 寫入驗票主檔
                            effectCount = _dal.Insert_Master(func);
                        }

                        //4. 寫入驗票明細
                        effectCount = _dal.Insert_Detail(func);

                        scpoe.Complete();
                    }
                }
                else if (func.CALL_TYPE == "2")
                {
                    using (ITransaction scpoe = db.GetTransaction())
                    {
                        //1. 刪除乘客個資
                        effectCount = _dal.Delete_Detail(func);

                        //2. 判斷是否成功
                        if (effectCount < 1)
                            throw new Exception("無此票券登船紀錄!");
                        else if (effectCount > 1)
                            throw new Exception("刪除到多筆資料!");

                        //3. 刪除名冊
                        effectCount = _dal.Delete_ManifestDetail(func);

                        scpoe.Complete();
                    }
                }
                else
                {
                    using (ITransaction scpoe = db.GetTransaction())
                    {
                        //1. 更新乘客個資
                        effectCount = _dal.Update_Detail(func);

                        //2. 判斷是否成功
                        if (effectCount < 1)
                            throw new Exception("補個資失敗!");
                        else if (effectCount > 1)
                            throw new Exception("修改到多筆資料!");

                        scpoe.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                return response.Error(ex.Message);
            }

            return response.OK(rtn);
        }
    }
}

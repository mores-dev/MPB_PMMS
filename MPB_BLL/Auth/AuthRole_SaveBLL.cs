using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using MPB_DAL;
using MPB_DAL.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.Auth;
using DataAccessUtility;

namespace MPB_BLL.Auth
{
    public class AuthRole_SaveBLL : BLLBase
    {
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="sm">資料主檔</param>
        /// <param name="sdGrid1">資料明細</param>
        public void AddData(ref ProcessResult pr, AuthRole_SaveMain sm, List<AuthRole_SaveDetailGrid1> sdGrid1)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthRole_SaveDAL dal = new AuthRole_SaveDAL(db);

                int cnt = dal.GetRoleNewID(sm);
                sm.RoleId = sm.RoleType + cnt.ToString("000");

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Insert_RoleInfo(sm);

                    sdGrid1.ForEach(x => { x.RoleId = sm.RoleId; x.CreateId = sm.CreateId; });
                    effectCount = Insert_Grid1(sdGrid1, dal);

                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        /// <summary>
        /// 修改資料
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="sm">資料主檔</param>
        /// <param name="sdGrid1">資料明細</param>
        public void UpdateData(ref ProcessResult pr, AuthRole_SaveMain sm, List<AuthRole_SaveDetailGrid1> sdGrid1)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthRole_SaveDAL dal = new AuthRole_SaveDAL(db);

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Update_RoleInfo(sm);
                    effectCount = dal.Delete_Grid1_RoleFunc(sm);

                    sdGrid1.ForEach(x => { x.RoleId = sm.RoleId; x.CreateId = sm.ModifyId; });
                    effectCount = Insert_Grid1(sdGrid1, dal);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

        private static int Insert_Grid1(List<AuthRole_SaveDetailGrid1> sdGrid1, AuthRole_SaveDAL dal)
        {
            int effectCount = 0;
            foreach (AuthRole_SaveDetailGrid1 sd in sdGrid1)
            {
                //sd.RoleId = sm.RoleId;
                //sd.ProgExec = sd.ProgExec.ToLower() == "true" ? "Y" : "N";
                sd.ProgExec = "Y";
                sd.ProgQuery = "Y";
                sd.ProgAdd = sd.ProgAdd.ToLower() == "true" ? "Y" : "N";
                sd.ProgMod = sd.ProgMod.ToLower() == "true" ? "Y" : "N";
                sd.ProgDel = sd.ProgDel.ToLower() == "true" ? "Y" : "N";
                //sd.ProgDel = "Y";
                //sd.ProgView = sd.ProgView.ToLower() == "true" ? "Y" : "N";
                sd.ProgView = "Y";
                //sd.ProgDo = sd.ProgDo.ToLower() == "true" ? "Y" : "N";
                //sd.ProgUndo = sd.ProgUndo.ToLower() == "true" ? "Y" : "N";

                effectCount += dal.Insert_Grid1_RoleFunc(sd);
            }

            return effectCount;
        }

        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="pr"></param>
        /// <param name="sm">資料主檔</param>
        public void DeleteData(ref ProcessResult pr, AuthRole_SaveMain sm)
        {
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                AuthRole_SaveDAL dal = new AuthRole_SaveDAL(db);

                int effectCount = -1;
                
                using (ITransaction scpoe = db.GetTransaction())
                {
                    effectCount = dal.Delete_Grid1_RoleFunc(sm);
                    effectCount = dal.Delete_RoleInfo(sm);
                    scpoe.Complete();
                }
            }
            catch
            {
                pr.ReturnId = -1;
                //pr.ReturnMessage = "寫入失敗";
            }
        }

    }
}

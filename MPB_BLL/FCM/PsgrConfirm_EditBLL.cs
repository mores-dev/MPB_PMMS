using System;
using System.Collections.Generic;
using System.Linq;
using MPB_BLL.COMMON;
using MPB_DAL;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;
using System.Text.RegularExpressions;
using DataAccessUtility;

namespace MPB_BLL.FCM
{
    public class PsgrConfirm_EditBLL : BLLBase
    {
        PsgrConfirm_EditDAL _dal = new PsgrConfirm_EditDAL();

        /// <summary>
        /// 查詢主要Function 由查詢首頁帶入維護頁面
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public PsgrConfirm_EditMain GetDataMain(PsgrConfirm_EditMain qc)
        {
            return _dal.Select_Manifest(qc);
        }

        public List<PsgrConfirm_EditDetailGrid1> GetDataDetailGrid1(PsgrConfirm_EditMain em)
        {
            List<PsgrConfirm_EditDetailGrid1> rtn = _dal.Select_Grid1_ManifestDtl(em);

            rtn.ForEach(x => { if (EnDeCode.DecryptAES256(x.IdNo, out string s, KeyType.IDNO)) x.IdNo = s; });
            rtn.ForEach(x => x.IdNo = DataMask.MaskValue(x.IdNo, MaskType.Id));

            return rtn;
        }

        public string CheckData(PsgrConfirm_EditMain qc, bool insert)
        {
            string errMsg = "";
            DateTime dt;
            string outStr;

            if (string.IsNullOrWhiteSpace(qc.C_ID) || string.IsNullOrWhiteSpace(qc.Station)
                || string.IsNullOrWhiteSpace(qc.VoyageTime) || string.IsNullOrWhiteSpace(qc.VesselId))
                errMsg += "資訊不完整，請重新查詢後進行補登!\n";

            if (insert)
            {
                if (string.IsNullOrWhiteSpace(qc.PsgrName))
                    errMsg += "姓名不可為空!\n";
                if (!DateTime.TryParse(qc.Birth, out dt))
                    errMsg += "出日年月日格式錯誤!\n";
                if (string.IsNullOrWhiteSpace(qc.IdNo))
                    errMsg += "證件號碼不可為空!\n";
                else
                {
                    if (qc.IdType == "0")
                    {
                        if (!ValidID(qc.IdNo, out outStr))
                            errMsg += "身分證字號格式錯誤\n";
                        else
                            qc.Sex = qc.IdNo.Substring(1, 1) == "1" ? "M" : "F";
                    }
                    else qc.Sex = "";
                }
            }
            return errMsg;
        }
        private bool ValidID(string str, out string result)
        {
            //這邊不做空白驗證 出現空白丟出去讓空白驗證驗
            if (string.IsNullOrEmpty(str))
            {
                result = "";
                return true;
            }

            bool flag = Regex.IsMatch(str, @"^[A-Za-z]{1}[1-2]{1}[0-9]{8}$");
            //使用正規運算式判斷是否符合格式
            int[] ID = new int[11];//英文字會轉成2個數字,所以多一個空間存放變11個
            int count = 0;
            str = str.ToUpper();//把英文字轉成大寫
            if (flag == true)//如果符合格式就進入運算
            {//先把A~Z的對應值存到陣列裡，分別存進第一個跟第二個位置
                switch (str.Substring(0, 1))//取出輸入的第一個字--英文字母作為判斷
                {
                    case "A": (ID[0], ID[1]) = (1, 0); break;//如果是A,ID[0]就放入1,ID[1]就放入0
                    case "B": (ID[0], ID[1]) = (1, 1); break;//以下以此類推
                    case "C": (ID[0], ID[1]) = (1, 2); break;
                    case "D": (ID[0], ID[1]) = (1, 3); break;
                    case "E": (ID[0], ID[1]) = (1, 4); break;
                    case "F": (ID[0], ID[1]) = (1, 5); break;
                    case "G": (ID[0], ID[1]) = (1, 6); break;
                    case "H": (ID[0], ID[1]) = (1, 7); break;
                    case "I": (ID[0], ID[1]) = (3, 4); break;
                    case "J": (ID[0], ID[1]) = (1, 8); break;
                    case "K": (ID[0], ID[1]) = (1, 9); break;
                    case "L": (ID[0], ID[1]) = (2, 0); break;
                    case "M": (ID[0], ID[1]) = (2, 1); break;
                    case "N": (ID[0], ID[1]) = (2, 2); break;
                    case "O": (ID[0], ID[1]) = (3, 5); break;
                    case "P": (ID[0], ID[1]) = (2, 3); break;
                    case "Q": (ID[0], ID[1]) = (2, 4); break;
                    case "R": (ID[0], ID[1]) = (2, 5); break;
                    case "S": (ID[0], ID[1]) = (2, 6); break;
                    case "T": (ID[0], ID[1]) = (2, 7); break;
                    case "U": (ID[0], ID[1]) = (2, 8); break;
                    case "V": (ID[0], ID[1]) = (2, 9); break;
                    case "W": (ID[0], ID[1]) = (3, 2); break;
                    case "X": (ID[0], ID[1]) = (3, 0); break;
                    case "Y": (ID[0], ID[1]) = (3, 1); break;
                    case "Z": (ID[0], ID[1]) = (3, 3); break;
                }
                for (int i = 2; i < ID.Length; i++)//把英文字後方的數字丟進ID[]裡
                {
                    ID[i] = Convert.ToInt32(str.Substring(i - 1, 1));
                }
                for (int j = 1; j < ID.Length - 1; j++)
                {
                    count += ID[j] * (10 - j);//根據公式,ID[1]*9+ID[2]*8......
                }
                count += ID[0] + ID[10];//把沒加到的第一個數加回來
                if (count % 10 != 0)//餘數是0代表正確
                {
                    result = "身份證字號錯誤";
                    return false;
                }

            }
            else
            {
                result = "身份證格式不正確";
                return false;
            }
            result = "";
            return true;
        }

        public string InsertData(PsgrConfirm_EditMain em)
        {
            string rtn = "";
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                PsgrConfirm_EditDAL dal = new PsgrConfirm_EditDAL();

                em.IdNoEncode = EnDeCode.EncryptAES256(em.IdNo, KeyType.IDNO);
                em.IdNo = DataMask.MaskValue(em.IdNo, MaskType.Id);

                int effectCount = -1;
                //int i = 0;
                using (ITransaction scope = db.GetTransaction())
                {
                    effectCount = dal.Insert_ManifestDtl(em);
                    scope.Complete();
                }

            }
            catch
            {
                rtn = "資料寫入失敗!\n";
            }
            return rtn;
        }

        public string DeleteData(PsgrConfirm_EditMain em)
        {
            string rtn = "";
            try
            {
                //商業邏輯、輸入檢查
                DbManager db = DbManager.GetInstance();
                PsgrConfirm_EditDAL dal = new PsgrConfirm_EditDAL();
                int effectCount = -1;
                //int i = 0;
                using (ITransaction scope = db.GetTransaction())
                {
                    effectCount = dal.Delete_ManifestDtl(em);
                    scope.Complete();
                }
                if (effectCount == 0)
                    rtn = "刪除失敗!\n";
            }
            catch
            {
                rtn = "資料寫入失敗!\n";
            }
            return rtn;
        }
    }
}

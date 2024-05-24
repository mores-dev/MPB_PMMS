using MPB_Entities.COMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MPB_BLL.COMMON
{
    public class VaildBLL : BLLBase
    {
        /// <summary>
        /// 票號 前兩碼英文為航商代碼  第三碼為英文字 第四碼至第九碼為數字 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool VaildTicketNo(string str, out string result)
        {

            if (!VaildRequired(str, out result))
            {
                return false;
            }

            if (!VaildSPChar(str, out result))
            {
                return false;
            }

            if (str.Length != 9)
            {
                result = "長度錯誤";
                logger.Warn(result);
                return false;
            }
            string[] engCodes = new string[] { "TL","TF" };
            string code = str.Substring(0, 2);
            if (string.IsNullOrEmpty(engCodes.Where(x => x == code).FirstOrDefault()))
            {
                result = "無此船商代碼";
                logger.Warn(result);
                return false;
            }

            if (!Regex.Match(str, "^[A-Z]{3}\\d{6}$", RegexOptions.IgnoreCase).Success)
            {
                result = "船票格式錯誤";
                logger.Warn(result);
                return false;
            }

            result = "";
            return true;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool VaildName(string str, out string result)
        {

            if(!VaildRequired(str, out result))
            {
                return false;
            }

            if (!VaildSPChar(str, out result))
            {
                return false;
            }

            if (!VaildLength(str, 2 , null, out result))
            {
                return false;
            }
            
            result = "";
            return true;
        }

        /// <summary>
        /// 生日驗證
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool VaildBirthday(DateTime dt, out string result)
        {
            DateTime now = DateTime.Now;
            if (now < dt)
            {
                result = "生日日期錯誤";
                return false;
            }
            result = "";
            return true;
        }

        /// <summary>
        /// 生日驗證
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool VaildBirthday(string str, out string result)
        {
            
            if (string.IsNullOrEmpty(str))
            {
                result = "不可空白";
                return false;
            }

            DateTime date = new DateTime();
            DateTime now = DateTime.Parse(DateTime.Now.AddDays(1d).ToShortDateString());
            if (!DateTime.TryParseExact(str, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                result = "生日格式錯誤";
                return false;
            }

            if (date >= now)
            {
                result = "生日不得大於今天";
                return false;

            }
            result = "";
            return true;
        }

        /// <summary>
        /// 證件總類(name)驗證
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool VaildIDType(string str, out string result)
        {
            //這邊不做空白驗證 出現空白丟出去讓空白驗證驗
            if (string.IsNullOrEmpty(str))
            {
                result = "";
                return true;
            }
            CodeListBLL bll = new CodeListBLL();
            List<CodeName> codeNames = bll.GetCodeList("000", "SYS01005");
            int cnt = codeNames.Where(x => x.Name == str.Trim()).Count();
            if (cnt == 0)
            {
                result = "無此證件總類";
                return false;
            }
            result = "";
            return true;
        }

        /// <summary>
        /// 證件種類(code)驗證
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool VaildIDTypeCode(string str, out string result)
        {
        
            if (string.IsNullOrEmpty(str))
            {
                result = "";
                return true;
            }
            CodeListBLL bll = new CodeListBLL();
            List<CodeName> codeNames = bll.GetCodeList("000", "SYS01005");
            int cnt = codeNames.Where(x => x.Code == str.Trim()).Count();
            if (cnt == 0)
            {
                result = "無此證件種類";
                return false;
            }

            result = "";
            return true;
        }

        /// <summary>
        /// 統一編號驗證
        /// </summary>
        /// <param name="uniId"></param>
        /// <returns></returns>
        public static bool ValidUniID(string str, out string result)
        {
            result = "";
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            int uni = 0;
            int[] validCode = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };
            bool digIs7 = false;
            if (str.Length != 8)
            {
                result = "長度錯誤";
                return false;
            }
            char[] arr = str.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                int dig = int.Parse(arr[i].ToString());
                dig = validCode[i] * dig;
                if (dig > 9)
                    dig = (dig / 10) + (dig % 10);

                if (i == 6 && dig == 7)
                    digIs7 = true;
                else
                    uni += dig;
            }

            if (uni % 10 == 0)
                return true;

            if (digIs7)
            {
                uni = uni + 1;
                if (uni % 10 == 0)
                    return true;
            }

            result = "格式錯誤";
            return false;
        }


        public static bool ValidPassword(string str, out string result)
        {
            Regex notAllBeCharacters = new Regex("[^A-Za-z]");
            Regex notAllBeNumbers = new Regex("[^0-9]");
            result = "";
            if (str.Trim().Length < 8)
            {
                result += "密碼不足8碼\n";
            }
            else if (str.Trim().Length > 16)
            {
                result += "密碼超過16碼\n";
            }
            else if (!notAllBeCharacters.Match(str).Success)
            {
                result += "密碼不得皆為英文\n";
            }
            else if (!notAllBeNumbers.Match(str).Success)
            {
                result += "密碼不得皆為數字\n";
            }

            return string.IsNullOrWhiteSpace(result);
        }

        /// <summary>
        /// 電話驗證
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool VaildTel(string str, out string result)
        {
            //這邊不做空白驗證 出現空白丟出去讓空白驗證驗
            if (string.IsNullOrEmpty(str))
            {                
                result = "";
                return true;
            }
            if (!Regex.IsMatch(str, @"^[0-9-+()#]*$"))
            {
                result = "電話號碼格式錯誤";
                return false;
                
            }
            result = "";
            return true;
        }

        /// <summary>
        /// 電子郵件驗證
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool VaildEMail(string str, out string result)
        {
            //這邊不做空白驗證 出現空白丟出去讓空白驗證驗
            if (string.IsNullOrEmpty(str))
            {
                result = "";
                return true;
            }
            if (!Regex.IsMatch(str, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                result = "EMAIL格式錯誤";
                return false;
               
            }
            result = "";
            return true;
        }

        /// <summary>
        /// 特殊字元驗證
        /// </summary>
        /// <param name="str"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool VaildSPChar(string str, out string result)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                result = "";
                return true;
            }
            if (Regex.IsMatch(str, @"[%!&*=+'""?<>\\]+"))
            {
                result = "特殊字元請移除";
                return false;                
            }
            result = "";
            return true;
        }
       
        /// <summary>
        /// 身分證號驗證
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errModel"></param>
        public static bool VaildID(string str, out string result)
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
                    result = "身分證字號錯誤";
                    return false;
                }

            }
            else
            {
                result = "身分證格式不正確";
                return false;               
            }
            result = "";
            return true;
        }

        /// <summary>
        /// 航商(code)驗證
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool VaildCID(string str, out string result)
        {

            if (string.IsNullOrEmpty(str))
            {
                result = "不可空白";
                return false;
            }
            VendorBLL vdbll = new VendorBLL();
            List<CodeName> codeNames = vdbll.GetVendorDropDown();
            string[] arr = str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string s in arr)
            {
                int cnt = codeNames.Where(x => x.Code == s.Trim()).Count();
                if (cnt == 0)
                {
                    result = "無此航商";
                    return false;
                }
            }
            result = "";
            return true;
        }

        /// <summary>
        /// 去程日期驗證
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool VaildOrderDate(string str, out string result)
        {
            DateTime date = new DateTime();
            DateTime now = DateTime.Parse(DateTime.Now.AddDays(1d).ToShortDateString());
            if (!DateTime.TryParseExact(str, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                result = "去程日期格式錯誤";
                return false;
            }
            if (date < now)
            {
                result = "去程日期不得小於明天";
                return false;
                
            }
            result = "";
            return true;

        }

        /// <summary>
        /// 必填驗證
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errModel"></param>
        public static bool VaildRequired(string str, out string result)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                result = "不可空白";
                return false;               
            }

            result = "";
            return true;

        }

        /// <summary>
        /// 長度驗證
        /// </summary>
        /// <param name="str"></param>
        /// <param name="errModel"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        public static bool VaildLength(string str, int? minLength, int? maxLength, out string result)
        {
            int strLength = str.Length;
            if (minLength.HasValue)
            {
                if (strLength < minLength.Value)
                {
                    result = string.Format("長度不足{0}字", minLength.Value);
                    return false;
                } 
            }

            if (maxLength.HasValue)
            {
                if (strLength > maxLength.Value)
                {
                    result = string.Format("長度超過{0}字", minLength.Value);
                    return false;
                }                    
            }
            result = "";
            return true;
        }

        #region ErrorModel
        public static void VaildID(string str, ref ErrorModel error)
        {
            string result = "";
            if (!VaildIDType(str, out result))
            {
                error.msg = result;
                return;
            }
        }

        public static void VaildIDType(string str, ref ErrorModel error)
        {
            string result = "";
            if (!VaildRequired(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildSPChar(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildIDType(str, out result))
            {
                error.msg = result;
                return;
            }
        }

        public static void VaildIDTypeCode(string str, ref ErrorModel error)
        {
            string result = "";
            if (!VaildRequired(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildSPChar(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildIDTypeCode(str, out result))
            {
                error.msg = result;
                return;
            }
        }

        public static void VaildCID(string str, ref ErrorModel error)
        {
            string result = "";
            if (!VaildRequired(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildSPChar(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildCID(str, out result))
            {
                error.msg = result;
                return;
            }
        }

        public static void VaildTel(string str, ref ErrorModel error)
        {
            string result = "";
            if (!VaildRequired(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildSPChar(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildTel(str, out result))
            {
                error.msg = result;
                return;
            }
        }

        public static void VaildEMail(string str, ref ErrorModel error)
        {
            string result = "";
            if (!VaildRequired(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildSPChar(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildEMail(str, out result))
            {
                error.msg = result;
                return;
            }
        }

        public static void VaildOrderDate(string str, ref ErrorModel error)
        {
            string result = "";
            if (!VaildRequired(str, out result))
            {
                error.msg = result;
                return;
            }
            if (!VaildOrderDate(str, out result))
            {
                error.msg = result;
                return;
            }
        }

        public static void VaildBirthday(DateTime date, ref ErrorModel error)
        {
            string result = "";
            if (!VaildBirthday(date, out result))
            {
                error.msg = result;
                return;
            }
          
        }
        #endregion
    }
}
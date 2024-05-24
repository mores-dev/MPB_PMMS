using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MPB_BLL.COMMON
{
    public class DataMask
    {
        /// 

        /// 資料遮罩
        /// 
        ///需遮罩之字串
        ///選擇需遮罩之型態:手機,地址....
        /// 加密過後的字串
        public static string MaskValue(string val, MaskType Type)
        {

            switch (Type)
            {
                case MaskType.Name:
                    val = MaskName(val);
                    break;
                //case "Creditcard":
                //    val = MaskCreditcard(val);
                //    break;
                case MaskType.Addr:
                    val = MaskAddr(val);
                    break;
                case MaskType.Id:
                    val = MaskId(val);
                    break;
                case MaskType.Tel:
                    val = MaskTel(val);
                    break;
                default:
                    break;
            }

            return val;
        }
        /// 

        /// 姓名遮罩包含英文姓名
        /// 
        ///
        /// 姓名
        /// 
        /// 王O名
        private static string MaskName(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                string maskstr, maskchar;
                maskchar = null;
                if (Regex.IsMatch(val, "[A-Za-z]"))
                {
                    if (val.IndexOf("-") > 1)
                    {
                        maskstr = val.Split('-')[1];
                        val = val.Replace(maskstr, "*");
                    }else if (val.IndexOf(" ") > 1)
                    {
                        maskstr = val.Split(' ')[1];
                        val = val.Replace(maskstr, "*");
                    }
                    else
                    {
                        int End = (int)(val.Length / 2);
                        maskstr = val.Substring(1, End);
                        for (int i = 0; i < maskstr.Length; i++)
                        {
                            maskchar = maskchar + "*";
                        }
                        val = val.Replace(maskstr, maskchar);
                    }
                }
                else
                {

                    int End = (int)(val.Length / 2);
                    maskstr = val.Substring(1, End);
                    for (int i = 0; i < maskstr.Length; i++)
                    {
                        maskchar = maskchar + "*";
                    }
                    val = val.Replace(maskstr, maskchar);
                }
            }
            else
            {

                val = "";
            }

            return val;
        }
        /// 

        /// 信用卡遮罩--信用卡號=>前6後4不mask
        /// 
        ///5555-2525-1266-2213
        /// 
        /// 5555-25**-****-2213
        ///
        //private static string MaskCreditcard(string val)
        //{
        //    if (!string.IsNullOrEmpty(val))
        //    {

        //        val = (Regex.IsMatch(val.Replace("-", ""), RegularExp.creditcard) ? val.Substring(0, 7) + "**-****" + val.Substring(14, 5) : "");

        //    }
        //    else
        //    {

        //        val = "";
        //    }
        //    return val;

        //}
        /// 

        /// 地址=>留前6個字(Unicode)
        /// 
        ///台南市中正路321巷7弄17號2F
        /// 台南市中正路***********
        private static string MaskAddr(string val)
        {
            string maskstr, maskchar;
            maskchar = null;
            if (!string.IsNullOrEmpty(val))
            {
                int End = (int)(val.Length - 6);
                maskstr = val.Substring(6, End);
                for (int i = 0; i < maskstr.Length; i++)
                {
                    maskchar = maskchar + "*";
                }
                val = val.Replace(maskstr, maskchar);

            }
            else
            {

                val = "";
            }
            return val;


        }
        /// 

        /// 身分證字號=>後四碼前的前四碼隱碼
        /// 
        ///A123456789
        ///A1****6789
        private static string MaskId(string val)
        {
            string maskstr, maskchar;
            maskchar = "****";
            if (!string.IsNullOrEmpty(val))
            {
                if (val.Length>=8)
                {
                    int end = (int)(val.Length - 4);
                    int start = (int)(val.Length - 8);
                    maskstr = val.Substring(0, start) +maskchar + val.Substring(end);
                   
                }
                else
                {

                    maskstr = "";

                }
            }
            else
            {

                maskstr = "";
            }
            return maskstr;


        }
        /// 

        /// 聯絡方式(電話) =>固定遮後四碼
        /// 
        ///02-12345678
        /// 02-1234****
        private static string MaskTel(string val)
        {
            string maskstr = "";
            string maskchar = "****";
            if (!string.IsNullOrEmpty(val))
            {
                if(val.Length>4)
                {
                    int Strl = val.Length - 4;
                    maskstr = val.Substring(0, Strl) + maskchar;
                }
            }
            else
            {
                maskstr = "";
            }
            return maskstr;

        }
    }

    public enum MaskType
    {
        Name,
        Creditcard,
        Addr,
        Id,
        Tel
    }

   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPB_Entities;
using NLog;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;


namespace MPB_BLL
{
    public class BLLBase
    {
        /// <summary>
        /// NLog
        /// </summary>
        //protected NLog.Logger Logger;
        protected static Logger logger = LogManager.GetCurrentClassLogger();

        public void ExceptionLog(Exception x)
        {
            logger.Warn(String.Format("{0} \r\n{1} \r\n", x.Message, x.StackTrace));
        }

        /// <summary>
        /// 10進制轉36進制
        /// </summary>
        /// <param name="i">10進制值</param>
        /// <returns>36進制值</returns>
        public string ConvertTo36(int i)
        {
            string s = "";
            int j = 0;
            while (i >= 36)
            {
                j = i % 36;
                if (j < 10)
                    s += j.ToString();
                else
                    s += Convert.ToChar(j + 87);
                i = i / 36;
            }
            if (i < 10)
                s += i.ToString();
            else
                s += Convert.ToChar(i + 87);
            Char[] c = s.ToCharArray();
            Array.Reverse(c);
            return Convert.ToString(new string(c)).ToUpper();
        }

        /// <summary>
        /// 36進制轉10進制
        /// </summary>
        /// <param name="i">36進制值</param>
        /// <returns>36進制值</returns>
        public int From36ToDecimal(string s)
        {
            const string CharList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var reversed = s.ToUpper().Reverse();
            int result = 0;
            int pow = 0;
            foreach (char c in reversed)
            {
                result += CharList.IndexOf(c) * (int)Math.Pow(36, pow);
                pow++;
            }
            return result;
        }

        public string GetSM_SN_Like(string ticketNo)
        {
            int year = From36ToDecimal(ticketNo.Substring(0, 1));
            int mon = From36ToDecimal(ticketNo.Substring(1, 1));
            int day = From36ToDecimal(ticketNo.Substring(2, 1));
            return (20 + year).ToString() + mon.ToString("00") + day.ToString("00");
        }

        public static string DateOfWeek_SimpleTW(DateTime dt)
        {
            string[] Day = new string[] { "(日)", "(一)", "(二)", "(三)", "(四)", "(五)", "(六)" };
            return Day[Convert.ToInt32(dt.DayOfWeek)];
        }

        public SqlParameter GetSqlParam(string key, string value, string emptyReplace = "")
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = key;
            param.Value = string.IsNullOrWhiteSpace(value) ? emptyReplace : value;
            return param;
        }

        protected string ColumnCheck_Datetime(string dt)
        {
            DateTime tmp;
            if (DateTime.TryParse(dt, out tmp))
                dt = tmp.ToString("yyyy/MM/dd");
            else
                dt = "";
            return dt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPB_Entities.COMMON;

namespace MPB_DAL.COMMON
{
    public class UserLogDAL : DALBase
    {

        public UserLogDAL() { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="db">DbManager</param>
        public UserLogDAL(DbManager db) : base(db) { }
        string sql = "";

        /// <summary>
        /// 寫入LOG_IDRECORD
        /// </summary>
        /// <param name="ProgId"></param>
        /// <returns></returns>
        public int Insert_LogRecord(UserLog userEdit)
        {

            sql = "";
            sql += " INSERT INTO LogRecord (";
            //類別
            sql += " LogType";
            //系統類別
            sql += ", LogSys";
            //異動日期
            sql += ", LogDT";
            //使用者代號
            sql += ", UserID";
            //程式代號
            sql += ", ProgID";
            //程式功能
            sql += ", ProgFunc";
            //資料Key值
            sql += ", KeyData";
            //登入IP
            sql += ", LoginIP)";

            sql += " select ";
            //類別
            sql += " @LogType";
            //系統類別
            sql += ", 'C'";
            //異動日期
            sql += ", GETDATE()";
            //使用者代號
            sql += ", @UserId";
            //程式代號
            sql += ", ''";
            //程式功能
            sql += ", ''";
            //資料Key值
            sql += ", ''";
            //登入IP
            sql += ", @LoginIp";
            return Execute(@sql, userEdit);
        }
    }
}

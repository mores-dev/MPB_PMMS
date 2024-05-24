
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MPB_DAL;
using MPB_Entities.Api;

namespace MPB__DAL.Api
{
    public class WriteAPILog_DAL : DALBase
    {
        public WriteAPILog_DAL() { }

        public WriteAPILog_DAL(DbManager db) : base(db) { }

        public void InsertData(WriteAPILog_Entities ent)
        {
            string sql = "";

            sql += "Insert Into APILog (";
            sql += "  DeviceType";
            sql += ", DeviceID";
            sql += ", FUNC_ID";
            sql += ", sJSON";
            sql += ", RSPN_CODE";
            sql += ", RSPN_MSG";
            sql += ", rJSON";
            sql += ", CREATEDT )";
            sql += " Select";
            sql += "  @DeviceType";
            sql += ", @DEVICE_ID";
            sql += ", @FuncId";
            sql += ", @sJson";
            sql += ", @RspnCode";
            sql += ", @RspnMsg";
            sql += ", @rJson";
            sql += ", GetDate()";

            Execute(@sql, ent);
        }
    }
}

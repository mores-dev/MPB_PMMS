using System;
using System.Collections.Generic;
using MPB_Entities.COMMON;

namespace MPB_DAL.COMMON
{
    public class CodeListDAL : DALBase
    {
        public CodeListDAL() { }

        public CodeListDAL(DbManager db) : base(db) { }

        public List<CodeName> GetCodeList(string branch, string code_key)
        {
            return Fetch<CodeName>(@"Select CodeID code, CodeDesc name From cCodeList
                Where CodeKey = @1 and Status = '1' order by CodeOrder", branch, code_key);
        }

        public List<CodeName> GetCodeList_SpsInfo()
        {
            string sql = "";

            sql += "Select SPS_ID as code, SPS_NAME as name";
            sql += "\n" + " From cSpsInfo ";
            sql += " Order By SEQ ";
            return Fetch<CodeName>(@sql);
        }

        public List<CodeName> GetCodeList_DeviceType(bool cdps = false, string inStr = "")
        {
            string sql = "";
            sql += "Select DeviceTypeID as code, DeviceTypeDesc as name";
            sql += "\n" + " From cDeviceType ";
            sql += " Where 1 = 1";
            if (!cdps)
                sql += " And DeviceTypeID != 'C' ";
            if (!string.IsNullOrWhiteSpace(inStr))
                sql += " AND DeviceTypeID IN " + inStr ;

            return Fetch<CodeName>(@sql);
        }

        public List<CodeName> GetCodeList_DeviceId(bool singleType, string typeStr, string spsStr)
        {
            string sql = "";
            sql += " Select DEVICE_ID as code, DEVICE_NAME as name ";
            sql += "   FROM cStationConf ";
            
            sql += "  Where 1 = 1";

            if (!string.IsNullOrWhiteSpace(typeStr))
            {
                if (singleType)
                    sql += " AND DeviceTypeID = @0";
                else
                    sql += " AND DeviceTypeID IN " + typeStr;
            }
            if (!string.IsNullOrWhiteSpace(spsStr))
                sql += " AND SPS_ID = @1";

            sql += "  Order By SPS_ID, DEVICE_ID ";

            return Fetch<CodeName>(@sql, typeStr, spsStr);
        }

        public string GetDeviceId_Name(string deviceId)
        {
            string sql = "";

            sql += "Select ISNULL(DEVICE_NAME, '(未設定)') as name";

            sql += "  From cStationConf";
            sql += " Where 1 = 1";

            sql += "   And DEVICE_ID = @0 ";

            return SingleOrDefault<string>(@sql, deviceId);
        }

        public int Check_DEVICE_ID(string deviceId)
        {
            string sql = "";

            sql += "Select Count(*) From cStationConf";
            sql += " Where 1 = 1";
            sql += "   AND DEVICE_ID = @0 ";

            return SingleOrDefault<int>(@sql, deviceId);
        }

        public List<CodeName> GetCodeList_RoleId()
        {
            string sql = "";

            sql += " Select A.RoleID as code, A.RoleName as name";

            sql += "  From cRoleInfo A";

            sql += " Where 1 = 1";
            sql += "   And RoleID in ('1000', '1002', '2000', '2001', '4000') ";

            sql += " Order By A.RoleID";
            
            return Fetch<CodeName>(@sql);
        }

        public List<CodeName> GetCodeList_Company(bool excludeMPB)
        {
            string sql = "";

            sql += " Select A.C_ID as code, A.C_NAME as name";

            sql += "  From cCompany A";

            sql += " Where 1 = 1";
            sql += " And C_STATUS = '1'";
            if (excludeMPB)
            {
                sql += " And C_ID NOT IN ('C00000')";
            }
            sql += " Order By A.C_ID";

            return Fetch<CodeName>(@sql);
        }
    }
}

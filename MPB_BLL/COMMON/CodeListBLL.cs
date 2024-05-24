using MPB_DAL.COMMON;
using MPB_Entities.COMMON;
using System.Collections.Generic;

namespace MPB_BLL.COMMON
{
    public class CodeListBLL : BLLBase
    {
        CodeListDAL _dal = new CodeListDAL();
        public List<CodeName> GetCodeList(string branch, string codeKey)
        {
            return _dal.GetCodeList(branch, codeKey);
        }

        public List<CodeName> GetCodeList_SpsInfo()
        {
            return _dal.GetCodeList_SpsInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cdps">include cdps or not </param>
        /// <param name="inStr">seperate by "," </param>
        /// <returns></returns>
        public List<CodeName> GetCodeList_DeviceType(bool cdps = false, string inStr = "")
        {
            string inSql = ConstructInStr(inStr);

            return _dal.GetCodeList_DeviceType(cdps, inSql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeStr">Specific DeviceType, seperate by "," </param>
        /// <param
        /// <returns></returns>
        public List<CodeName> GetCodeList_DeviceId(bool singleType, string typeStr = "", string spsStr = "")
        {
            string typeSql = typeStr;
            if (!singleType)
                typeSql = ConstructInStr(typeStr);

            return _dal.GetCodeList_DeviceId(singleType, typeSql, spsStr);
        }

        private string ConstructInStr(string inStr)
        {
            if (string.IsNullOrWhiteSpace(inStr)) return "";
            
            string rtn = "";
            string[] inArr = inStr.Split(',');
            rtn += " ('";
            for (int i = 0; i < inArr.Length; i++)
            {
                if (i != 0)
                    rtn += ", '";
                rtn += inArr[i] + "'";
            }
            rtn += ") ";
            return rtn;
        }

        public List<CodeName> GetCodeList_RoleId()
        {
            return _dal.GetCodeList_RoleId();
        }

        public List<CodeName> GetCodeList_Company(bool excludeMPB = true)
        {
            return _dal.GetCodeList_Company(excludeMPB);
        }
    }
}

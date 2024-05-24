using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;
using MPB_BLL.COMMON;
using MPB_DAL.FCM;
using MPB_Entities.FCM;

namespace MPB_BLL.FCM
{
    public class PsgrRegisterList_QueryBLL : BLLBase
    {
        PsgrRegisterList_QueryDAL _dal = new PsgrRegisterList_QueryDAL();

        //public DataSet GetPrint1List(PsgrRegisterList_QueryCondition qc)
        //{
        //    DbParameter[] parms = new DbParameter[2];

        //    parms[0] = GetSqlParam("ODR_DT", qc.OdrDt);

        //    parms[1] = GetSqlParam("ODR_TYPE", qc.OdrType);


        //    return _dal.GetPrint1List(parms);
        //}
        public DataTable GetPrint1List(PsgrRegisterList_QueryCondition qc)
        {
            DbParameter[] parms = new DbParameter[3];

            parms[0] = GetSqlParam("ODR_DT", qc.OdrDt);

            parms[1] = GetSqlParam("ODR_TYPE", qc.OdrType, "AL");

            parms[2] = GetSqlParam("C_ID", qc.C_ID, "AL");

            DataTable rtn = _dal.GetPrint1List(parms).Tables[0];
            foreach(DataRow row in rtn.Rows)
            {
                if (EnDeCode.DecryptAES256(row["ID_NO"].ToString(), out string s, KeyType.IDNO))
                    row["ID_NO"] = DataMask.MaskValue(s, MaskType.Id);
            }

            return rtn;
        }
    }
}

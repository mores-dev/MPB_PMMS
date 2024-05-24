using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Linq;
using MPB_BLL.COMMON;
using MPB_DAL.FCM;
using MPB_Entities.FCM;
using System.Collections.Generic;
using MPB_Entities.COMMON;

namespace MPB_BLL.FCM
{
    public class PsgrManifest_QueryBLL : BLLBase
    {
        PsgrManifest_QueryDAL _dal = new PsgrManifest_QueryDAL();

        public DataTable GetPrint1List(PsgrManifest_QueryCondition qc)
        {
            DbParameter[] parms = new DbParameter[4];

            parms[0] = GetSqlParam("SHIPPING_DT", qc.ShippingDt);

            parms[1] = GetSqlParam("C_ID", qc.C_ID, "AL");

            parms[2] = GetSqlParam("STATION", qc.Station, "AL");

            parms[3] = GetSqlParam("VOYAGE_TIME", qc.VoyageTime, "AL");

            DataTable rtn = _dal.GetPrint1List(parms).Tables[0];

            foreach(DataRow row in rtn.Rows)
            {
                if (EnDeCode.DecryptAES256(row["ID_NO"].ToString(), out string s, KeyType.IDNO))
                    row["ID_NO"] = DataMask.MaskValue(s, MaskType.Id).ToUpper();
            }

            return rtn;
        }

        public List<CodeName> GetVoyageTime(PsgrManifest_QueryCondition qc)
        {
            return _dal.SelectVoyageTime(qc);
        }

    }
}

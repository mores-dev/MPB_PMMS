using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using MPB_BLL.COMMON;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;

namespace MPB_BLL.FCM
{
    public class PsgrConfirm_QueryBLL : BLLBase
    {
        PsgrConfirm_QueryDAL _dal = new PsgrConfirm_QueryDAL();

        public PageList<PsgrConfirm_QueryResult> GetPageList(PsgrConfirm_QueryCondition qc)
        {
            qc.Station = "";
            qc.VoyageTime = "";
            qc.VesselId = "";
            if (string.IsNullOrWhiteSpace(qc.QueryStr))
                return new PageList<PsgrConfirm_QueryResult>();

            string[] strArr = qc.QueryStr.Split('|');
            if (strArr.Length == 4)
            {
                qc.C_ID = strArr[0];
                qc.Station = strArr[1];
                qc.VoyageTime = strArr[2];
                qc.VesselId = strArr[3];
            }

            PageList<PsgrConfirm_QueryResult> rtn = _dal.GetPageList(qc);
            string Key = ConfigurationManager.AppSettings["QueryRegisterDtlKey"].PadRight(32, '0');
            rtn.Items.ForEach(x => { if (EnDeCode.DecryptAES256(x.IdNo, out string dcrpt, KeyType.IDNO)) x.IdNo = dcrpt; });
            rtn.Items.ForEach(x => {x.IdNo = DataMask.MaskValue(x.IdNo, MaskType.Id); });

            return rtn;
        }

        public List<CodeName> GetQueryStr(PsgrConfirm_QueryCondition qc)
        {
            return _dal.GetQueryStr(qc);
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MPB_DAL.FCM;
using MPB_Entities.FCM;

namespace MPB_BLL.FCM
{
    public class NCCRStatistic_QueryBLL : BLLBase
    {
        NCCRStatistic_QueryDAL _dal = new NCCRStatistic_QueryDAL();

        public DataSet GetPrint1List(NCCRStatistic_QueryCondition qc)
        {
            DbParameter[] parms = new DbParameter[2];
            SqlParameter param0 = new SqlParameter();
            param0.ParameterName = "ODR_DT_START";
            param0.Value = qc.OdrDtStart;
            parms[0] = param0;

            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "ODR_DT_END";
            param1.Value = qc.OdrDtEnd;
            parms[1] = param1;

            return _dal.GetPrint1List(parms);
        }
    }
}

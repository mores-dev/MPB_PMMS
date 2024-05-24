using System.Data;
using System.Data.Common;

namespace MPB_DAL.FCM
{
    public class NCCRStatistic_QueryDAL : DALBase
    {
        public NCCRStatistic_QueryDAL() { }

        public NCCRStatistic_QueryDAL(DbManager db) : base(db) { }

        public DataSet GetPrint1List(DbParameter[] parms)
        {
            return ExecuteStoredProcedure("SP_Report_NCCRStatistic", parms);
        }
    }
}

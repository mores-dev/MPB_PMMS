using System.Data;
using System.Data.Common;

namespace MPB_DAL.FCM
{
    public class PsgrRegisterList_QueryDAL : DALBase
    {
        public PsgrRegisterList_QueryDAL() { }

        public PsgrRegisterList_QueryDAL(DbManager db) : base(db) { }

        public DataSet GetPrint1List(DbParameter[] parms)
        {
            return ExecuteStoredProcedure("SP_Report_PsgrRegisterList", parms);
        }
    }
}

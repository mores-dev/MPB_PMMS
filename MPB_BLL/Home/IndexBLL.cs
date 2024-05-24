using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;
using MPB_DAL.COMMON;
using MPB_DAL.Home;
using MPB_Entities.COMMON;

namespace MPB_BLL.Home
{
    public class IndexBLL : BLLBase 
    {
        IndexDAL _dal = new IndexDAL();
        
        public List<CodeName> GetSubSystem(string userId)
        {
            return _dal.GetSubSystem(userId);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;
using MPB_DAL.Home;
using MPB_Entities.Home;

namespace MPB_BLL.Home
{
    public class LtreeBLL : BLLBase
    {

        LtreeDAL _dal = new LtreeDAL();

        public List<LtreeQueryResult> GetLtree(string userId)
        {
            var li = _dal.GetList(userId);

            return li;
        }

    }
}

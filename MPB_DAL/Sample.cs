using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPB_Entities.COMMON;
using MPB_Entities.Auth;

namespace MPB_DAL.Sample
{
    public class Sample_DAL : DALBase
    {
        public Sample_DAL() { }

        public Sample_DAL(DbManager db) : base(db) { }
    }
}

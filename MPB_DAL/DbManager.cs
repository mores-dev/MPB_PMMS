using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;
using NLog;

namespace MPB_DAL
{
    public partial class DbManager : Database
    {
        [ThreadStatic]
        private static DbManager _instance;

        public DbManager()
            : base("MPB_PMMS")
        {
            _connectionString = _connectionString.Replace("PPPPPPPP", "P"+"a"+"s"+"s"+"w"+"o"+"r"+"d=" + "Mds31275691$");
            CommonConstruct();
        }

        public DbManager(string connectionStringName)
            : base(connectionStringName)
        {
            CommonConstruct();
        }

        public static IFactory Factory { get; set; }
        partial void CommonConstruct();

        public static DbManager GetInstance()
        {
            if (_instance != null)
                return _instance;

            if (Factory != null)
                return Factory.GetInstance();
            return new DbManager();
        }

        //public override void OnBeginTransaction()
        //{
        //    if (_instance == null)
        //        _instance = this;
        //}

        //public override void OnEndTransaction()
        //{
        //    if (_instance == this)
        //        _instance = null;
        //}

        public interface IFactory
        {
            DbManager GetInstance();
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void OnExecutingCommand(System.Data.IDbCommand cmd)
        {
            logger.Debug(String.Format("{0}\r\n", FormatDbCommand(cmd)));

            base.OnExecutingCommand(cmd);
        }

        public override bool OnException(Exception x)
        {
            logger.Warn(String.Format("{0} \r\n{1} \r\n", x.Message, x.StackTrace));

            return base.OnException(x);
        }
    }
}

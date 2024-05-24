using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessUtility
{
    //Database of source must add partial too
    public partial class Database
    {
        public string FormatDbCommand(IDbCommand cmd)
        {
            if (cmd == null)
                return "Command Not found";

            var sb = new StringBuilder();
            string sql = cmd.CommandText;
            var args = cmd.Parameters;

            if (sql == null)
                return "CommandText is Empty";
            sb.Append(sql);

            if (args != null && args.Count > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < args.Count; i++)
                {
                    var arg = args[i] as DbParameter;

                    sb.AppendFormat("\t -> {0}{1} [{2}] = \"{3}\"\t--{4} {5}\n", _paramPrefix, i, arg == null ? "null" : arg.DbType.ToString()
                        , (arg == null || arg.Value == null) ? "null" : arg.Value.ToString(), args[i], arg == null ? "null" : arg.Direction.ToString());
                }
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString(); ;
        }

        //for display store produce parameter name
        IDbCommand _lastDbCommand;

        //DoPreExecute function must add '_lastDbCommand = cmd;' in last line
        public string LastDbCommand
        {
            get { return FormatDbCommand(_lastDbCommand); }
        }
    }
}

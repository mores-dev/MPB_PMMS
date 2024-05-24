using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.Odbc;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;
using DataAccessUtility.Internal;

namespace DataAccessUtility
{
    //Database of source must add partial too
    public partial class Database
    {
        public void ExecuteProcedure(string procName, params DbParameter[] args)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateProcedureCommand(_sharedConnection, procName, args))
                    {
                        cmd.ExecuteNonQuery();
                        OnExecutedCommand(cmd);
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
            }
        }

        public IDbCommand CreateProcedureCommand(IDbConnection connection, string procName, params DbParameter[] args)
        {
            // Prebuild the sql
            object[] param = (object[])args;
            //_dbType.PreBuildCommand(ref procName, ref param);

            // Create the command and add parameters
            IDbCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = procName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = _transaction;
            foreach (var item in args)
            {
                //AddParam(cmd, item, null);
                if (item != null)
                {
                    if ((item.Direction == ParameterDirection.InputOutput || item.Direction == ParameterDirection.Input) &&
                        item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(item);
                }
            }

            // Notify the DB type
            _dbType.PreExecute(cmd);

            // Call logging
            if (!String.IsNullOrEmpty(procName))
                DoPreExecute(cmd);

            return cmd;
        }


        public DataSet ExecuteStoredProcedure(string procName, params DbParameter[] args)
        {
            DataSet ds = new DataSet();
            try
            {
                OpenSharedConnection();
                try
                {
                    using (SqlCommand cmd = CreateProcedureCommand(_sharedConnection, procName, args) as SqlCommand)
                    {
                        //cmd.ExecuteNonQuery();
                        //OnExecutedCommand(cmd);
                        
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds);
                        }
                        OnExecutedCommand(cmd);
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
            }
            return ds;
        }

    }
}

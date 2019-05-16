using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace ETS.Data.SqlHelper
{
    public class SqlHelper : IDisposable
    {
        private SqlConnection con;
        private bool disposed;
        private SqlTransaction transaction;
        private SqlCommand cmd;
        private static readonly string connectionstring = "ConnectionString";//ConfigurationManager.ConnectionStrings[""].ConnectionString;
       
        public SqlHelper()
            : this(connectionstring)
        {

        }

        public SqlHelper(SqlConnection con)
        {
            this.con = con;
        }

        public SqlHelper(string sectionName)
        {
            string name = String.IsNullOrEmpty(sectionName) ? connectionstring : sectionName;
            string connString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            this.con = new SqlConnection(connString);
        }

        public DataTable ExecuteDataTable(string cmdText, params SqlParameter[] parameters)
        {
            cmd = new SqlCommand(cmdText, con);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }

            cmd.CommandText = cmdText;
            cmd.Parameters.AddRange(parameters);
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        ///  返回DateSet
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet DateSetQuery(string SQLString, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommands(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }
        private static void PrepareCommands(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }




        public SqlCommand PrepareSqlCommand(string commandText, CommandType commandType)
        {
            cmd = new SqlCommand(commandText, con);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            cmd.CommandType = commandType;
            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }
            return cmd;
        }

        public SqlCommand PrepareTextSqlCommand(string commandText)
        {
            return PrepareSqlCommand(commandText, CommandType.Text);
        }

        public SqlCommand PrepareStoredSqlCommand(string commandText)
        {
            return PrepareSqlCommand(commandText, CommandType.StoredProcedure);
        }


        public void BeginTrancation()
        {
            if (this.con.State != ConnectionState.Open)
            {
                this.con.Open();
            }
            this.transaction = this.con.BeginTransaction();
        }

        public void Commit()
        {
            this.transaction.Commit();

        }

        public void Rollback()
        {
            this.transaction.Rollback();

        }


        public static T ExecSinleReader<T>(SqlCommand cmd, Func<IDataReader, T> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return func(reader);
                }
                return default(T);
            }
        }

        public static List<T> ExecCollectionReader<T>(SqlCommand cmd, Func<IDataReader, T> func)
        {
            using (var reader = cmd.ExecuteReader())
            {
                List<T> listT = new List<T>();
                while (reader.Read())
                {
                    listT.Add(func(reader));
                }
                return listT;
            }
        }


        public T TransactionHelper<T>(Func<T> func)
        {
            bool currentTrascation = (null == Transaction.Current);
            if (currentTrascation)
            {
                this.BeginTrancation();
            }
            try
            {
                T result = func();
                if (currentTrascation)
                {
                    this.Commit();
                }
                return result;
            }
            catch
            {
                if (currentTrascation)
                {
                    this.Rollback();
                }
                throw;
            }
        }


        #region IDisposable

        ~SqlHelper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                if (this.con != null)
                {
                    con.Close();
                    con.Dispose();
                }
                if (this.cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
                if (this.transaction != null)
                {
                    this.transaction.Dispose();
                }
            }
            this.con = null;
            this.cmd = null;
            this.transaction = null;
            this.disposed = true;
        }

        #endregion

        //internal int ExecSqlHelper<T1>(Func<SqlHelper, int> func)
        //{
        //    throw new NotImplementedException();
        //}

    }


    //public static class SqlHelperExtend
    //{



    //}
}

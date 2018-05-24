using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.SQLHelper
{
    /// <summary>
    /// In this enum you can define your coonection string name that same as webconfig connectionstring
    /// </summary>
    public enum ConnectionString
    {
        CSharpHelpers
    }

    /// <summary>
    /// The SqlHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>s
    public class SqlHelper
    {
        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region ExecuteNonQuery
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="cnStr"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            int retVal = 0;

            try
            {
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandType = cmdType;
                CMD.CommandTimeout = 0;

                if (CN.State == ConnectionState.Closed)
                    CN.Open();

                retVal = Convert.ToInt32(CMD.ExecuteNonQuery());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
            }

            return retVal;
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[] Params, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            int retVal = 0;

            try
            {
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandType = cmdType;
                CMD.CommandTimeout = 0;

                foreach (SqlParameter Param in Params)
                {
                    CMD.Parameters.Add(Param);
                }

                if (CN.State == ConnectionState.Closed)
                    CN.Open();

                retVal = Convert.ToInt32(CMD.ExecuteNonQuery());
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
            }

            return retVal;
        }

        #endregion

        #region ExecuteReader

        public static SqlDataReader ExecuteReader(string sql, SqlParameter[] Params, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            SqlDataReader DR;

            try
            {
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandType = cmdType;
                CMD.CommandTimeout = 0;

                foreach (SqlParameter Param in Params)
                {
                    CMD.Parameters.Add(Param);
                }

                if (CN.State == ConnectionState.Closed)
                    CN.Open();

                DR = CMD.ExecuteReader();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
            }

            return DR;
        }

        public static SqlDataReader ExecuteReader(string sql, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            SqlDataReader DR;

            try
            {
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandType = cmdType;
                CMD.CommandTimeout = 0;

                if (CN.State == ConnectionState.Closed)
                    CN.Open();

                DR = CMD.ExecuteReader();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
            }

            return DR;
        }

        #endregion

        #region ExecuteScalar

        public static object ExecuteScalar(string sql, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            object retVal;

            try
            {
                CMD.CommandType = cmdType;
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandTimeout = 0;

                if (CN.State == ConnectionState.Closed)
                    CN.Open();

                retVal = CMD.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
            }

            return retVal;
        }

        public static object ExecuteScalar(string sql, SqlParameter[] Params, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            object retVal;

            try
            {
                CMD.CommandType = cmdType;
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandTimeout = 0;

                foreach (SqlParameter Param in Params)
                {
                    CMD.Parameters.Add(Param);
                }

                if (CN.State == ConnectionState.Closed)
                    CN.Open();

                retVal = CMD.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
            }

            return retVal;
        }

        #endregion

        #region ExecuteDataset

        public static DataSet ExecuteDataSet(string sql, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            DataSet DS = new DataSet();
            SqlDataAdapter DA = new SqlDataAdapter();

            try
            {
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandType = cmdType;
                CMD.CommandTimeout = 0;

                DA.SelectCommand = CMD;
                DA.Fill(DS);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
                DA.Dispose();
                DS.Dispose();
            }

            return DS;
        }

        public static DataSet ExecuteDataSet(string sql, SqlParameter[] Params, CommandType cmdType, ConnectionString cnStr)
        {
            SqlConnection CN = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlCommand CMD = new SqlCommand();
            DataSet DS = new DataSet();
            SqlDataAdapter DA = new SqlDataAdapter();

            try
            {
                CMD.Parameters.Clear();
                CMD.Connection = CN;
                CMD.CommandText = sql;
                CMD.CommandType = cmdType;
                CMD.CommandTimeout = 0;

                DA.SelectCommand = CMD;

                foreach (SqlParameter Param in Params)
                {
                    DA.SelectCommand.Parameters.Add(Param);
                }

                DA.Fill(DS);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (CN.State == ConnectionState.Open)
                    CN.Close();

                CN.Dispose();
                CMD.Dispose();
                DA.Dispose();
                DS.Dispose();
            }

            return DS;
        }

        #endregion

        #region Cache
        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }
        #endregion

        #region Other Method
        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion

        #region Transaction Methods

        static bool tranValue;
        static SqlTransaction Tran;
        static SqlConnection CN = new SqlConnection();
        static SqlCommand CMD = new SqlCommand();
        static ConnectionString cnStr;

        public static ConnectionString ConnectionString
        {
            get { return cnStr; }
            set { cnStr = value; }
        }

        // Transaction Classes
        public static bool TransactionValue
        {
            get { return tranValue; }
            set { tranValue = value; }
        }

        public static void BeginTransaction()
        {
            try
            {
                if (CN.State != ConnectionState.Open)
                {
                    CN.ConnectionString = ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString();
                    CN.Open();
                }
                Tran = CN.BeginTransaction(IsolationLevel.ReadCommitted);
                tranValue = true;
            }
            catch
            {
                throw;
            }
        }

        public static void CommitTransaction()
        {
            try
            {
                Tran.Commit();
                tranValue = false;
            }
            catch
            {
                Tran.Rollback();
                CN.Close();
                throw;
            }
        }

        public static void RollbackTransaction()
        {
            try
            {
                Tran.Rollback();
                if (CN.State != ConnectionState.Closed)
                {
                    CN.Close();
                }
                tranValue = false;
            }
            catch
            {
                throw;
            }
        }

        public static int ExecuteNonQueryTrn(string sql, SqlParameter[] Params, CommandType cmdType)
        {
            if (TransactionValue == true)
            {
                CMD.Transaction = Tran;
            }

            CMD.Parameters.Clear();
            CMD.Connection = CN;
            CMD.CommandType = cmdType;
            CMD.CommandText = sql;

            foreach (SqlParameter Param in Params)
            {
                CMD.Parameters.Add(Param);
            }

            if ((CN.State != ConnectionState.Open) && (TransactionValue == false))
                CN.Open();

            Int32 retVal = Convert.ToInt32(CMD.ExecuteNonQuery());

            if (TransactionValue == false)
                CN.Close();

            return retVal;
        }

        public static int ExecuteNonQueryTrn(string sql, CommandType cmdType)
        {
            if (TransactionValue == true)
            {
                CMD.Transaction = Tran;
            }

            CMD.Parameters.Clear();
            CMD.Connection = CN;
            CMD.CommandType = cmdType;
            CMD.CommandText = sql;

            if ((CN.State != ConnectionState.Open) && (TransactionValue == false))
                CN.Open();

            Int32 retVal = Convert.ToInt32(CMD.ExecuteNonQuery());

            if (TransactionValue == false)
                CN.Close();

            return retVal;
        }

        public static object ExecuteScalarTrn(string sql, SqlParameter[] Params, CommandType cmdType)
        {
            if (TransactionValue == true)
                CMD.Transaction = Tran;

            CMD.Parameters.Clear();
            CMD.Connection = CN;
            CMD.CommandType = cmdType;
            CMD.CommandText = sql;

            foreach (SqlParameter Param in Params)
            {
                CMD.Parameters.Add(Param);
            }

            if ((CN.State != ConnectionState.Open) && (TransactionValue == false))
                CN.Open();

            object retVal = CMD.ExecuteScalar();

            if (TransactionValue == false)
                CN.Close();

            return retVal;
        }

        public static object ExecuteScalarTrn(string sql, CommandType cmdType)
        {
            if (TransactionValue == true)
                CMD.Transaction = Tran;

            CMD.Parameters.Clear();
            CMD.Connection = CN;
            CMD.CommandType = cmdType;
            CMD.CommandText = sql;

            if ((CN.State != ConnectionState.Open) && (TransactionValue == false))
                CN.Open();

            object retVal = CMD.ExecuteScalar();

            if (TransactionValue == false)
                CN.Close();

            return retVal;
        }

        #endregion
    }
}

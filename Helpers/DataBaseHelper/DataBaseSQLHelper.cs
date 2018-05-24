using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.DataBaseHelper
{
    /// <summary>
    /// In this enum you can define your coonection string name that same as webconfig connectionstring
    /// </summary>
    public enum ConnectionString
    {
        CSharpHelpers
    }

    public abstract class DataBaseSQLHelper
    {
        public DataBaseSQLHelper()
        {

        }

        #region Public Method
        /// <summary>
        /// Determine if there is a field in a table
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <param name="cnStr">ConnectionString</param>
        /// <returns>does it exist</returns>
        public static bool ColumnExists(string tableName, string columnName, ConnectionString cnStr)
        {
            string sql = "select count(1) from syscolumns where [id]=object_id('" + tableName + "') and [name]='" + columnName + "'";
            object res = GetSingle(sql, cnStr);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        public static int GetMaxID(string FieldName, string TableName, ConnectionString cnStr)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql, cnStr);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        public static bool Exists(string strSql, ConnectionString cnStr)
        {
            object obj = GetSingle(strSql, cnStr);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool TableExists(string tableName, ConnectionString cnStr)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + tableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            object obj = GetSingle(strsql, cnStr);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Exists(string strSql, ConnectionString cnStr, params SqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cnStr, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Execute simple SQL statements
        /// <summary>
        /// Perform a calculation query result statement and return the query result.
        /// </summary>
        /// <param name="sqlString">Calculate query result statement</param>
        /// <param name="cnStr">ConnectionString</param>
        /// <returns>search result（object）</returns>
        public static object GetSingle(string sqlString, ConnectionString cnStr)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static object GetSingle(string sqlString, ConnectionString cnStr, params SqlParameter[] cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
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
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public static int ExecuteSql(string sqlString, ConnectionString cnStr)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        public static int ExecuteSqlByTime(string sqlString, int times, ConnectionString cnStr)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }


        #endregion

        #region Stored procedure operation
        /// <summary>
        /// Execute the stored procedure, return SqlDataReader(Note: After calling this method, be sure to Close the SqlDataReader)
        /// </summary>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="parameters">Stored procedure parameters</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters, ConnectionString cnStr)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString());
            SqlDataReader returnReader;
            connection.Open();
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);
            return returnReader;

        }

        /// <summary>
        /// Execute the stored procedure
        /// </summary>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="parameters">Stored procedure parameters</param>
        /// <param name="tableName">Table name in DataSet result</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, ConnectionString cnStr)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString()))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int times, ConnectionString cnStr)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[cnStr.ToString()].ConnectionString.ToString()))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.SelectCommand.CommandTimeout = times;
                sqlDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        /// <summary>
        /// Build a SqlCommand object (to return a result set, not an integer value)
        /// </summary>
        /// <param name="connection">Database Connectivity</param>
        /// <param name="storedProcName">Stored procedure name</param>
        /// <param name="parameters">Stored procedure parameters</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // Check the output parameter of the unassigned value and assign it as DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
        #endregion
    }
}

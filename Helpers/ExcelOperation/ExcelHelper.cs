using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Helpers.ExcelOperation
{
    public class ExcelHelper
    {
        #region Export data to Excel file
        /// </summary> 
        /// Export Excel file, automatically return downloadable file stream
        /// </summary> 
        public static void DataTable1Excel(System.Data.DataTable dataTable)
        {
            GridView gvExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;
            if (dataTable != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                curContext.Response.Charset = "utf-8";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                gvExport = new GridView();
                gvExport.DataSource = dataTable.DefaultView;
                gvExport.AllowPaging = false;
                gvExport.DataBind();
                gvExport.RenderControl(htmlWriter);
                curContext.Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=gb2312\"/>" + strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// Export Excel file, convert to readable mode
        /// </summary>
        public static void DataTable2Excel(System.Data.DataTable dataTable)
        {
            DataGrid dgExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dataTable != null)
            {
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                dgExport = new DataGrid();
                dgExport.DataSource = dataTable.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// Export Excel file and customize file name
        /// </summary>
        public static void DataTable3Excel(System.Data.DataTable dataTable, String fileName)
        {
            GridView dgExport = null;
            HttpContext curContext = HttpContext.Current;
            StringWriter strWriter = null;
            HtmlTextWriter htmlWriter = null;

            if (dataTable != null)
            {
                HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
                curContext.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + ".xls");
                curContext.Response.ContentType = "application nd.ms-excel";
                curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
                curContext.Response.Charset = "GB2312";
                strWriter = new StringWriter();
                htmlWriter = new HtmlTextWriter(strWriter);
                dgExport = new GridView();
                dgExport.DataSource = dataTable.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                dgExport.RenderControl(htmlWriter);
                curContext.Response.Write(strWriter.ToString());
                curContext.Response.End();
            }
        }

        /// <summary>
        /// Export data to Excel file
        /// </summary>
        /// <param name="dataTable">DataTable object</param>
        /// <param name="excelFilePath">EXcel file path</param>
        public static bool OutputToExcel(DataTable dataTable, string excelFilePath)
        {
            if (File.Exists(excelFilePath))
            {
                throw new Exception("The file already exists！");
            }

            if ((dataTable.TableName.Trim().Length == 0) || (dataTable.TableName.ToLower() == "table"))
            {
                dataTable.TableName = "Sheet1";
            }

            //The number of columns in the data table
            int ColCount = dataTable.Columns.Count;

            //Used to count, serial numbers when instantiating parameters
            int i = 0;

            //Create parameters
            OleDbParameter[] para = new OleDbParameter[ColCount];

            //Create table structure SQL statement
            string TableStructStr = @"Create Table " + dataTable.TableName + "(";

            //Connection string
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);

            //Create a table structure
            OleDbCommand objCmd = new OleDbCommand();

            //Data type collection
            ArrayList DataTypeList = new ArrayList();
            DataTypeList.Add("System.Decimal");
            DataTypeList.Add("System.Double");
            DataTypeList.Add("System.Int16");
            DataTypeList.Add("System.Int32");
            DataTypeList.Add("System.Int64");
            DataTypeList.Add("System.Single");



            //Traverse all columns of the data table, used to create a table structure
            foreach (DataColumn col in dataTable.Columns)
            {
                // If the column is a numeric column, set the column's data type to double
                if (DataTypeList.IndexOf(col.DataType.ToString()) >= 0)
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.Double);
                    objCmd.Parameters.Add(para[i]);

                    //If it is the last column
                    if (i + 1 == ColCount)
                    {
                        TableStructStr += col.ColumnName + " double)";
                    }
                    else
                    {
                        TableStructStr += col.ColumnName + " double,";
                    }
                }
                else
                {
                    para[i] = new OleDbParameter("@" + col.ColumnName, OleDbType.VarChar);
                    objCmd.Parameters.Add(para[i]);

                    //If it is the last column
                    if (i + 1 == ColCount)
                    {
                        TableStructStr += col.ColumnName + " varchar)";
                    }
                    else
                    {
                        TableStructStr += col.ColumnName + " varchar,";
                    }
                }
                i++;
            }

            //Create Excel file and file structure
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = TableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            // Insert the record's SQL statement
            string InsertSql_1 = "Insert into " + dataTable.TableName + " (";
            string InsertSql_2 = " Values (";
            string InsertSql = "";

            // Iterates over all columns for inserting records, creating SQL statements for inserting records here
            for (int colID = 0; colID < ColCount; colID++)
            {
                if (colID + 1 == ColCount)
                {
                    InsertSql_1 += dataTable.Columns[colID].ColumnName + ")";
                    InsertSql_2 += "@" + dataTable.Columns[colID].ColumnName + ")";
                }
                else
                {
                    InsertSql_1 += dataTable.Columns[colID].ColumnName + ",";
                    InsertSql_2 += "@" + dataTable.Columns[colID].ColumnName + ",";
                }
            }

            InsertSql = InsertSql_1 + InsertSql_2;

            // Iterate over all data rows of the data table
            for (int rowID = 0; rowID < dataTable.Rows.Count; rowID++)
            {
                for (int colID = 0; colID < ColCount; colID++)
                {
                    if (para[colID].DbType == DbType.Double && dataTable.Rows[rowID][colID].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = dataTable.Rows[rowID][colID].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = InsertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }

        /// <summary>
        /// Export data to Excel file
        /// </summary>
        /// <param name="dataTable">DataTable object</param>
        /// <param name="columns">The data column collection to export</param>
        /// <param name="excelFilePath">Excel file path</param>
        public static bool OutputToExcel(DataTable dataTable, ArrayList columns, string excelFilePath)
        {
            if (File.Exists(excelFilePath))
            {
                throw new Exception("The file already exists！");
            }

            //If the number of data columns is greater than the number of columns in the table, take all columns of the data table
            if (columns.Count > dataTable.Columns.Count)
            {
                for (int s = dataTable.Columns.Count + 1; s <= columns.Count; s++)
                {
                    columns.RemoveAt(s);   //移除数据表列数后的所有列
                }
            }


            // Iterate over all data columns and remove the data column if its data type is not DataColumn
            DataColumn column = new DataColumn();
            for (int j = 0; j < columns.Count; j++)
            {
                try
                {
                    column = (DataColumn)columns[j];
                }
                catch (Exception)
                {
                    columns.RemoveAt(j);
                }
            }
            if ((dataTable.TableName.Trim().Length == 0) || (dataTable.TableName.ToLower() == "table"))
            {
                dataTable.TableName = "Sheet1";
            }

            //The number of columns in the data table
            int ColCount = columns.Count;

            //Create parameters
            OleDbParameter[] para = new OleDbParameter[ColCount];

            //Create a table structure of the SQL statement
            string TableStructStr = @"Create Table " + dataTable.TableName + "(";

            //Connection string
            string connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFilePath + ";Extended Properties=Excel 8.0;";
            OleDbConnection objConn = new OleDbConnection(connString);

            //Create a table structure
            OleDbCommand objCmd = new OleDbCommand();

            // Collection of data types
            ArrayList DataTypeList = new ArrayList();
            DataTypeList.Add("System.Decimal");
            DataTypeList.Add("System.Double");
            DataTypeList.Add("System.Int16");
            DataTypeList.Add("System.Int32");
            DataTypeList.Add("System.Int64");
            DataTypeList.Add("System.Single");

            DataColumn col = new DataColumn();

            //Traverse all columns of the data table, used to create a table structure
            for (int k = 0; k < ColCount; k++)
            {
                col = (DataColumn)columns[k];

                //The data type of the column is digital
                if (DataTypeList.IndexOf(col.DataType.ToString().Trim()) >= 0)
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.Double);
                    objCmd.Parameters.Add(para[k]);

                    //If it is the last column
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " Double)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " Double,";
                    }
                }
                else
                {
                    para[k] = new OleDbParameter("@" + col.Caption.Trim(), OleDbType.VarChar);
                    objCmd.Parameters.Add(para[k]);

                    //If it is the last column
                    if (k + 1 == ColCount)
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar)";
                    }
                    else
                    {
                        TableStructStr += col.Caption.Trim() + " VarChar,";
                    }
                }
            }

            //Create Excel file and file structure
            try
            {
                objCmd.Connection = objConn;
                objCmd.CommandText = TableStructStr;

                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }
                objCmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //Insert record SQL statement
            string InsertSql_1 = "Insert into " + dataTable.TableName + " (";
            string InsertSql_2 = " Values (";
            string InsertSql = "";

            //Traverses all columns for inserting records, creating SQL statements for inserting records here
            for (int colID = 0; colID < ColCount; colID++)
            {
                if (colID + 1 == ColCount)
                {
                    InsertSql_1 += columns[colID].ToString().Trim() + ")";
                    InsertSql_2 += "@" + columns[colID].ToString().Trim() + ")";
                }
                else
                {
                    InsertSql_1 += columns[colID].ToString().Trim() + ",";
                    InsertSql_2 += "@" + columns[colID].ToString().Trim() + ",";
                }
            }

            InsertSql = InsertSql_1 + InsertSql_2;

            //Traverse all data rows of the data table
            DataColumn DataCol = new DataColumn();
            for (int rowID = 0; rowID < dataTable.Rows.Count; rowID++)
            {
                for (int colID = 0; colID < ColCount; colID++)
                {
                    //Because the columns are not continuous, you cannot use row and column numbers to get cells. Columns need column names.
                    DataCol = (DataColumn)columns[colID];
                    if (para[colID].DbType == DbType.Double && dataTable.Rows[rowID][DataCol.Caption].ToString().Trim() == "")
                    {
                        para[colID].Value = 0;
                    }
                    else
                    {
                        para[colID].Value = dataTable.Rows[rowID][DataCol.Caption].ToString().Trim();
                    }
                }
                try
                {
                    objCmd.CommandText = InsertSql;
                    objCmd.ExecuteNonQuery();
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                }
            }
            try
            {
                if (objConn.State == ConnectionState.Open)
                {
                    objConn.Close();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Get Excel file data table list
        /// </summary>
        public static ArrayList GetExcelTables(string excelFileName)
        {
            DataTable dt = new DataTable();
            ArrayList TablesList = new ArrayList();
            if (File.Exists(excelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + excelFileName))
                {
                    try
                    {
                        conn.Open();
                        dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }

                    //Get the number of data tables
                    int tablecount = dt.Rows.Count;
                    for (int i = 0; i < tablecount; i++)
                    {
                        string tablename = dt.Rows[i][2].ToString().Trim().TrimEnd('$');
                        if (TablesList.IndexOf(tablename) < 0)
                        {
                            TablesList.Add(tablename);
                        }
                    }
                }
            }
            return TablesList;
        }

        /// <summary>
        /// Export Excel file to DataTable (first row as header)
        /// </summary>
        /// <param name="excelFilePath">Excel file path</param>
        /// <param name="tableName">Data table name, if the data table name is wrong, the default is the first data table name</param>
        public static DataTable InputFromExcel(string excelFilePath, string tableName)
        {
            if (!File.Exists(excelFilePath))
            {
                throw new Exception("Excel file does not exist！");
            }

            //If the data table name does not exist, the data table name is the first data sheet of the Excel file
            ArrayList TableList = new ArrayList();
            TableList = GetExcelTables(excelFilePath);

            if (tableName.IndexOf(tableName) < 0)
            {
                tableName = TableList[0].ToString().Trim();
            }

            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFilePath + ";Extended Properties=Excel 8.0");
            OleDbCommand cmd = new OleDbCommand("select * from [" + tableName + "$]", dbcon);
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

            try
            {
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                adapter.Fill(table);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
            }
            return table;
        }

        /// <summary>
        /// Get the data list of the specified data table of the Excel file
        /// </summary>
        /// <param name="excelFileName">Excel file name</param>
        /// <param name="tableName">Data table name</param>
        public static ArrayList GetExcelTableColumns(string excelFileName, string tableName)
        {
            DataTable dt = new DataTable();
            ArrayList ColsList = new ArrayList();
            if (File.Exists(excelFileName))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + excelFileName))
                {
                    conn.Open();
                    dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tableName, null });

                    //Get the number of columns
                    int colcount = dt.Rows.Count;
                    for (int i = 0; i < colcount; i++)
                    {
                        string colname = dt.Rows[i]["Column_Name"].ToString().Trim();
                        ColsList.Add(colname);
                    }
                }
            }
            return ColsList;
        }
    }
}

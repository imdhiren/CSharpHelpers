using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.CSV
{
    public static class CsvHelper
    {
        /// <summary>
        /// Convert datatable to csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="strFilePath">File Path</param>
        /// <param name="tableheader">Table Header</param>
        /// <param name="columname">Column Name</param>
        public static bool DataTableToCSV(DataTable dt, string strFilePath, string tableheader, string columname)
        {
            try
            {
                string strBufferLine = "";
                StreamWriter strmWriterObj = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8);
                strmWriterObj.WriteLine(tableheader);
                strmWriterObj.WriteLine(columname);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strBufferLine = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dt.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// CSV to DataTable
        /// </summary>
        /// <param name="filePath">CSV Path</param>
        /// <param name="n">Indicates that the nth line is the field title, the n+1th line is the beginning of the record</param>
        public static DataTable CSVToDataTable(string filePath, int n, DataTable dt)
        {
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.UTF8, false);
            int i = 0, m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    string[] split = str.Split(',');

                    System.Data.DataRow dr = dt.NewRow();
                    for (i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}

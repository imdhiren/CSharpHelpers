using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.ListToDataTable
{
    public static class ListToDataTableHelper
    {
        /// <summary>
        /// Convert the specified collection to a DataTable.
        /// </summary>
        /// <param name="list">Will specify the collection。</param>
        /// <returns>Return the converted DataTable。</returns>
        public static DataTable ToDataTable(this IList list)
        {
            DataTable table = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    Type pt = pi.PropertyType;
                    if ((pt.IsGenericType) && (pt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        pt = pt.GetGenericArguments()[0];
                    }
                    table.Columns.Add(new DataColumn(pi.Name, pt));
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }
            return table;
        }

        /// <summary>
        /// Convert the generic collection specified to a DataTable.
        /// </summary>
        /// <typeparam name="T">Genraric T</typeparam>
        /// <param name="list">Will specify the collection</param>
        /// <returns>Return the converted DataTable。</returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            DataTable table = new DataTable();
            //Create column head
            PropertyInfo[] propertys = typeof(T).GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                Type pt = pi.PropertyType;
                if ((pt.IsGenericType) && (pt.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    pt = pt.GetGenericArguments()[0];
                }
                table.Columns.Add(new DataColumn(pi.Name, pt));
            }
            //Create data line
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }
            return table;
        }
    }
}


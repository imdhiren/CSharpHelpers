using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.DataTableToEntity
{
    public static class DataTableToEntityHelper
    {
        /// <summary>
        /// DataTable to Entity
        /// </summary>
        /// <typeparam name="T">>Generic Type</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataTable table) where T : new()
        {
            T entity = new T();
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type newType = item.PropertyType;

                            //(newType.IsGenericType) Determine if the type is generic because nullable is a generic class.
                            //(newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) Judge whether convertsionType is nullable generic class
                            if (newType.IsGenericType && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) 
                            {
                                // If the type is a nullable class, declare a NullableConverter class that provides the conversion from the Nullable class to the underlying primitive type
                                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);

                                // The primitive primitive type that converts type to a nullable pair
                                newType = nullableConverter.UnderlyingType;
                            }

                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);

                        }

                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// DataTable To Multiple Entity
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns></returns>
        public static List<T> ToEntities<T>(this DataTable table) where T : new()
        {
            List<T> entities = new List<T>();
            if (table == null)
                return null;
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            Type newType = item.PropertyType;

                            //(newType.IsGenericType) Determine if the type is generic because nullable is a generic class.
                            //(newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) Judge whether convertsionType is nullable generic class
                            if (newType.IsGenericType && newType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                            {

                                // If the type is a nullable class, declare a NullableConverter class that provides the conversion from the Nullable class to the underlying primitive type
                                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(newType);

                                // The primitive primitive type that converts type to a nullable pair
                                newType = nullableConverter.UnderlyingType;
                            }
                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                        }
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }
    }
}

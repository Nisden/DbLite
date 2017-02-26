namespace DbLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public static class DbLiteDataReaderExtensions
    {
        #region Read - Read single elements

        public static TModel Read<TModel>(this IDataReader reader, Dictionary<int, PropertyInfo> propertyCache)
            where TModel : class, new()
        {
            return (TModel)Read(reader, typeof(TModel), DbLiteModelInfo<TModel>.Instance, propertyCache);
        }

        public static object Read(this IDataReader reader, Type type, Dictionary<int, PropertyInfo> propertyCache)
        {
            return Read(reader, type, new DbLiteModelInfo(type), propertyCache);
        }

        private static object Read(IDataReader reader, Type type, DbLiteModelInfo modelInfo, Dictionary<int, PropertyInfo> propertyCache)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (propertyCache == null)
                throw new ArgumentNullException(nameof(propertyCache), $"The {nameof(propertyCache)} must be assigned, even if empty");

            if (propertyCache.Count == 0)
            {
                for (int c = 0; c < reader.FieldCount; c++)
                {
                    // Loop each column, and find a property with a matching name
                    DbLiteModelInfoColumn columnInfo;
                    if (modelInfo.Columns.TryGetValue(reader.GetName(c), out columnInfo))
                    {
                        propertyCache.Add(c, columnInfo.Property);
                    }
                }
            }

            // Create element 
            object obj = Activator.CreateInstance(type);

            // Read all the columns
            foreach (var columnInfo in propertyCache)
            {
                object value = reader.GetValue(columnInfo.Key);
                if (value is DBNull)
                    value = null;

                if (value == null && columnInfo.Value.PropertyType.GetTypeInfo().IsValueType)
                    throw new InvalidCastException($"Unable to set {columnInfo.Value.PropertyType.Name} to null");

                columnInfo.Value.SetValue(obj, Convert.ChangeType(value, columnInfo.Value.PropertyType));
            }

            return obj;
        }

        #endregion

        #region ToList - Read multiple elements

        public static List<TModel> ToList<TModel>(this IDataReader reader) 
            where TModel : class, new()
        {
            List<TModel> results = new List<TModel>();

            Dictionary<int, PropertyInfo> propertyCache = new Dictionary<int, PropertyInfo>();
            while (reader.Read())
            {
                results.Add(reader.Read<TModel>(propertyCache));
            }

            return results;
        }

        public static List<object> ToList(this IDataReader reader, Type type)
        {
            List<object> results = new List<object>();

            Dictionary<int, PropertyInfo> propertyCache = new Dictionary<int, PropertyInfo>();
            while (reader.Read())
            {
                results.Add(reader.Read(type, propertyCache));
            }

            return results;
        }

        #endregion
    }
}

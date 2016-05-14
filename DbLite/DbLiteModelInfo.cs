namespace DbLite
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    public sealed class DbLiteModelInfo<TModel> : DbLiteModelInfo
    {
        private static Lazy<DbLiteModelInfo<TModel>> instance = new Lazy<DbLiteModelInfo<TModel>>();
        public static DbLiteModelInfo<TModel> Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public DbLiteModelInfo() : base(typeof(TModel))
        {

        }
    }

    public class DbLiteModelInfo
    {
        public string Name { get; private set; }

        public bool HasAutoIncrement { get; private set; }

        public IDictionary<string, DbLiteModelInfoColumn> Columns { get; private set; }

        /// <summary>
        /// Gets the information about a type in relation to DbLite.
        /// If you can use the generic version <see cref="DbLiteModelInfo{TModel}"/> as its cached and provides performance benefits
        /// </summary>
        /// <param name="type"></param>
        public DbLiteModelInfo(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Resolve name for type
            Name = type.GetCustomAttribute<AliasAttribute>()?.Name ?? type.Name;

            // Loop the properties and extract column infomation
            var columns = new Dictionary<string, DbLiteModelInfoColumn>();
            foreach (var property in type.GetProperties())
            {
                if (property.GetCustomAttribute<IgnoreAttribute>() != null)
                    continue;

                var columnInfo = new DbLiteModelInfoColumn(property);
                columns.Add(property.Name, columnInfo);
            }

            // Change dictionary to read only
            Columns = new ReadOnlyDictionary<string, DbLiteModelInfoColumn>(columns);

            HasAutoIncrement = Columns.Any(x => x.Value.AutoIncrementing);
        }
    }

    public sealed class DbLiteModelInfoColumn
    {
        public string Name { get; private set; }

        public PropertyInfo Property { get; private set; }

        public bool Key { get; private set; }

        public bool AutoIncrementing { get; private set; }

        public DbLiteModelInfoColumn(PropertyInfo property)
        {
            Name = property.GetCustomAttribute<AliasAttribute>()?.Name ?? property.Name;
            Key = property.GetCustomAttribute<KeyAttribute>() != null;
            AutoIncrementing = property.GetCustomAttribute<AutoIncrementAttribute>() != null;
            Property = property;
        }

        public object GetValue(object item)
        {
            if (item == null)
                throw new NotImplementedException(nameof(item));

            return Property.GetMethod.Invoke(item, null);
        }
    }
}

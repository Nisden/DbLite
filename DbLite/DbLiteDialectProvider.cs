namespace DbLite
{
    using Query;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class DbLiteDialectProvider
    {
        public abstract bool SupportsMultipleInsertValues
        {
            get;
        }

        public DbLiteDialectProvider()
        {

        }

        public abstract string EscapeTable(string tableName);

        public abstract string EscapeColumn(string columnName);

        public abstract DbLiteQueryProvider CreateQueryProvider(IDbConnection connection);

        public virtual void SetParameterDataType(IDbDataParameter parameter, object value)
        {
            if (value == null)
            {
                parameter.DbType = DbType.Object;
            }
            if (value is string)
            {
                parameter.DbType = DbType.String;
            }
            else if (value is Boolean)
            {
                parameter.DbType = DbType.Boolean;
            }
            else if (value is DateTime)
            {
                parameter.DbType = DbType.DateTime;
            }
            else if (value is Int16)
            {
                parameter.DbType = DbType.Int16;
            }
            else if (value is Int32)
            {
                parameter.DbType = DbType.Int32;
            }
            else if (value is Int64)
            {
                parameter.DbType = DbType.Int64;
            }
            else if (value is Decimal)
            {
                parameter.DbType = DbType.Decimal;
            }
            else
            {
                throw new NotImplementedException($"DataType for {value.GetType().FullName} is not set");
            }
        }
    }
}

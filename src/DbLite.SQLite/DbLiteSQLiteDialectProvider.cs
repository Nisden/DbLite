namespace DbLite.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DbLiteSQLiteDialectProvider : DbLiteDialectProvider
    {
        public override bool SupportsMultipleInsertValues
        {
            get
            {
                return true;
            }
        }

        public override string RetriveLastIdentity
        {
            get
            {
                return ";SELECT last_insert_rowid()";
            }
        }

        public override string EscapeColumn(string columnName)
        {
            return Escape(columnName);
        }

        public override string EscapeTable(string tableName)
        {
            return Escape(tableName);
        }

        protected string Escape(string value)
        {
            // SQLLite is escaped with ", and " is escaped with ""
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}

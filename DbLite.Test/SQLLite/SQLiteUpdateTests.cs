namespace DbLite.Test.SQLLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SQLiteUpdateTests : UpdateTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteUpdateTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

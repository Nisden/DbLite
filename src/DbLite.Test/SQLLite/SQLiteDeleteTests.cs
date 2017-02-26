namespace DbLite.Test.SQLLite
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.Sqlite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Xunit.Collection("SQLite")]
    public class SQLiteDeleteTests : DeleteTests<SQLiteDatabaseFixture, SqliteConnection>
    {
        public SQLiteDeleteTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

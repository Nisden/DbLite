namespace DbLite.Test.SQLLite
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Data.Sqlite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    [Xunit.Collection("SQLite")]
    public class SQLiteSelectTests : SelectTests<SQLiteDatabaseFixture, SqliteConnection>
    {
        public SQLiteSelectTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

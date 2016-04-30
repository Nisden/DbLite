namespace DbLite.Test.SQLLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    [Xunit.Collection("SQLite")]
    public class SQLiteSelectTests : SelectTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteSelectTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

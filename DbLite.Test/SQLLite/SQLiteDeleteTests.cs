namespace DbLite.Test.SQLLite
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Xunit.Collection("SQLite")]
    public class SQLiteDeleteTests : DeleteTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteDeleteTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

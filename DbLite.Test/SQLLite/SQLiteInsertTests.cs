namespace DbLite.Test.SQLLite
{
    using System.Data.SQLite;

    [Xunit.Collection("SQLite")]
    public class SQLiteInsertTests : InsertTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteInsertTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

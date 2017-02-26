namespace DbLite.Test.SQLLite
{
    using Microsoft.Data.Sqlite;

    [Xunit.Collection("SQLite")]
    public class SQLiteInsertTests : InsertTests<SQLiteDatabaseFixture, SqliteConnection>
    {
        public SQLiteInsertTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

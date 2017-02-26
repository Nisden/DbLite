namespace DbLite.Test.SQLLite
{
    using Microsoft.Data.Sqlite;

    [Xunit.Collection("SQLite")]
    public class SQLiteReadmeTests : ReadmeTests<SQLiteDatabaseFixture, SqliteConnection>
    {
        public SQLiteReadmeTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

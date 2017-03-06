namespace DbLite.Test.SQLLite
{
    using Microsoft.Data.Sqlite;

    [Xunit.Collection("SQLite")]
    public class SQLiteUpdateTests : UpdateTests<SQLiteDatabaseFixture, SqliteConnection>
    {
        public SQLiteUpdateTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

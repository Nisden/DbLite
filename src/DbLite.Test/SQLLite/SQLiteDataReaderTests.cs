namespace DbLite.Test.SQLLite
{
    using Microsoft.Data.Sqlite;

    [Xunit.Collection("SQLite")]
    public class SQLiteDataReaderTests : DataReaderTests<SQLiteDatabaseFixture, SqliteConnection>
    {
        public SQLiteDataReaderTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

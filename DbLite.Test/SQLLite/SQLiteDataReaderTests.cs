namespace DbLite.Test.SQLLite
{
    using System.Data.SQLite;

    [Xunit.Collection("SQLite")]
    public class SQLiteDataReaderTests : DataReaderTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteDataReaderTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

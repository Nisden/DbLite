namespace DbLite.Test.SQLLite
{
    using System.Data.SQLite;

    public class SQLiteDataReaderTests : DataReaderTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteDataReaderTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

namespace DbLite.Test.SQLLite
{
    using System.Data.SQLite;

    [Xunit.Collection("SQLite")]
    public class SQLiteQueryTests : QueryTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteQueryTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

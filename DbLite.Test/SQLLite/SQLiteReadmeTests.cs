namespace DbLite.Test.SQLLite
{
    using System.Data.SQLite;

    public class SQLiteReadmeTests : ReadmeTests<SQLiteDatabaseFixture, SQLiteConnection>
    {
        public SQLiteReadmeTests(SQLiteDatabaseFixture fixture) : base(fixture)
        { }
    }
}

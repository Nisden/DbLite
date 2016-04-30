namespace DbLite.Test.MSSQL
{
    using System.Data.SqlClient;
    using System.Data.SQLite;

    [Xunit.Collection("MSSQL")]
    public class MSSQLReadmeTests : ReadmeTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLReadmeTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

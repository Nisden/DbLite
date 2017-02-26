namespace DbLite.Test.MSSQL
{
    using System.Data.SqlClient;
    using Microsoft.Data.Sqlite;

    [Xunit.Collection("MSSQL")]
    public class MSSQLReadmeTests : ReadmeTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLReadmeTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

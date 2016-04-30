namespace DbLite.Test.MSSQL
{
    using System.Data.SqlClient;
    using System.Data.SQLite;

    [Xunit.Collection("MSSQL")]
    public class MSSQLQueryTests : QueryTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLQueryTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

namespace DbLite.Test.MSSQL
{
    using System.Data.SqlClient;
    using System.Data.SQLite;

    [Xunit.Collection("MSSQL")]
    public class MSSQLDataReaderTests : DataReaderTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLDataReaderTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

namespace DbLite.Test.MSSQL
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Xunit.Collection("MSSQL")]
    public class MSSQLDeleteTests : DeleteTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLDeleteTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

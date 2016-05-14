namespace DbLite.Test.MSSQL
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Xunit.Collection("MSSQL")]
    public class MSSQLInsertTests : InsertTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLInsertTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

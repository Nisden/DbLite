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
    public class MSSQLUpdateTests : UpdateTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLUpdateTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

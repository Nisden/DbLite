namespace DbLite.Test.MSSQL
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Microsoft.Data.Sqlite;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    [Xunit.Collection("MSSQL")]
    public class MSSQLSelectTests : SelectTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLSelectTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

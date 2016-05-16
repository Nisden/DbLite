namespace DbLite.Test.MSSQL.Replication
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Test.Replication;

    [Xunit.Collection("MSSQL")]
    public class MSSQLTableReplicationTests : TableReplicationTests<MSSQLDatabaseFixture, SqlConnection>
    {
        public MSSQLTableReplicationTests(MSSQLDatabaseFixture fixture) : base(fixture)
        { }
    }
}

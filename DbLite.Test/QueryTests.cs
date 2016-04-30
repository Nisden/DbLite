namespace DbLite.Test
{
    using System.Data;
    using System.Linq;
    using Tables;
    using Xunit;

    public abstract class QueryTests<TDatabaseFixture, TConnection>
        where TDatabaseFixture : DatabaseFixture<TConnection>
        where TConnection : IDbConnection
    {
        private readonly TDatabaseFixture fixture;
        public TDatabaseFixture Fixture
        {
            get
            {
                return fixture;
            }
        }

        protected QueryTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact(Skip = "Expression build not implemented yet")]
        public void QuerySimpleTableAll()
        {
            using (var connection = Fixture.Open())
            {
                Assert.NotEmpty(connection.Query<SimpleTable>().Where(x => x.String1 == "Test1"));
            }
        }
    }
}

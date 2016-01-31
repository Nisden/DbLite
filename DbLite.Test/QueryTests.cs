namespace DbLite.Test
{
    using System.Data;
    using System.Linq;
    using Tables;
    using Xunit;

    [Collection("Database Collection")]
    public abstract class QueryTests<TDatabaseFixture, TConnection> : IClassFixture<TDatabaseFixture>
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
            Assert.NotEmpty(fixture.Db.Query<SimpleTable>().Where(x => x.String1 == "Test1"));
        }
    }
}

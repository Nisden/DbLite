namespace DbLite.Test
{
    using Tables;
    using System.Data;
    using Xunit;

    public abstract class DeleteTests<TDatabaseFixture, TConnection>
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

        protected DeleteTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void DeleteRecord()
        {
            using (new System.Transactions.TransactionScope())
            {
                using (var connection = Fixture.Open())
                {
                    bool eventInvoked = false;
                    DbLiteConfiguration.BeforeDelete += (ss, ee) => eventInvoked = true;

                    var record = connection.Single<SimpleTable>("Interger1 = @Id", new { Id = 20 });
                    connection.Delete(record);

                    Assert.Null(connection.Single<SimpleTable>("Interger1 = @Id", new { Id = 20 }));
                    Assert.True(eventInvoked, "BeforeDelete was not invoked");
                }
            }
        }
    }
}

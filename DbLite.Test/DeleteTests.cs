namespace DbLite.Test
{
    using Tables;
    using System.Data;
    using Xunit;

    [Collection("Database Collection")]
    public abstract class DeleteTests<TDatabaseFixture, TConnection> : IClassFixture<TDatabaseFixture>
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
            using (Fixture.Db.BeginTransaction())
            {
                var record = Fixture.Db.Single<SimpleTable>("Interger1 = @Id", new { Id = 20 });
                Fixture.Db.Delete(record);

                Assert.Null(Fixture.Db.Single<SimpleTable>("Interger1 = @Id", new { Id = 20 }));
            }
        }
    }
}

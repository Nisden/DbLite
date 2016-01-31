namespace DbLite.Test
{
    using System.Data;
    using Tables;
    using Xunit;

    [Collection("Database Collection")]
    public abstract class SelectTests<TDatabaseFixture, TConnection> : IClassFixture<TDatabaseFixture>
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

        protected SelectTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void SelectAll()
        {
            var all = Fixture.Db.Select<SimpleTable>(null, null);
            Assert.Equal(100, all.Count);
        }

        [Fact]
        public void SelectWhereNoPrefix()
        {
            var some = Fixture.Db.Select<SimpleTable>("Interger1 <= 50", null);
            Assert.NotEmpty(some);
        }

        [Fact]
        public void SelectWhereNoPrefixWithParamater()
        {
            var some = Fixture.Db.Select<SimpleTable>("Interger1 <= @LessThan", new { LessThan = 50 });
            Assert.NotEmpty(some);
        }

        [Fact]
        public void SelectWhereWithPrefix()
        {
            var some = Fixture.Db.Select<SimpleTable>("where Interger1 <= 50", null);
            Assert.NotEmpty(some);
        }

        [Fact]
        public void SelectWithFrom()
        {
            var some = Fixture.Db.Select<SimpleTable>("from SimpleTable where Interger1 <= 50", null);
            Assert.NotEmpty(some);
        }

        [Fact]
        public void SelectFull()
        {
            var some = Fixture.Db.Select<SimpleTable>("SELECT * FROM SimpleTable", null);
            Assert.NotEmpty(some);
        }
    }
}

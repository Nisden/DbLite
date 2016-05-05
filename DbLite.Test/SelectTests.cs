namespace DbLite.Test
{
    using System.Data;
    using Tables;
    using Xunit;

    public abstract class SelectTests<TDatabaseFixture, TConnection>
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
            using (var connection = Fixture.Open())
            {
                var all = connection.Select<SimpleTable>(null, null);
                Assert.Equal(100, all.Count);
            }
        }

        [Fact]
        public void SelectWhereNoPrefix()
        {
            using (var connection = Fixture.Open())
            {
                var some = connection.Select<SimpleTable>("Interger1 <= 50", null);
                Assert.NotEmpty(some);
            }
        }

        [Fact]
        public void SelectWhereNoPrefixWithParamater()
        {
            using (var connection = Fixture.Open())
            {
                var some = connection.Select<SimpleTable>("Interger1 <= @LessThan", new { LessThan = 50 });
                Assert.NotEmpty(some);
            }
        }

        [Fact]
        public void SelectWhereWithPrefix()
        {
            using (var connection = Fixture.Open())
            {
                var some = connection.Select<SimpleTable>("where Interger1 <= 50");
                Assert.NotEmpty(some);
            }
        }

        [Fact]
        public void SelectWithFrom()
        {
            using (var connection = Fixture.Open())
            {
                var some = connection.Select<SimpleTable>("from SimpleTable where Interger1 <= 50");
                Assert.NotEmpty(some);
            }
        }

        [Fact]
        public void SelectFull()
        {
            using (var connection = Fixture.Open())
            {
                var some = connection.Select<SimpleTable>("SELECT * FROM SimpleTable");
                Assert.NotEmpty(some);
            }
        }

        [Fact]
        public void SelectOrderBy()
        {
            using (var connection = Fixture.Open())
            {
                connection.Select<SimpleTable>("ORDER BY [Interger1]");
            }
        }
    }
}

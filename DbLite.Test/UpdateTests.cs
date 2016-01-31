namespace DbLite.Test
{
    using System.Data;
    using System.Linq;
    using Tables;
    using Xunit;

    [Collection("Database Collection")]
    public abstract class UpdateTests<TDatabaseFixture, TConnection> : IClassFixture<TDatabaseFixture>
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

        protected UpdateTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
    
        [Fact]
        public void DoUpdate()
        {
            using (this.Fixture.Db.BeginTransaction())
            {
                var record = this.Fixture.Db.Single<SimpleTable>("Interger1 = @Id", new { Id = 5 });
                Assert.NotNull(record);

                // Change record
                record.String1 = "Nick";
                record.String2 = "Ninja";

                this.Fixture.Db.Update(record);

                // Check the record has changed
                Assert.NotEmpty(this.Fixture.Db.Select<SimpleTable>("String1 = @Str1 AND String2 = @Str2", new { Str1 = "Nick", Str2 = "Ninja" }));
            }
        }

        [Fact]
        public void UpdateMultiKey()
        {
            using (this.Fixture.Db.BeginTransaction())
            {
                this.Fixture.Db.Update(new MultiKeyTable()
                {
                    Id1 = 1,
                    Id2 = "Hello",
                    Value = "My name is now ninja!"
                });
            }
        }
    }
}

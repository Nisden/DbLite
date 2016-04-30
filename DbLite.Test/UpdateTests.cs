namespace DbLite.Test
{
    using System.Data;
    using System.Linq;
    using Tables;
    using Xunit;

    public abstract class UpdateTests<TDatabaseFixture, TConnection>
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
            using (new System.Transactions.TransactionScope())
            {
                using (var connection = Fixture.Open())
                {
                    var record = connection.Single<SimpleTable>("Interger1 = @Id", new { Id = 5 });
                    Assert.NotNull(record);

                    // Change record
                    record.String1 = "Nick";
                    record.String2 = "Ninja";

                    connection.Update(record);

                    // Check the record has changed
                    Assert.NotEmpty(connection.Select<SimpleTable>("String1 = @Str1 AND String2 = @Str2", new { Str1 = "Nick", Str2 = "Ninja" }));
                }
            }
        }

        [Fact]
        public void UpdateMultiKey()
        {
            using (new System.Transactions.TransactionScope())
            {
                using (var connection = Fixture.Open())
                {
                    connection.Update(new MultiKeyTable()
                    {
                        Id1 = 1,
                        Id2 = "Hello",
                        Value = "My name is now ninja!"
                    });
                }
            }
        }
    }
}

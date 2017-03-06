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
            using (var connection = Fixture.Open())
            {
                using (connection.OpenTransaction())
                {
                    var record = connection.Single<SimpleTable>("Interger1 = @Id", new { Id = 5 });
                    Assert.NotNull(record);

                    // Change record
                    record.String1 = "Nick";
                    record.String2 = "Ninja";

                    bool eventInvoked = false;
                    DbLiteConfiguration.BeforeUpdate += (ss, ee) =>
                    {
                        eventInvoked = true;
                    };

                    connection.Update(record);

                    // Check that the before update event was triggered
                    Assert.True(eventInvoked, "BeforeUpdate was not hit");

                    // Check the record has changed
                    Assert.NotEmpty(connection.Select<SimpleTable>("String1 = @Str1 AND String2 = @Str2", new { Str1 = "Nick", Str2 = "Ninja" }));
                }
            }
        }

        [Fact]
        public void UpdateMultiKey()
        {
            using (var connection = Fixture.Open())
            {
                using (connection.OpenTransaction())
                {
                    bool eventInvoked = false;
                    DbLiteConfiguration.BeforeUpdate += (ss, ee) =>
                    {
                        eventInvoked = true;
                    };

                    connection.Update(new MultiKeyTable()
                    {
                        Id1 = 1,
                        Id2 = "Hello",
                        Value = "My name is now ninja!"
                    });

                    // Check that the before update event was triggered
                    Assert.True(eventInvoked, "BeforeUpdate was not hit");
                }
            }
        }

        [Fact]
        public void UpdateAutoIncrement()
        {
            using (var connection = Fixture.Open())
            {
                using (connection.OpenTransaction())
                {
                    var autoIncrement = new AutoIncrementingTable()
                    {
                        Value = "LALALA"
                    };
                    autoIncrement.Id = connection.Insert(autoIncrement);

                    var records = connection.Select<AutoIncrementingTable>("");

                    connection.Update(autoIncrement);
                }
            }
        }
    }
}

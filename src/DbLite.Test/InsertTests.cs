namespace DbLite.Test
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tables;
    using Xunit;

    public abstract class InsertTests<TDatabaseFixture, TConnection>
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

        protected InsertTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void SimpleInsert()
        {
            using (var connection = Fixture.Open())
            {
                using (connection.OpenTransaction())
                {
                    connection.Insert(new SimpleTable()
                    {
                        String1 = "Is pure magic!"
                    });

                    Assert.NotEmpty(connection.Select<SimpleTable>("WHERE [String1] = @String1", new { String1 = "Is pure magic!" }));
                }
            }
        }

        [Fact]
        public void MultiInsert()
        {
            using (var connection = Fixture.Open())
            {
                using (connection.OpenTransaction())
                {
                    connection.Insert(
                        new SimpleTable()
                        {
                            String1 = "Is pure magic!"
                        },
                        new SimpleTable()
                        {
                            String1 = "Is pure magic2!"
                        });

                    Assert.NotEmpty(connection.Select<SimpleTable>("WHERE [String1] = @String1", new { String1 = "Is pure magic!" }));
                    Assert.NotEmpty(connection.Select<SimpleTable>("WHERE [String1] = @String1", new { String1 = "Is pure magic2!" }));
                }
            }
        }

        [Fact]
        public void InsertWithIdentity()
        {
            using (var connection = Fixture.Open())
            {
                using (connection.OpenTransaction())
                {
                    connection.Insert(new AutoIncrementingTable()
                    {
                        Value = "Stuff"
                    });

                    Assert.NotEmpty(connection.Select<AutoIncrementingTable>(string.Empty));
                }
            }

        }
    }
}

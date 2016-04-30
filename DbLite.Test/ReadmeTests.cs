namespace DbLite.Test
{
    using Tables;
    using System.Data;
    using Xunit;
    using System.Collections.Generic;

    public abstract class ReadmeTests<TDatabaseFixture, TConnection>
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

        protected ReadmeTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Test()
        {
            // Start a transaction
            using (new System.Transactions.TransactionScope())
            {
                // Get a instance of IDbConnection, this can be SQLiteConnection, SQLConnection, etc
                using (var db = Fixture.Open())
                {
                    var record = new SimpleTable
                    {
                        Interger1 = 22,
                        String1 = "Hello! This is an insert"
                    };
                    db.Insert(record);

                    record.String1 = "We just changed the text using an update!";
                    db.Update(record);

                    // Select all records where Interger1 is 22
                    List<SimpleTable> records = db.Select<SimpleTable>("Interger1 = @Id", new { Id = 22 });

                    // And now we just deleted all the records
                    records.ForEach(x => db.Delete(x));
                }
            }
        }
    }
}

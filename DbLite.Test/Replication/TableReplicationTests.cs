namespace DbLite.Test.Replication
{
    using DbLite.Replication;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Tables;
    using Xunit;

    public abstract class TableReplicationTests<TDatabaseFixture, TConnection>
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

        protected TableReplicationTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Theory]
        [InlineData(10)]
        public void ReplicateInserts(int records)
        {
            // Syncs 10 records from Db2 to Db1
            var newRecords = Enumerable.Range(0, 10).Select(x => new ReplicationTestTable()
            {
                Id = Guid.NewGuid(),
                Source = "Db2",
                LastUpdated = DateTime.UtcNow,
                Value1 = "Test " + x,
                Value3 = "Test " + x
            }).ToArray();

            using (var db1 = Fixture.Open("Db1"))
            {
                using (var db2 = Fixture.Open("Db2"))
                {
                    var replicator = new TableReplication<ReplicationTestTable>(new ReplicationOptions
                    {
                        DestinationConnection = () => Fixture.Open("Db1"),
                        InstanceNames = new string[] { "Db1", "Db2" }
                    });

                    // Dry run
                    replicator.Run(db2);

                    // Insert records
                    db2.Insert(newRecords);

                    // Sync records
                    var result = replicator.Run(db2);

                    // Check that records was synced
                    Assert.Equal(10, result.RecordsAdded);
                    Assert.Equal(0, result.RecordsUpdated);
                }
            }
        }
    }
}

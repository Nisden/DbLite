﻿namespace DbLite.Test
{
    using DbLite.Test.Tables;
    using System.Collections.Generic;
    using System.Data;
    using Xunit;

    [Collection("Database Collection")]
    public abstract class DataReaderTests<TDatabaseFixture, TConnection> : IClassFixture<TDatabaseFixture>
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

        protected DataReaderTests(TDatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Read_SimpleTable()
        {
            using (var command = fixture.Db.CreateCommand())
            {
                command.CommandText = "SELECT * FROM SimpleTable";

                List<SimpleTable> result;
                using (var dataReader = command.ExecuteReader())
                {
                    result = dataReader.ToList<SimpleTable>();
                }

                Assert.Equal(100, result.Count);

                foreach (var item in result)
                {
                    Assert.NotEmpty(item.String1);
                    Assert.NotEmpty(item.String2);
                    Assert.InRange(item.Interger1, 5, 500);
                }
            }
        }
    }
}

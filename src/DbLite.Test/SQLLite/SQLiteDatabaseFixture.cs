using DbLite.Test.Tables;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLite;
using Xunit;

namespace DbLite.Test.SQLLite
{
    public class SQLiteDatabaseFixture : DatabaseFixture<Microsoft.Data.Sqlite.SqliteConnection>, IDisposable
    {
        private static string fileName;

        public override SqliteConnection Open()
        {
            var connection = new SqliteConnection($"Data Source={fileName};");
            connection.Open();

            return connection;
        }

        protected override void SetupDatabase()
        {
            fileName = System.IO.Path.GetTempFileName();

            using (var connection = Open())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE SimpleTable (
                                        String1 NVARCHAR,
                                        String2 NVARCHAR,
                                        Boolean1 BIT,
                                        Interger1 INT,
                                        Decimal1 DECIMAL)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE MultiKeyTable ( Id1 INT, Id2 NVARCHAR, Value NVARCHAR)";
                    command.ExecuteNonQuery();

                    command.CommandText = "CREATE TABLE AutoIncrementingTable (Id INTEGER PRIMARY KEY, Value NVARCHAR)";
                    command.ExecuteNonQuery();

                    // Insert data
                    var items = Enumerable.Range(1, 100).Select(x => new SimpleTable
                    {
                        String1 = "Test" + x,
                        String2 = "TestTest" + x,
                        Interger1 = 5 * x,
                        Decimal1 = 5.2m * x,
                        Boolean1 = false
                    }).ToArray();
                    connection.Insert(items);

                    connection.Insert(new MultiKeyTable()
                    {
                        Id1 = 1,
                        Id2 = "Hello",
                        Value = "my name is Mr. X"
                    });
                }
            }
        }

        public void Dispose()
        {
            if (fileName != null && System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
        }
    }

    [CollectionDefinition("SQLite")]
    public class SQLiteCollection : ICollectionFixture<SQLiteDatabaseFixture>
    {

    }
}

using DbLite.Test.Tables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLite;
using Xunit;
using System.IO;

namespace DbLite.Test.SQLLite
{
    public class SQLiteDatabaseFixture : DatabaseFixture<System.Data.SQLite.SQLiteConnection>, IDisposable
    {
        private Dictionary<string, string> namedDatabases = new Dictionary<string, string>();

        public override SQLiteConnection Open(string namedDatabase = "Default")
        {
            if (!namedDatabases.ContainsKey(namedDatabase))
                SetupDatabase(namedDatabase);

            var connection = new SQLiteConnection($"Data Source={namedDatabases[namedDatabase]};Version=3;");
            return connection.OpenAndReturn();
        }

        protected override void SetupDatabase(string databaseName)
        {
            string fileName = System.IO.Path.GetTempFileName();
            namedDatabases.Add(databaseName, fileName);

            using (var connection = Open(databaseName))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE SimpleTable (
                                        String1 NVARCHAR,
                                        String22 NVARCHAR,
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

        public override void Dispose()
        {
            foreach (var database in namedDatabases.Values)
            {
                if (File.Exists(database))
                    File.Delete(database);
            }
        }
    }

    [CollectionDefinition("SQLite")]
    public class SQLiteCollection : ICollectionFixture<SQLiteDatabaseFixture>
    {

    }
}

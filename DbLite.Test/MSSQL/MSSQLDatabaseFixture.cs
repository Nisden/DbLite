using DbLite.Test.Tables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLite;
using System.Data.SqlClient;
using Xunit;

namespace DbLite.Test.MSSQL
{
    public class MSSQLDatabaseFixture : DatabaseFixture<System.Data.SqlClient.SqlConnection>
    {
        public List<string> namedDatabases = new List<string>();

        public override SqlConnection Open(string namedDatabase = "Default")
        {
            if (!namedDatabases.Contains(namedDatabase))
                SetupDatabase(namedDatabase);

            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.InitialCatalog = "DbLiteTest_" + namedDatabase;
            connectionStringBuilder.DataSource = ".";
            connectionStringBuilder.IntegratedSecurity = true;

            var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }

        protected override void SetupDatabase(string databaseName)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.InitialCatalog = "master";
            connectionStringBuilder.DataSource = ".";
            connectionStringBuilder.IntegratedSecurity = true;

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"IF EXISTS (select * from sys.databases where name = 'DbLiteTest_{databaseName}')
                                             BEGIN
                                             	ALTER DATABASE DbLiteTest_{databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                                             	DROP DATABASE DbLiteTest_{databaseName}
                                             END
                                             CREATE DATABASE DbLiteTest_{databaseName}";
                    command.ExecuteNonQuery();

                    connection.ChangeDatabase("DbLiteTest_" + databaseName);
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE SimpleTable (
                                        String1 NVARCHAR(MAX),
                                        String22 NVARCHAR(MAX),
                                        Boolean1 BIT,
                                        Interger1 INT,
                                        Decimal1 DECIMAL)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE MultiKeyTable ( Id1 INT, Id2 NVARCHAR(MAX), Value NVARCHAR(MAX))";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE AutoIncrementingTable (
                                           	Id INT PRIMARY KEY IDENTITY,
                                           	Value NVARCHAR(MAX)
                                           )";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE ReplicationTestTable (
                                           	Id uniqueidentifier PRIMARY KEY,
                                           	Value1 NVARCHAR(MAX),
                                            Value2 NVARCHAR(MAX),
                                            Value3 NVARCHAR(MAX),
                                            Deleted BIT NOT NULL,
                                            LastUpdated DATETIME NOT NULL,
                                            Source NVARCHAR(MAX) NOT NULL)";
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

            namedDatabases.Add(databaseName);
        }

        public override void Dispose()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.InitialCatalog = "master";
            connectionStringBuilder.DataSource = ".";
            connectionStringBuilder.IntegratedSecurity = true;

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    foreach (var databaseName in namedDatabases)
                    {
                        command.CommandText = $@"ALTER DATABASE DbLiteTest_{databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE
                                                 DROP DATABASE DbLiteTest_{databaseName}";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    [CollectionDefinition("MSSQL")]
    public class MSSQLCollection : ICollectionFixture<MSSQLDatabaseFixture>
    {

    }
}

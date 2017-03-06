using DbLite.Test.Tables;
using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
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
        private static string baseConnectionString = Environment.GetEnvironmentVariable("MSSQLConnectionString") ?? "Server=.;Database=master;Trusted_Connection=True;";

        public override SqlConnection Open()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(baseConnectionString);
            connectionStringBuilder.InitialCatalog = "DbLiteTest";

            var connection = new SqlConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            return connection;
        }

        protected override void SetupDatabase()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(baseConnectionString);
            connectionStringBuilder.InitialCatalog = "master";

            using (var connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"IF EXISTS(SELECT * FROM sys.databases WHERE [name] = 'DbLiteTest')
                                        BEGIN
                                            DROP DATABASE DbLiteTest
                                        END
                                        
                                        CREATE DATABASE DbLiteTest
                                        SELECT 'DbLiteTest'";
                    connection.ChangeDatabase((string)command.ExecuteScalar());
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"CREATE TABLE SimpleTable (
                                        String1 NVARCHAR(MAX),
                                        String2 NVARCHAR(MAX),
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
    }

    [CollectionDefinition("MSSQL")]
    public class MSSQLCollection : ICollectionFixture<MSSQLDatabaseFixture>
    {

    }
}

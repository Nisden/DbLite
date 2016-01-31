using DbLite.Test.Tables;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLite;

namespace DbLite.Test.SQLLite
{
    public class SQLiteDatabaseFixture : DatabaseFixture<System.Data.SQLite.SQLiteConnection>
    {
        protected override SQLiteConnection Open()
        {
            var connection = new SQLiteConnection("Data Source=:memory:;Version=3;New=True;");
            return connection.OpenAndReturn();
        }

        protected override void SetupDatabase()
        {
            using (var command = Db.CreateCommand())
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

                // Insert data
                var items = Enumerable.Range(1, 100).Select(x => new SimpleTable
                {
                    String1 = "Test" + x,
                    String2 = "TestTest" + x,
                    Interger1 = 5 * x,
                    Decimal1 = 5.2m * x,
                    Boolean1 = false
                }).ToArray();
                Db.Insert(items);

                Db.Insert(new MultiKeyTable()
                {
                    Id1 = 1,
                    Id2 = "Hello",
                    Value = "my name is Mr. X"
                });
            }
        }
    }
}

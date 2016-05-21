# DbLite

[![Build status](https://ci.appveyor.com/api/projects/status/bw0rglovw5508yj1/branch/master?svg=true)](https://ci.appveyor.com/project/NsdWorkBook/dblite/branch/master)

A simple micro ORM

# Sample

```csharp
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
```

# Packages
* [![NuGet](https://img.shields.io/nuget/v/DbLite.svg?maxAge=2592000)](https://www.nuget.org/packages/DbLite/) DbLite 
* [![NuGet](https://img.shields.io/nuget/v/DbLite.MSSQL.svg?maxAge=2592000)](https://www.nuget.org/packages/DbLite.MSSQL/) DbLite.MSSQL
* [![NuGet](https://img.shields.io/nuget/v/DbLite.SQLite.svg?maxAge=2592000)](https://www.nuget.org/packages/DbLite.SQLite/) DbLite.SQLite

# Notes

* Everything is parameterized, no inline variables
* Currently there are only dialect providers for SQLite and MSSQL
* Test coverage is spotty

# Requirements

* Visual Studio 2015 or newer
* .NET 4.5.2

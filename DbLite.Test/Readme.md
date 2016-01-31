# DbLite

A simple micro ORM

# Sample

```csharp
// Get a instance of IDbConnection, this can be SQLiteConnection, SQLConnection, etc
var db = Fixture.Db;

using (db.BeginTransaction())
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
```

# Notes

* Everything is parameterized, no inline variables
* Currently there is only a dialect provider for SQLite
* Test coverage is spotty at the moment

# Requirements

* Visual Studio 2015 or newer
* .NET 4.5.2
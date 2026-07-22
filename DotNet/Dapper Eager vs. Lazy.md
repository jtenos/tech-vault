```csharp
using SqliteConnection conn = new(_config.GetConnectionString("MyDatabase")!);
await conn.OpenAsync(stoppingToken);

// Eager returning a list
IEnumerable<Foo> foos = conn.Query<Foo>("SELECT * FROM Foos", buffered: true/*default*/);
_logger.LogInformation("Buffered: {type}", foos.GetType().Name); // List`1
foreach (Foo foo in foos)
{
    _logger.LogInformation("Foo: {Foo}", foo.Name);
}

// Lazy IEnumerable instead of returning a list
foos = conn.Query<Foo>("SELECT * FROM Foos", buffered: false);
_logger.LogInformation("Not buffered: {type}", foos.GetType().Name); // <QueryImpl>d__149`1
foreach (Foo foo in foos)
{
    _logger.LogInformation("Foo: {Foo}", foo.Name);
}
```
```csharp
IServiceProvider serviceProvider = new ServiceCollection()
    .AddLogging(builder => builder.AddConsole())
    .AddDbContext<MyDbContext>((sp, options) =>
        options
            .UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>())
            .EnableDetailedErrors()       // <-----------------------
            .EnableSensitiveDataLogging() // <-----------------------
            .UseSqlite("Data Source=mydb.sqlite3")
    ).BuildServiceProvider();
```
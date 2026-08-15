```csharp
// dotnet add package Microsoft.EntityFrameworkCore.Proxies

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_connectionString);

[Table("Customers")]
public class Customer {
    [Key]
    public int CustomerID { get; set; }
    public string Name { get; set; } = "";
 
    [InverseProperty(nameof(Customer))]
    public virtual List<Car>? Cars { get; set; }
}
 
[Table("Cars")]
public class Car {
    [Key]
    public int CarID { get; set; }
    public string Make { get; set; } = "";
    public int CustomerID { get; set; }
 
    [ForeignKey(nameof(CustomerID))]
    public virtual Customer? Customer { get; set; }
}
 
// Use lazy loading: easiest, but requires two database calls
async static Task Main() {
    using var context = new MyContext();
 
    /*
        SELECT TOP(2) [c].[CustomerID], [c].[Name]
        FROM [Customers] AS [c]
        WHERE [c].[CustomerID] = 4
    */
    var cust = await context.Customers.SingleAsync(x => x.CustomerID == 4).ConfigureAwait(false);
    Console.WriteLine(cust.Name);
 
    /*
        exec sp_executesql N'SELECT [c].[CarID], [c].[CustomerID], [c].[Make]
        FROM [Cars] AS [c]
        WHERE [c].[CustomerID] = @__p_0',N'@__p_0 int',@__p_0=4
    */
    foreach (var car in cust.Cars!) {
        Console.WriteLine($"{car.CarID}: {car.Make}");
    }
}
 
// Get the data upfront using Include - single database call, but more complex query
async static Task Main() {
    using var context = new MyContext();
 
    /*
        SELECT [t].[CustomerID], [t].[Name], [c0].[CarID], [c0].[CustomerID], [c0].[Make]
        FROM (
            SELECT TOP(2) [c].[CustomerID], [c].[Name]
            FROM [Customers] AS [c]
            WHERE [c].[CustomerID] = 4
        ) AS [t]
        LEFT JOIN [Cars] AS [c0] ON [t].[CustomerID] = [c0].[CustomerID]
        ORDER BY [t].[CustomerID], [c0].[CarID]
    */
    var cust = await context.Customers.Include(c => c.Cars)
        .SingleAsync(x => x.CustomerID == 4).ConfigureAwait(false);
    Console.WriteLine(cust.Name);
 
    foreach (var car in cust.Cars!) {
        Console.WriteLine($"{car.CarID}: {car.Make}");
    }
}
```
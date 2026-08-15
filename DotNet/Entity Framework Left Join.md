```csharp
// Database structure
create table dbo.Foods (
  FoodID int not null identity primary key
 ,FoodName varchar(100) not null
);
go
insert dbo.Foods (FoodName) values ('Pizza'), ('Chicken'), ('Potatoes'), ('Broccoli');
go
create table dbo.People (
 PersonID int not null identity primary key
 ,FirstName varchar(100) not null
 ,FavoriteFoodID int null -- We will left join against this
 ,constraint FK_Person_FavoriteFoodID
  foreign key (FavoriteFoodID) references dbo.Foods (FoodID)
);
go
insert dbo.People (FirstName, FavoriteFoodID)
values ('John', 1), ('Mary', 2), ('Pat', null);
go


[Table("Foods")]
public class Food {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FoodID { get; set; }
    public string FoodName { get; set; } = "";
}
 
[Table("People")]
public class Person {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PersonID { get; set; }
    public string FirstName { get; set; } = "";
    public int? FavoriteFoodID { get; set; }
 
    [ForeignKey(nameof(FavoriteFoodID))]
    public Food? FavoriteFood { get; set; }
}
 
public class MyContext : DbContext {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlServer(@"server=(localdb)\MSSQLLocalDB;database=sandbox20200409;integrated security=true;");
    }
 
    public DbSet<Food>? Foods { get; set; }
    public DbSet<Person>? People { get; set; }
}

using var context = new MyContext();

// Navigation properties
var firstQuery = (from p in context.People
                  select new
                  {
                      p.PersonID,
                      p.FirstName,
                      p.FavoriteFood!.FoodID,
                      p.FavoriteFood.FoodName
                  }).ToArray();

// Without navigation properties
var secondQuery = (from p in context.People
                   from f in context.Foods.Where(f => f.FoodID == p.FavoriteFoodID).DefaultIfEmpty()
                   select new
                   {
                       p.PersonID,
                       p.FirstName,
                       f.FoodID,
                       f.FoodName
                   }).ToArray();

SELECT [p].[PersonID], [p].[FirstName], [f].[FoodID], [f].[FoodName]
FROM [People] AS [p]
LEFT JOIN [Foods] AS [f] ON [p].[FavoriteFoodID] = [f].[FoodID]
```
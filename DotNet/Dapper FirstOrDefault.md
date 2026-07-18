If there are no results returned from the query, here’s what happens:

- QueryFirstOrDefault<Foo>:
- If Foo is a struct, then you’ll get the zero-value of the struct
  - If Foo is a class, then you’ll get NULL
- QueryFirstOrDefault<Foo>:
  - For both class and struct, you’ll get NULL

Here’s some code to demonstrate:



```csharp
/*
create table dbo.Suns (
     ID int not null identity primary key
     ,FirstName varchar(100) not null
);

go

insert into dbo.Suns (FirstName) values ('John'), ('Jane'), ('Mary'), ('Pat');
using Microsoft.Data.SqlClient;
using Dapper;
*/

using SqlConnection conn = new("Data Source=.;Initial Catalog=sandbox;Integrated Security=SSPI;TrustServerCertificate=True;");
conn.Open();

void Go<T>(string msg){
    Console.WriteLine(msg);
    var item = conn.QueryFirstOrDefault<T>("SELECT * FROM dbo.Suns WHERE ID = @ID",new { ID = 5 });
    if (item is null) { Console.WriteLine("NULL"); }
    else { Console.WriteLine(item); }

    Console.WriteLine("------------------------");
}

Go<SunRecordClass>("SunRecordClass");
Go<SunRecordClass?>("SunRecordClass?");

Go<SunRecordStruct>("SunRecordStruct");
Go<SunRecordStruct?>("SunRecordStruct?");

Go<SunClass>("SunClass");
Go<SunClass?>("SunClass?");

Go<SunStruct>("SunStruct");
Go<SunStruct?>("SunStruct?");

public record class SunRecordClass(int ID, string FirstName);
public record struct SunRecordStruct(int ID, string FirstName);

public record class SunClass
{
    public int ID { get; init; }
    public string FirstName { get; init; } = default!;

}
public record struct SunStruct
{
    public SunStruct(int id, string firstName) => (ID, FirstName) = (id, firstName);
    public int ID { get; init; }
    public string FirstName { get; init; } = default!;
}
SunRecordClass
NULL
------------------------
SunRecordClass?
NULL
------------------------
SunRecordStruct
SunRecordStruct { ID = 0, FirstName =  }
------------------------
SunRecordStruct?
NULL
------------------------
SunClass
NULL
------------------------
SunClass?
NULL
------------------------
SunStruct
SunStruct { ID = 0, FirstName =  }
------------------------
SunStruct?
NULL
------------------------
```
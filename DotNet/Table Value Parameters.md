```sql
create type dbo.PersonType as table
(
    Id int
    ,Name varchar(50)
);
go
create procedure dbo.TestPerson
(
    @people dbo.PersonType readonly
)
as
begin
    select * from @people;
end;
go
```

```csharp
private static SqlDataRecord CreatePersonRecord(int id, string name) {
    var metaData = new[] {
            new SqlMetaData("Id", SqlDbType.Int),
            new SqlMetaData("Name", SqlDbType.VarChar, 50)
        };
    var record = new SqlDataRecord(metaData);
    record.SetInt32(0, id);
    record.SetString(1, name);
    return record;
}

private SqlDataRecord[] GetPersonRecords() {
    retun new[] {  CreatePersonRecord(1, "John Doe"), CreatePersonRecord(2, "Jane Doe") };
}

using (var conn = new SqlConnection(cs)) {
    conn.Open();
    using (var comm = conn.CreateCommand()) {
        comm.CommandText = "dbo.TestPerson";
        comm.CommandType = CommandType.StoredProcedure;
       
        var peopleParmsValue = GetPersonRecords();
        if (!peopleParmsValue.Any()) {
            peopleParmsValue = null; // Don't set the value to an empty array
        }
       
        comm.Parameters.Add(new SqlParameter {
            ParameterName = "@people",
            Value = peopleParmsValue,
            SqlDbType = SqlDbType.Structured,
            TypeName = "dbo.PersonType"
        });

        using (var rdr = comm.ExecuteReader()) {
            while (rdr.Read()) {
                Console.WriteLine("{0}: {1}", rdr["Id"], rdr["Name"]);
            }
        }
    }
}
```
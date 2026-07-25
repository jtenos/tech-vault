```sql
create procedure dbo.TestProc
(
    @customError bit
    ,@sqlError bit
)
as
begin
    begin transaction;
    begin try
        if @customError = 1
        begin
            raiserror('This is my custom error.', 16, 99);
            return;
        end;
 
        if @sqlError = 1
        begin
            exec ('select * from SomeFakeTable');
            return;
        end;
 
        select 1;
 
        commit transaction;
    end try
    begin catch
        if @@trancount > 0
            rollback transaction;
        declare @errorMessage nvarchar(4000) = error_message();
        declare @errorSeverity int = error_severity();
        declare @errorState int = error_state();
        declare @procName nvarchar(4000) = object_name(@@procid);
 
        -- TODO: Log Error information
 
        raiserror(@errorMessage, @errorSeverity, @errorState);
    end catch;
end;
go
```

```csharp
try {
  string cs = "...";
  bool customError = false, sqlError = false;
   
  using (var conn = new SqlConnection(cs)) {
    conn.Open();
    using (var comm = conn.CreateCommand()) {
      comm.CommandText = "dbo.TestProc";
      comm.CommandType = CommandType.StoredProcedure;
      comm.Parameters.AddRange(new[] {
        new SqlParameter("@customError", customError),
        new SqlParameter("@sqlError", sqlError)
      });
      using (var rdr = comm.ExecuteReader()) {
        if (rdr.Read()) {
          Console.WriteLine("Success: " + rdr[0]);
        }
      }
    }
  }
} catch (SqlException ex) {
  if (ex.Class == 16 && ex.State == 99) {
    Console.WriteLine("Custom error: " + ex.Message);
  } else {
    Console.WriteLine("SQL error: " + ex.Message);
  }
}
catch (Exception ex) {
  Console.WriteLine(ex);
}
 
// For executing a proc through Entity Framework (maybe rewrite this as catch when)
catch (EntityCommandExecutionException ex) {
  var sqlException = ex.InnerException as SqlException;
  if (sqlException != null && sqlException.Class == 16 && sqlException.State == 99) {
    // Custom error
  }
}
```
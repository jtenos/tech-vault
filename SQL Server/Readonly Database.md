```sql
-- Set to readonly:
use master;
go
alter database mydatabase set read_only with no_wait;
go

-- Re-enable writing:
use master;
go
alter database mydatabase set read_write with no_wait;
go
```
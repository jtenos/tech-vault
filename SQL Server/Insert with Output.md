This is an alternative to things like scope_identity. This appears to be a better all-around solution, since it can handle multiple inserts, doesn't have any possibility of conflicts with other processes, and can handle other non-identity computed columns with the same syntax.

```sql
declare @result table (ID int);

-- dbo.Records has identity column ID
insert dbo.Records (PersonName)
output inserted.ID into @result
values (@PersonName);

select ID from @result; -- The newly created identity
```


```sql
create procedure util.RebuildAllIndexes
as
begin
    declare @msg nvarchar(1000);
    set @msg =  '------------- Executing RebuildAllIndexes -------------';
    raiserror(@msg, 0, 1) with nowait;

    set nocount on;

    if object_id('tempdb..#tables') is not null
        drop table #tables;

    select '[' + object_schema_name([object_id]) + '].[' + [name] + ']' [fullName]
    into #tables
    from sys.tables;

    declare @cur cursor;
    set @cur = cursor local fast_forward for
        select fullName from #tables order by fullName;

    declare @sql nvarchar(max) = 'declare @msg nvarchar(1000);' + char(10);

    declare @fullName nvarchar(261);
    open @cur
    while 1=1
    begin
        fetch next from @cur into @fullName;
        if @@fetch_status <> 0
            break;

        set @sql += 'set @msg = convert(varchar, getutcdate(), 127) + '' Working on ' + @fullName + '''' + char(10)
            + 'raiserror(@msg, 0, 1) with nowait;' + char(10)
            + 'alter index all on ' + @fullName + ' rebuild;' + char(10)
            + 'set @msg = convert(varchar, getutcdate(), 127) + '' Done''' + char(10)
            + 'raiserror(@msg, 0, 1) with nowait;' + char(10)
    end;
    close @cur;
    deallocate @cur;

    exec sp_executesql @sql;
end;
go
```
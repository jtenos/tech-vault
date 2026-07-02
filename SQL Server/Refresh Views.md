```sql
create procedure util.RefreshViews
as
begin
    declare @msg nvarchar(1000);
    set @msg = '------------- Executing RefreshViews -------------';
    raiserror(@msg, 0, 1) with nowait;
       
    set nocount on;

    declare @viewNames table
    (
        FullName nvarchar(261) -- '[' + 128 + '].[' + 128 + ']'
    );

    insert @viewNames (FullName)
        select '[' + object_schema_name([object_id]) + '].[' + [name] + ']'
        from sys.views;

    declare @fullName nvarchar(261);

    select top 1 @fullName = FullName from @viewNames order by FullName;
    while exists (select 1 from @viewNames)
    begin
        set @msg = convert(varchar, getutcdate(), 127) + ' Working on ' + @fullName;
        raiserror(@msg, 0, 1) with nowait;

        begin try
            exec ('sp_refreshview N''' + @fullName + '''');
            set @msg = convert(varchar, getutcdate(), 127) + ' Done';
            raiserror(@msg, 0, 1) with nowait;
        end try
        begin catch
            raiserror('Failed', 0, 1) with nowait;
        end catch;

        delete @viewNames where FullName = @fullName;
        select top 1 @fullName = FullName from @viewNames order by FullName;
    end;
end;
go
```
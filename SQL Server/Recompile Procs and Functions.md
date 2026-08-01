```sql
create procedure util.RecompileProcsAndFunctions
as
begin
    declare @msg nvarchar(1000);
    set @msg = '------------- Executing RecompileProcsAndFunctions -------------';
    raiserror(@msg,0,1) with nowait;

    set nocount on;

    declare @objectNames table
    (
        FullName nvarchar(261) -- '[' + 128 + '].[' + 128 + ']'
    );

    insert @objectNames (FullName)
        select '[' + object_schema_name([object_id]) + '].[' + [name] + ']'
        from sys.objects
        where type in ('P','IF','FN');

    declare @fullName nvarchar(261);

    select top 1 @fullName = FullName from @objectNames order by FullName;
    while exists (select 1 from @objectNames)
    begin
        set @msg = convert(varchar, getutcdate(), 127) + ' Working on ' + @fullName;
        raiserror(@msg,0,1) with nowait;

        begin try
            exec ('sp_recompile N''' + @fullName + '''')

            set @msg = convert(varchar, getutcdate(), 127) + ' Done';
            raiserror(@msg,0,1) with nowait;
        end try
        begin catch
            raiserror('Failed...', 0, 1) with nowait;
        end catch;
           
        delete @objectNames where FullName = @fullName;
        select top 1 @fullName = FullName from @objectNames order by FullName;
    end;
end;
go
```
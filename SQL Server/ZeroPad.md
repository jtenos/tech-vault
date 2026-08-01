```sql
create function util.ZeroPad
(
    @i bigint
    ,@numTotalDigits int
)
returns varchar(100)
as
begin
    declare @result varchar(100);
    select @result = right(replicate('0', @numTotalDigits) + cast(@i as varchar(100)), @numTotalDigits);
    return @result;
end;
go

-- or FORMAT(@i, '000000')
```
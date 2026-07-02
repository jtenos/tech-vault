```sql
create function dbo.ConvertFromBase64String (@Base64Value varchar(max))
returns varbinary(max)
as
begin
    return cast('' as xml).value('xs:base64Binary(sql:variable("@Base64Value"))', 'varbinary(max)');
end;
go

create function dbo.ConvertToBase64String (@BinaryValue varbinary(max))
returns varchar(max)
as
begin
    return cast('' as xml).value('xs:base64Binary(sql:variable("@BinaryValue"))','varchar(max)');
end;
go

declare @b varbinary(max) = cast('hello' as varbinary(max));
select @b; -- 0x68656C6C6F
 
declare @b64 varchar(max);
select @b64 = dbo.ConvertToBase64String(@b);
select @b64; -- aGVsbG8=
 
declare @result varbinary(max);
select @result = dbo.ConvertFromBase64String(@b64);
select @result; -- 0x68656C6C6F
 
declare @t varchar(max);
select @t = cast(@result as varchar(max));
select @t; -- hello

GO
————

CREATE FUNCTION util.ConvertFromBase64 (
    @Input NVARCHAR(MAX)
)
RETURNS VARBINARY(MAX)
AS
BEGIN
    -- Note that "value" is case-sensitive (must be lower-case)
    RETURN CAST(@Input AS XML).value('.', 'VARBINARY(MAX)');
END;
GO

CREATE FUNCTION util.ConvertToBase64 (
    @Input VARBINARY(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (
        SELECT @Input
        FOR XML PATH(''), BINARY BASE64
    );
END
GO

```
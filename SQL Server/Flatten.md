```sql
CREATE TYPE [dbo].[SingleColumnText] AS TABLE (
    [TextValue] NVARCHAR (MAX) NULL);
GO
CREATE FUNCTION [dbo].[Flatten_Table] (
    @Input dbo.SingleColumnText READONLY
    ,@Delimiter NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (
        SELECT STUFF((
            SELECT @Delimiter + [TextValue]
            FROM @Input
            FOR XML PATH(''), TYPE
        ).value('(./text())[1]', 'NVARCHAR(MAX)'), 1, 1, '')
    );
END;
GO

---------------------------------------------------------------------

CREATE FUNCTION [dbo].[Flatten_XML] (
    @Values XML -- <items><item v="abc"/><item v="def"/></items>
    ,@Delimiter NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (
SELECT STUFF((
            SELECT @Delimiter + [TextValue]
            FROM (
SELECT X.v.value('.', 'NVARCHAR(MAX)')
FROM @Values.nodes('/items/item/@v') X(v)
) AS tbl ([TextValue])
            FOR XML PATH(''), TYPE
        ).value('(./text())[1]', 'NVARCHAR(MAX)'), 1, 1, '')
    );
END;
GO

---------------------------------------------------------------------

CREATE FUNCTION [dbo].[Flatten_JSON] (
    @Values NVARCHAR(MAX) -- ["abc","def"]
    ,@Delimiter NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    RETURN (
SELECT STUFF((
            SELECT @Delimiter + [TextValue]
            FROM (
SELECT [TextValue]
FROM OPENJSON(@Values) WITH ([TextValue] NVARCHAR(MAX) '$')
) AS tbl ([TextValue])
            FOR XML PATH(''), TYPE
        ).value('(./text())[1]', 'NVARCHAR(MAX)'), 1, 1, '')
    );
END;
GO

---------------------------------------------------------------------

DECLARE @tbl [dbo].[SingleColumnText];
INSERT INTO @tbl VALUES ('abc'), ('def');
SELECT [dbo].[Flatten_Table] (@tbl, ',');

DECLARE @xml XML = '<items><item v="abc"/><item v="def"/></items>';
SELECT [dbo].[Flatten_XML] (@xml, ',');

DECLARE @json NVARCHAR(MAX) = '["abc","def"]';
SELECT [dbo].[Flatten_JSON] (@json, ',');

---------------------------------------------------------------------

create function util.Flatten
(
    @input util.SingleColumnText readonly
    ,@delimiter nvarchar(max)
)
returns nvarchar(max)
as
begin
    declare @result nvarchar(max);
    select @result = coalesce(@result + @delimiter, '') + TextValue
    from @input
    order by PK;
 
    return @result;
end;
go
create function util.Unflatten
(
    @input nvarchar(max)
    ,@delimiter nchar(1)
)
returns table
as
    return
    (
        select
            row_number() over (order by n) - 1 [Idx]
            ,substring(@input, n, charindex(@delimiter, @input + @delimiter, n) - n) [TextValue]
        from util.Numbers
        where n <= len(@input)
        and substring(@delimiter + @input, n, 1) = @delimiter
    );
go
```
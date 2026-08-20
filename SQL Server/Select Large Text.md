If you have a large string, possibly with line breaks, it may be difficult to read this in SSMS.

There are ways to do it, or you can do it outside the editor, but if you’re looking for something
quick and dirty to let you analyze text, throw your text into a variable and call the following.

The result will be a clickable XML value, with the text inside a CDATA element.

```
create procedure util.SelectStringAsCdata
(
    @text varchar(max)
)
as
begin
    -- This creates a clickable XML result which opens a new tab in SSMS, allowing you to see
    -- a large text value. PRINT and RAISERROR have limitations, but this will show any
    -- size result.
 
    declare @crlf char(2) = char(13) + char(10);
    select 1 [TAG], 0 [PARENT], @crlf + @text + @crlf [root!1!!CDATA] for xml explicit;
end;
go

```

I was hoping to build a function that returns XML, so you could select text like this in a regular query, but apparently SQL Server does not keep CDATA in XML values.

https://social.msdn.microsoft.com/Forums/sqlserver/en-US/e22efff3-192e-468e-b173-ced52ada857f/how-to-get-cdata-with-for-xml-path?forum=sqlxml
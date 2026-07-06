Very immature procedure - just queries a single XML document to retrieve a single column. Haven't explored more useful things like querying for multiple columns.

```sql
declare @parm varchar(10);
set @parm='2';

declare @tbl table (SomeXMLColumn xml);
insert @tbl (SomeXMLColumn)
values ('<root>
<Foo id="1"><Bar>A</Bar></Foo>
<Foo id="2"><Bar>B</Bar></Foo>
<Foo id="3"><Bar>C</Bar></Foo>
</root>');

select x.value('.', 'varchar(max)')
from @tbl tbl
cross apply
    tbl.SomeXMLColumn.nodes('//Foo[@id=sql:variable("@parm")]/Bar/text()') x(x);
```
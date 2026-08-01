```sql
-- Simple conversion from XML values to single-column data records, useful for
-- things like passing collections around

-- convert-xml-to-values

declare @RecordDataXML xml;
select @RecordDataXML = '[fields][f v="a" /][f v="b" /][f v="c" /][f v="d" /][/fields]';

select n.n, x.value('.', 'varchar(max)')
from dbo.Numbers n
cross apply @RecordDataXML.nodes('/fields/f[sql:column("n.n")]/@v') x(x)
where n.n between 1 and @RecordDataXML.value('count(/fields/f)', 'int')
order by n.n;

go

-- convert-values-to-xml

declare @tbl table (ord int, val varchar(100));
insert @tbl values (1, 'a'), (2, 'b'), (4, 'd'), (3, 'c');

declare @xml xml;
set @xml = (
    select val [@v]
    from @tbl
    order by ord
    for xml path ('f'), type
);

set @xml = (select @xml for xml path ('fields'), type);

select @xml;

go
```
```sql
declare @tbl table (id int, xval xml);
insert @tbl values
    (1, '<root><items><foo id="34">some text</foo></items></root>')
    ,(2, '<root><items><foo id="56">more text</foo></items></root>');
 
select
    t.id
    ,x.value('@id', 'int') [FooID]
    ,x.value('.', 'varchar(max)') [FooText]
from @tbl t
outer apply t.xval.nodes('/root/items/foo') x(x);
go

/*
id     FooID   FooText
--------------------------
1      34      some text
2      56      some text
*/
```

With variables:

```sql
-- Pick the third item from each record
declare @idx int = 3;
declare @tbl table (id int, xval xml);
insert @tbl values
    (1, '<root><item>34</item><item>42</item><item>22</item></root>')
    ,(2, '<root><item>87</item><item>93</item><item>14</item></root>');
 
select
tbl.id
,x.value('.', 'int') [ValueByVariable]
from @tbl tbl
outer apply tbl.xval.nodes('root/item[sql:variable("@idx")]/text()') x(x)
go

/*
id  ValueByVariable
1   22
2   14
*/
```
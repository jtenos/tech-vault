```sql
if object_id('tempdb..#joe') is not null
    drop table #joe;

go

create table #joe (id int not null, name varchar(100));

insert #joe
    select 1, 'John'
    union select 2, 'Jane'
    union select 3, 'Mary'
    union select 4, 'Pat';

declare @newValues table (id int not null, name varchar(100));
insert @newValues values (1, 'John'), (2, 'Phil'), (4, 'Pat'), (5, 'George');

select * from #joe;
select * from @newValues;

merge #joe tgt
using (select id, name from @newValues) src
on tgt.id = src.id
when matched then update set name = src.name
when not matched by source then delete
when not matched by target then insert (id, name) values (src.id, src.name);

select * from #joe;
```
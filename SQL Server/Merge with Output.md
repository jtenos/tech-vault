```sql
if object_id('tempdb..#joe') is not null
    drop table #joe;

go

declare @results table (id int);

create table #joe (id int not null identity, name varchar(100));

insert #joe
    select 'John' [name]
    union select 'Jane'
    union select 'Mary'
    union select 'Pat';

select * from #joe order by id;

declare @id int, @name varchar(100);

-- Update
--set @id = 2; set @name = 'Phil';

-- Insert
--set @id = null; set @name = 'Phil';

merge #joe tgt
using (select @id [id]) src
on tgt.id = src.id
when matched then
    update set name = @name
when not matched then
    insert (name)
    values (@name)
output inserted.id into @results;

select * from #joe order by id;
select * from @results;
```
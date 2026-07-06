```sql
if object_id('tempdb..#joe') is not null
    drop table #joe;
 
select a, b
into #joe
from
(
    select 1 a, null b
    union select 11 a, null b
    union select 111 a, null b
) x;
 
alter table #joe add primary key (a);
 
select * from #joe;
 
declare @cur cursor;
 
set @cur = cursor local forward_only for
    select a from #joe
    for update of b;
open @cur;
 
declare @a int;
while 1=1
begin
    fetch next from @cur into @a;
    if @@fetch_status <> 0 break;
 
    update #joe set b = a * 2 where current of @cur;
end;
 
close @cur;
deallocate @cur;
 
select * from #joe;
```
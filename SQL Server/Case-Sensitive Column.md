```sql
select serverproperty('collation');

-- DEFAULT (CASE-INSENSITIVE)

if object_id('tempdb..#tmp') is not null
  drop table #tmp;
 
create table #tmp (i int not null identity, val varchar(10));
 
insert #tmp (val) select 'a' where not exists (select 1 from #tmp where val = 'a');
insert #tmp (val) select 'a' where not exists (select 1 from #tmp where val = 'a');
insert #tmp (val) select 'A' where not exists (select 1 from #tmp where val = 'A');
insert #tmp (val) select 'A' where not exists (select 1 from #tmp where val = 'A');
 
select * from #tmp;

GO

-- CASE-SENSITIVE

if object_id('tempdb..#tmp') is not null
 drop table #tmp;
 
create table #tmp (i int not null identity, val varchar(10) collate Latin1_General_CS_AS);
 
insert #tmp (val) select 'a' where not exists (select 1 from #tmp where val = 'a');
insert #tmp (val) select 'a' where not exists (select 1 from #tmp where val = 'a');
insert #tmp (val) select 'A' where not exists (select 1 from #tmp where val = 'A');
insert #tmp (val) select 'A' where not exists (select 1 from #tmp where val = 'A');
 
select * from #tmp;

GO

--LIST COLLATIONS:

select [name], [description] from fn_helpcollations() order by [name];
```
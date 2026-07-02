```sql
create table dbo.Dates (
    TheDate date not null
    ,constraint PK_Dates primary key (TheDate)
);
go
 
declare @StartDate date = '1950-01-01';
declare @EndDate date = '2099-12-31';
;with cte as (
    select @StartDate [TheDate]
    union all
    select dateadd(day, 1, [TheDate])
    from cte
    where dateadd(day, 1, [TheDate]) <= @EndDate
)
insert dbo.Dates (TheDate)
select TheDate from cte
option (maxrecursion 0);
go
 
select * from dbo.Dates order by TheDate;
go
```
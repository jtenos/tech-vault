Old-fashioned way of formatting

```sql
declare @theDate datetime = getdate();
 
select
    -- yyyy-MM-dd
    convert(char(10), @theDate, 126)
 
    -- MM/dd/yyyy
    ,convert(char(10), @theDate, 101)
 
    -- yyyy-MM-dd HH:mm:ss
    ,convert(char(19), @theDate, 120)
 
    -- yyyy-MM-dd HH:mm:ss.fff
    ,convert(char(23), @theDate, 121)
 
    -- yyyy-MM-ddTHH:mm:ss.fff (ISO8601)
    ,convert(char(23), @theDate, 126)
 
    -- MMM _d yyyy _h:mm:ss.fff
    --(if day/hour < 10, space is inserted so len is always 26)
    ,convert(char(26), @theDate, 109)
```
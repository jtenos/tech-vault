```sql
create procedure util.ViewJobHistory
(
    @ExecutionDate date = null
    ,@JobName sysname = null
)
as
begin
    declare @executionDateInt int;
    set @executionDateInt = year(@ExecutionDate) * 10000
        + month(@ExecutionDate) * 100
        + day(@ExecutionDate);
 
    select
        JobName
        ,JobSuccess
        ,RunDateFormatted + ' '
            + left(RunTimePadded, 2)
            + ':' + substring(RunTimePadded, 3, 2)
            + ':' + right(RunTimePadded, 2) [RunDateTime]
        ,DurationSeconds + (60 * DurationMinutes) + (60 * 60 * DurationHours) [TotalDurationSeconds]
    from
    (
        select
            sj.name [JobName]
            ,case sjh.run_status
                when 1 then 'Success'
                else 'FAIL'
            end [JobSuccess]
            ,left(cast(sjh.run_date as varchar(8)), 4)
                + '-'
                + substring(cast(sjh.run_date as varchar(8)), 5, 2)
                + '-'
                + right(cast(sjh.run_date as varchar(8)), 2) [RunDateFormatted]
            ,right('000000' + cast(sjh.run_time as varchar(6)), 6) [RunTimePadded]
            ,sjh.run_duration / 10000 [DurationHours]
            ,(sjh.run_duration - (sjh.run_duration / 10000 * 10000)) / 100 [DurationMinutes]
            ,(sjh.run_duration - (sjh.run_duration / 100 * 100)) [DurationSeconds]
        from msdb..sysjobs sj
        join msdb..sysjobhistory sjh on sj.job_id = sjh.job_id
        where isnull(@executionDateInt, sjh.run_date) = sjh.run_date
        and isnull(@JobName, sj.name) = sj.name
        and sjh.step_id = 0
    ) x
end;
go
```
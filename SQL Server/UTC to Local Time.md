https://stackoverflow.com/questions/80https://stackoverflow.com/questions/8038744/convert-datetime-column-from-utc-to-local-time-in-select-statement/803879238744/convert-datetime-column-from-utc-to-local-time-in-select-statement/8038792

```sql
CONVERT(
    DATETIME,
    SWITCHOFFSET(
        CONVERT(DATETIMEOFFSET, MyTable.UtcColumn),
        DATENAME(TzOffset, SYSDATETIMEOFFSET())
    )
) AS ColumnInLocalTime
```
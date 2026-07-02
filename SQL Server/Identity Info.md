```sql
WITH cte AS ( 
    SELECT 
    s.name + '.' + t.name AS TableName, 
    c.name AS ColumnName 
    FROM sys.tables t 
    INNER JOIN sys.schemas s ON t.schema_id = s.schema_id 
    INNER JOIN sys.columns c ON c.object_id = t.object_id 
    AND c.is_identity = 1 
) 
SELECT 
    TableName, 
    ColumnName, 
    IDENT_SEED(TableName) AS Seed, 
    IDENT_INCR(TableName) AS Increment, 
    IDENT_CURRENT(TableName) AS LastIdentity 
FROM cte 
ORDER BY TableName
```
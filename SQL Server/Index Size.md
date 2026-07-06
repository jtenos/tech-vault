Modified from this SQLShack article.
https://www.sqlshack.com/how-to-monitor-total-sql-server-indexes-size/

```sql
DECLARE @SchemaName SYSNAME = N'dbo';
DECLARE @TableName SYSNAME = N'MyTableName';

DECLARE @object_id INT;
SELECT @object_id = [object_id]
FROM sys.tables
WHERE [schema_id] = SCHEMA_ID(@SchemaName)
AND [name] = @TableName;


SELECT
    tn.[name] [TableName]
    ,ix.[name] [IndexName]
    ,FORMAT(SUM(sz.[used_page_count]) * 8 / 1024, '#,##0') AS [Index size (MB)]
FROM sys.dm_db_partition_stats AS sz
INNER JOIN sys.indexes ix
ON sz.[object_id] = ix.[object_id]
AND sz.[index_id] = ix.[index_id]
INNER JOIN sys.tables tn ON tn.OBJECT_ID = ix.object_id
WHERE tn.[object_id] = @object_id
GROUP BY tn.[name], ix.[name]
ORDER BY SUM(sz.[used_page_count]) * 8 DESC;
```

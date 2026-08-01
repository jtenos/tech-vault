Modified from RedGate SQL Scripts Manager
https://www.red-gate.com/products/dba/sql-scripts-manager/

```sql
DECLARE @SchemaName SYSNAME = N'dbo';
DECLARE @TableName SYSNAME = N'MyTableName';

DECLARE @object_id INT;
SELECT @object_id = [object_id]
FROM sys.tables
WHERE [schema_id] = SCHEMA_ID(@SchemaName)
AND [name] = @TableName;

--fragmentation
SELECT
    i.[name] [index]
    ,ddips.[index_type_desc]
    ,ddips.[avg_fragmentation_in_percent] [FragmentationPercent]
    ,ddips.[fragment_count]
    ,ddips.[page_count]
FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'limited') ddips
JOIN sys.[indexes] i ON ddips.[object_id] = i.[object_id] AND ddips.[index_id] = i.[index_id]
WHERE ddips.[object_id] = @object_id
AND ddips.alloc_unit_type_desc = 'IN_ROW_DATA'
ORDER BY ddips.[avg_fragmentation_in_percent] DESC;
```
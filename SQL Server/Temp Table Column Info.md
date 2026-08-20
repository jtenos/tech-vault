```sql
/*
You need to make sure sp_help runs in the same database where the table is located (tempdb).
You can do this by prefixing the call directly:
*/

EXEC tempdb.dbo.sp_help @objname = N'#temp';

--Or by prefixing a join against tempdb.sys.columns:

SELECT [column] = c.name,
       [type] = t.name, c.max_length, c.precision, c.scale, c.is_nullable
    FROM tempdb.sys.columns AS c
    INNER JOIN tempdb.sys.types AS t
    ON c.system_type_id = t.system_type_id
    AND t.system_type_id = t.user_type_id
    WHERE [object_id] = OBJECT_ID(N'tempdb.dbo.#temp');

-- This doesn't handle nice things for you, like adjusting max_length for varchar differently from nvarchar, but it's a good start.

-- In SQL Server 2012 or better, you can use a new DMF to describe a resultset, which takes that issue away (and also assembles max_length/precision/scale for you). But it doesn't support #temp tables, so just inject the query without the INTO:

SELECT name, system_type_name, is_nullable
  FROM sys.dm_exec_describe_first_result_set(N'SELECT
        a.col1,
        a.col2,
        b.col1...
      --INTO #temp
      FROM ...;',NULL,1);
```
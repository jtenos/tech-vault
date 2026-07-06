```sql
CREATE TABLE [dbo].[Numbers] (
  [Num] INT NOT NULL
    CONSTRAINT [PK_Numbers] PRIMARY KEY
);
GO

-- TODO: Need to verify that this syntax is correct
INSERT INTO [dbo].[Numbers] ([Num])
SELECT [q].[Num] FROM (
  SELECT ROW_NUMBER() OVER (ORDER BY [o1].[object_id]) AS [Num]
  FROM [sys].[objects] AS [o1]
  CROSS JOIN [sys].[objects] AS [o2]
  CROSS JOIN [sys].[objects] AS [o3]
  CROSS JOIN [sys].[objects] AS [o4]
  CROSS JOIN [sys].[objects] AS [o5]
  CROSS JOIN [sys].[objects] AS [o6]
) AS [q]
WHERE [q].[Num] < 100000;
```
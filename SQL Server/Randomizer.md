```sql
-- Problem:

;WITH [tbl] AS (SELECT 1 [a] UNION SELECT 2 UNION SELECT 3)
SELECT RAND() FROM [tbl];
GO

-- Results:
-- 0.916865371478081
-- 0.916865371478081
-- 0.916865371478081

-- Solution:

CREATE VIEW [dbo].[RandomizerView]
AS
SELECT RAND() AS [RandomNumber];
GO

CREATE FUNCTION [dbo].[Randomizer]()
RETURNS FLOAT
AS
BEGIN
    RETURN (SELECT [RandomNumber] FROM [dbo].[RandomizerView]);
END;
GO

;WITH [tbl] AS (SELECT 1 [a] UNION SELECT 2 UNION SELECT 3)
SELECT [dbo].[Randomizer]() FROM [tbl];
GO

-- Results:
-- 0.231503002136247
-- 0.39685070525331
-- 0.513351106107159
```
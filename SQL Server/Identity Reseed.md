```sql
CREATE TABLE [dbo].[tbl] ([i] INT IDENTITY);
GO

TRUNCATE TABLE [dbo].[tbl];

-- i=1, i=2
INSERT INTO [dbo].[tbl] DEFAULT VALUES;
INSERT INTO [dbo].[tbl] DEFAULT VALUES;

DECLARE @NewSeed INT;
SELECT @NewSeed = IDENT_CURRENT('[dbo].[tbl]');

SET IDENTITY_INSERT [dbo].[tbl] ON;

-- i=999999999
INSERT [tbl] ([i]) VALUES (999999999);

SET IDENTITY_INSERT [dbo].[tbl] OFF;

DBCC CHECKIDENT ('[dbo].[tbl]', RESEED, @NewSeed);

-- i=3, i=4
INSERT INTO [dbo].[tbl] DEFAULT VALUES;
INSERT INTO [dbo].[tbl] DEFAULT VALUES;

-- 1,2,3,4,999999999
SELECT * FROM [dbo].[tbl];
```
```sql
USE [master];
GO
IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = 'SomeDatabase')
BEGIN
    ALTER DATABASE SomeDatabase SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SomeDatabase;
END;
GO

```
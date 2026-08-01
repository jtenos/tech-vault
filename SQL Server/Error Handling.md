```sql
-- https://www.sommarskog.se/error_handling/Part1.html

CREATE OR ALTER PROCEDURE [dbo].[HandleError]
AS
BEGIN
    DECLARE @ErrorMessage NVARCHAR(2048) = ERROR_MESSAGE();
    DECLARE @Severity TINYINT = ERROR_SEVERITY();
    DECLARE @State TINYINT = ERROR_STATE();
    DECLARE @ErrorNumber INT = ERROR_NUMBER();
    DECLARE @Procedure SYSNAME = ERROR_PROCEDURE();
    DECLARE @LineNumber INT = ERROR_LINE();

    IF @ErrorMessage NOT LIKE '***%'
    BEGIN
        SELECT @ErrorMessage = '*** '
            + COALESCE(QUOTENAME(@Procedure), '<dynamic SQL>')
            + ', Line ' + LTRIM(STR(@LineNumber))
            + '. Error Number ' + LTRIM(STR(@ErrorNumber))
            + ': ' + @ErrorMessage;
    END;
    RAISERROR('%s', @Severity, @State, @ErrorMessage);
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[DoSomething]
AS
BEGIN
    SET XACT_ABORT, NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        /* Do Something Here */
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        EXEC [dbo].[HandleError];
        RETURN 55555;
    END CATCH;
END;
GO
```
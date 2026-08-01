```sql
DECLARE @ID INT;

;WITH [first_in_queue] AS (
    SELECT TOP 1 ID
    FROM [dbo].[Records]
    WHERE [Status] = 'NEW'
    ORDER BY [CreateDate]
)
UPDATE [r]
SET [r].[Status] = 'RUN', @ID = [r].[ID]
FROM [dbo].[Records] AS [r]
JOIN [first_in_queue] ON [r].[ID] = [first_in_queue].[ID];

SELECT @ID; -- The ID that was dequeued

---------------------------------------------------------------

CREATE PROCEDURE dbo.DequeueWorker
(@WorkerQueueID INT OUTPUT)
AS
BEGIN
    DECLARE @result TABLE
    (
        WorkerQueueID INT
    );

    ;WITH first_record
    AS (SELECT WorkerQueueID, StatusCode
        FROM dbo.WorkerQueue WITH (ROWLOCK, READPAST, UPDLOCK)
        WHERE StatusCode = 'NEW'
        ORDER BY WorkerQueueID
        OFFSET 0 ROWS
        FETCH NEXT 1 ROW ONLY)
    UPDATE first_record
    SET StatusCode = 'RUN'
    OUTPUT inserted.WorkerQueueID
    INTO @result
    FROM first_record WITH (ROWLOCK, READPAST, UPDLOCK);

    SELECT @WorkerQueueID = WorkerQueueID
    FROM @result;
END;
GO
```
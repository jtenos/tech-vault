```sql
IF object_id('dbo.tbl') IS NOT NULL
DROP TABLE dbo.tbl;
GO

CREATE TABLE dbo.tbl (
    id INT
    ,name VARCHAR(100)
);
INSERT dbo.tbl
VALUES (1 ,'John'), (2 ,'Jane');

-- Element is the table name, and columns become attributes
SELECT *
FROM dbo.tbl
FOR XML AUTO;
/*
<dbo.tbl id="1" name="John" />
<dbo.tbl id="2" name="Jane" />
*/

-- Each element gets named "row"
SELECT *
FROM dbo.tbl
FOR XML RAW;
/*
<row id="1" name="John" />
<row id="2" name="Jane" />
*/

-- Define the root name and row name, then define the XML structure by the query column names
SELECT
    id [@the-id]
    ,[name] [the-name]
FROM dbo.tbl
FOR XML PATH ('the-item'), ROOT ('the-root');
/*
<the-root>
    <the-item the-id="1">
        <the-name>John</the-name>
    </the-item>
    <the-item the-id="2">
        <the-name>Jane</the-name>
    </the-item>
</the-root>
*/

GO

IF OBJECT_ID('dbo.tbl') IS NOT NULL
    DROP TABLE dbo.tbl;
GO
```
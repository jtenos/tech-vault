# Line
```sql
/*
A series of lines
*/
DECLARE @SRID INT = 0;

DECLARE @Line GEOMETRY = GEOMETRY::STGeomFromText('LINESTRING (1 2, 3 4, 7 6, 7 2, 1 2)', @SRID);
SELECT @Line;
```

# Point
```sql
/*
Create two points
*/
DECLARE @SRID INT = 0; -- Zero for no SRID, just a regular coordinate plane

DECLARE @g1 GEOMETRY = GEOMETRY::STGeomFromText('POINT (3 4)', @SRID);
DECLARE @g2 GEOMETRY = GEOMETRY::STGeomFromText('POINT (4 5)', @SRID);

DECLARE @Distance FLOAT = @g1.STDistance(@g2);

SELECT @Distance; -- square root of 2
```

# STArea
```sql
/*
Area of a polygon
*/
;WITH q AS (
	SELECT [Name], [geometry].STArea() [AreaInSquareMeters]
	FROM [dbo].[states]
)
SELECT [Name], FORMAT([AreaInSquareMeters], '#,##0')
FROM q
ORDER BY [AreaInSquareMeters] DESC;
```

# STBoundary
```sql
/*
Boundary of a polygon
*/
SELECT [geometry].STBoundary()
FROM [dbo].[states]```
```

# STBuffer
```sql
/*
Show a buffer around a polygon
*/

DECLARE @AZ GEOMETRY;
SELECT @AZ = [geometry]
FROM [dbo].[states]
WHERE [Name] = 'Arizona';

DECLARE @AZBuffer GEOMETRY;
SELECT @AZBuffer = @AZ.STBuffer(50000)

SELECT @AZ
UNION ALL SELECT @AZBuffer;
```

# STCentroid
```sql
```
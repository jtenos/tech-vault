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
FROM [dbo].[states]
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
/*
Find the centroid of a polygon
*/
SELECT [geometry].STCentroid().STBuffer(20000)
FROM [dbo].[states]
UNION ALL
SELECT [geometry].STBoundary()
FROM [dbo].[states]
```

# STContains
```sql
/*
Find all states that are fully contained within
the buffer of New York
*/

DECLARE @NYBuffer GEOMETRY;
SELECT @NYBuffer = [geometry].STBuffer(200000)
FROM [dbo].[states]
WHERE [Name] = 'New York';

SELECT @NYBuffer
UNION ALL
SELECT [geometry] FROM [dbo].[states]
WHERE @NYBuffer.STContains([geometry]) = 1;
```

# STDistance
```sql
/*
Find the parcels that are within 30 meters of a flood zone
*/

;WITH flood_zones AS (
	SELECT [Geometry]
	FROM [dbo].[firm]
	WHERE [ZONE] = 'AE'
), distance_from_flood_zone AS (
	SELECT
		p.[ACCTID]
		,p.[geometry].STDistance(flood_zones.[Geometry]) [Distance]
	FROM [dbo].[parcels] p
	JOIN flood_zones ON p.[geometry].STDistance(flood_zones.[Geometry]) < 30
)
SELECT DISTINCT [ACCTID] FROM distance_from_flood_zone
ORDER BY ACCTID;
```

# STIntersection
```sql
/*
Find the pieces of the parcels that are in a flood zone
*/
SELECT p.[Geometry].STIntersection(f.[Geometry])
FROM [dbo].[parcels] p
JOIN [dbo].[firm] f
	ON p.[Geometry].STIntersects(f.[Geometry]) = 1
WHERE f.[ZONE] LIKE 'A%';
```

# STIntersects
```sql
/*
Find all parcels that intersect a flood zone
*/
SELECT p.[Geometry]
FROM [dbo].[parcels] p
JOIN [dbo].[firm] f
	ON p.[Geometry].STIntersects(f.[Geometry]) = 1
WHERE f.[ZONE] LIKE 'A%';
```

# STTouches
```sql
/*
Find all states that touch New York
*/
;WITH ny AS (
	SELECT [geometry]
	FROM [dbo].[states]
	WHERE [Name] = 'New York'
)
SELECT s.*
FROM [dbo].[states] s
JOIN ny ON s.[geometry].STTouches(ny.[geometry]) = 1
```
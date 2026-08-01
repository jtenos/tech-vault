```sql
IF OBJECT_ID('tempdb..#worldcities') IS NOT NULL
	DROP TABLE #worldcities;

CREATE TABLE #worldcities (
	[city] NVARCHAR(200)
	,[city_ascii] NVARCHAR(200)
	,[lat] FLOAT
	,[lng] FLOAT
	,[country] NVARCHAR(200)
	,[iso2] NVARCHAR(2)
	,[iso3] NVARCHAR(3)
	,[admin_name] NVARCHAR(200)
	,[capital] NVARCHAR(200)
	,[population] BIGINT
	,[id] BIGINT NOT NULL PRIMARY KEY

	,INDEX [IX_city_country] ([city], [country])
	,INDEX [IX_population_DESC] ([population] DESC)
);

-- https://simplemaps.com/data/world-cities
-- Convert to Unicode tsv with line breaks as LF
BULK INSERT #worldcities
FROM 'C:\Users\jtenos\Documents\GitHub\SQLServerSpatialSamples\worldcities.tsv'
WITH (
	FIRSTROW = 2
	,FIELDTERMINATOR = '	'
	,ROWTERMINATOR = '0x0a'
	,TABLOCK
);

ALTER TABLE #worldcities ADD [GEOG] GEOGRAPHY, [GEOGBuff] AS [GEOG].STBuffer(30000);

DECLARE @SRID INT = 4326; -- Standard lat/long

UPDATE #worldcities SET [GEOG] = GEOGRAPHY::STPointFromText(N'POINT('
			+ CAST([lng] AS NVARCHAR(50)) + N' ' + CAST([lat] AS NVARCHAR(50))
			+ N')', @SRID);

DECLARE @PhoenixID BIGINT, @TianjinID BIGINT;
DECLARE @PhoenixGeog GEOGRAPHY, @TianjinGeog GEOGRAPHY;
DECLARE @PhoenixLat FLOAT, @PhoenixLong FLOAT, @TianjinLat FLOAT, @TianjinLong FLOAT;

SELECT
	@PhoenixID = [id]
	,@PhoenixGeog = [GEOG]
	,@PhoenixLat = [lat]
	,@PhoenixLong = [lng]
FROM #worldcities
WHERE [city] = 'Phoenix'
AND [country] = 'United States';

SELECT
	@TianjinID = [id]
	,@TianjinGeog = [GEOG]
	,@TianjinLat = [lat]
	,@TianjinLong = [lng]
FROM #worldcities
WHERE [city] = 'Tianjin'
AND [country] = 'China';

DECLARE @MetersPerMile FLOAT = 1609.34;

SELECT FORMAT(@PhoenixGeog.STDistance(@TianjinGeog) / @MetersPerMile, '#,##0.0') [Miles from Phoenix to Tianjin];

DECLARE @PhxToTianjinLine GEOGRAPHY = GEOGRAPHY::STGeomFromText('LINESTRING ('
	+ CAST(@TianjinLong AS VARCHAR(10))
	+ ' '
	+ CAST(@TianjinLat AS VARCHAR(10))
	+ ', '
	+ CAST(@PhoenixLong AS VARCHAR(10))
	+ ' '
	+ CAST(@PhoenixLat AS VARCHAR(10))
	+ ')'
, @SRID);

SELECT [GEOGBuff] FROM (
	SELECT TOP 3000 [GEOGBuff]
	FROM #worldcities
	ORDER BY CASE [id] 
		WHEN @PhoenixID THEN 1 
		WHEN @TianjinID THEN 1 
	END, [population] DESC
) x
UNION ALL
SELECT @PhxToTianjinLine;
```
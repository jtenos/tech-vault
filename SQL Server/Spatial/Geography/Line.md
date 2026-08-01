The built-in spatial reference systems (SRID). 

```sql
-- SELECT * FROM sys.spatial_reference_systems

DECLARE @SRID INT = 4326; -- Standard lat/long

--from Milwaukee, WI to Tulsa, OK
DECLARE @Line GEOMETRY;
SELECT @Line = GEOMETRY::STGeomFromText('LINESTRING (-87.906471 43.038902, -95.992775 36.153980)', @SRID)

DECLARE @LineGeography GEOGRAPHY = GEOGRAPHY::STGeomFromText(@Line.STAsText(), @SRID)

SELECT FORMAT(@LineGeography.STLength() / 1000, '#,##0') + ' km';

```
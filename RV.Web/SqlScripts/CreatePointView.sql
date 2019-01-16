CREATE VIEW Point
AS
SELECT
  id as Id,
  ST_X(the_geom) as Longitude,
  ST_Y(the_geom) as Latitude
FROM roads_vertices_pgr;
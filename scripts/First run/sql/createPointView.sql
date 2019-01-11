CREATE VIEW Point
AS
SELECT
  id as Id,
  ST_X(the_geom) as Latitude,
  ST_Y(the_geom) as Longitude
FROM roads_vertices_pgr;


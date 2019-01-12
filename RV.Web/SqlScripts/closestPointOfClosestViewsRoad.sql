
CREATE TEMP TABLE temp_roads(road_id int, source_geom geometry, target_geom geometry, source int, target int, distance_to_road float, distance_to_source float, distance_to_target float);

INSERT INTO temp_roads
  (
    SELECT
       r.id as road_id,
       p.the_geom as source_geom,
       p2.the_geom as target_geom,
       source,
       target,
       ST_DISTANCE(
       ST_SetSRID(ST_MakePoint(20.983455, 52.231909), 4326),
       geom,
         true
         ) as distance_to_road,
       ST_DISTANCE(
       ST_SetSRID(ST_MakePoint(20.983455, 52.231909), 4326),
       p.the_geom,
         true
         ) as distance_to_source,
       ST_DISTANCE(
       ST_SetSRID(ST_MakePoint(20.983455, 52.231909), 4326),
       p2.the_geom,
         true
         ) as distance_to_target
from roads r
join roads_vertices_pgr p ON r.source = p.id
join roads_vertices_pgr p2 ON r.target = p2.id
where isview = true
order by distance_to_road limit 1);

select *
from temp_roads;

WITH next_point as
       (
  select
  CASE
    WHEN (distance_to_target > distance_to_source)
      THEN source
    ELSE target
  END as point_id,
  CASE
    WHEN (distance_to_target > distance_to_source)
      THEN source_geom
    ELSE target_geom
  END as point_geom
from temp_roads
         )

SELECT p.id, p.longitude, p.latitude, pt.cost
FROM pgr_aStar(
'SELECT
  r.id,
  source,
  target,
  st_length(geom, true) as cost,
  src.latitude as x1,
  src.longitude as y1,
  trg.latitude as x2,
  trg.longitude as y2
FROM roads r
JOIN point src ON src.id = source
JOIN point trg ON trg.id = target',
(SELECT source FROM roads
ORDER BY ST_Distance(
      ST_StartPoint(geom),
  ST_SetSRID(ST_MakePoint(20.983455, 52.231909), 4326),
  true
) ASC
LIMIT 1),
(SELECT source FROM roads
      ORDER BY ST_Distance(
      ST_StartPoint(geom),
  ST_SetSRID((select point_geom from next_point), 4326),
  true
      ) ASC
  LIMIT 1),
  false,
  false) as pt
JOIN point p ON p.id = pt.id1;

drop table temp_roads;
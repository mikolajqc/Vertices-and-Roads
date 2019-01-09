SELECT p.id, p.latitude, p.longitude
FROM pgr_dijkstra(
   'SELECT id, source, target, st_length(geom, true) as cost FROM roads',
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
        ST_SetSRID(ST_MakePoint(20.973400, 52.169647), 4326),
        true
   ) ASC
   LIMIT 1)
) as pt
JOIN point p ON p.id = pt.node;
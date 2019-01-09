SELECT p.id, p.longitude, p.latitude
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
  ST_SetSRID(ST_MakePoint(20.973400, 52.169647), 4326),
  true
      ) ASC
  LIMIT 1),
  false,
  false) as pt
JOIN point p ON p.id = pt.id1;
DROP TABLE IF EXISTS current_starting_point;
DROP TABLE IF EXISTS target_point;
DROP TABLE IF EXISTS closest_roads_to_source_target_line;
DROP TABLE IF EXISTS closest_roads_to_source_target_line;
DROP TABLE IF EXISTS result_path_in_points;

CREATE TEMP TABLE current_starting_point(longitude float, latitude float);
INSERT INTO current_starting_point VALUES (20.983455, 52.231909); -- to jest punkt startowy, wrzucic tutaj zmienne source node'a
CREATE TEMP TABLE target_point(longitude float, latitude float);
INSERT INTO target_point VALUES (20.973400, 52.169647);

DROP TABLE IF EXISTS nearest_point_to_source_and_target;
CREATE TEMP Table nearest_point_to_source_and_target(source_id int, target_id int);
INSERT INTO nearest_point_to_source_and_target VALUES
((SELECT source as source_id  FROM roads
    ORDER BY ST_Distance(
          ST_StartPoint(geom),
      ST_SetSRID(ST_MakePoint(
                  (select longitude from current_starting_point),
                  (select latitude from current_starting_point)), 4326),
      true
    ) ASC
    LIMIT 1),
(SELECT source as target_id FROM roads
    ORDER BY ST_Distance(
          ST_StartPoint(geom),
      ST_SetSRID(ST_MakePoint(
                  (select longitude from target_point),
                  (select latitude from target_point)), 4326),
      true
    ) ASC
    LIMIT 1));

DROP TABLE IF EXISTS nearest_point_to_source_and_target_geom;
CREATE TEMP Table nearest_point_to_source_and_target_geom(source_geom geometry, target_geom geometry);
INSERT INTO nearest_point_to_source_and_target_geom VALUES
(
  (SELECT the_geom as source_geom from roads_vertices_pgr rv JOIN nearest_point_to_source_and_target n ON rv.id = n.source_id),
  (SELECT the_geom as target_geom from roads_vertices_pgr rv JOIN nearest_point_to_source_and_target n ON rv.id = n.target_id)
);


--select *
--from nearest_point_to_source_and_target_geom;


CREATE TEMP TABLE closest_roads_to_source_target_line as
  (select t.*
from (select t.*, st_length(geom, true), sum(st_length(geom, true)) over (order by st_distance
  (st_makeline(
      (select source_geom from nearest_point_to_source_and_target_geom),
      (select target_geom from nearest_point_to_source_and_target_geom)
    ), -- source and target - przekatna
   geom,
   true
  ), id) as running_amount
      from roads t
      where t.isView = true
     ) t
where running_amount - st_length(geom, true) < 1000 -- R - REQUIREMENT
order by st_distance
           (st_makeline(
                (select source_geom from nearest_point_to_source_and_target_geom),
                (select target_geom from nearest_point_to_source_and_target_geom)
              ), -- source and target - przekatna
             geom,
             true
           ));

select *
from closest_roads_to_source_target_line;

-- to musi byc w repeat
-- zwraca liste punktow od danego punktu do najblizszego punktu w najblizszej widokowej sciezce

CREATE TEMP TABLE result_path_in_points(id int, longitude float, latitude float, ord SERIAL);

DO
$do$
  BEGIN
   FOR i IN 1..(select count(id) from closest_roads_to_source_target_line) LOOP

    DROP TABLE IF EXISTS current_result_path_in_points;

    CREATE TEMP TABLE nearest_road_to_point(road_id int, source_geom geometry, target_geom geometry, source int, target int, distance_to_road float, distance_to_source float, distance_to_target float);

    INSERT INTO nearest_road_to_point
      (
        SELECT
           r.id as road_id,
           p.the_geom as source_geom,
           p2.the_geom as target_geom,
           source,
           target,
           ST_DISTANCE(
           ST_SetSRID((select source_geom from nearest_point_to_source_and_target_geom), 4326),
           geom,
             true
             ) as distance_to_road,
           ST_DISTANCE(
           ST_SetSRID((select source_geom from nearest_point_to_source_and_target_geom), 4326),
           p.the_geom,
             true
             ) as distance_to_source,
           ST_DISTANCE(
           ST_SetSRID((select source_geom from nearest_point_to_source_and_target_geom), 4326),
           p2.the_geom,
             true
             ) as distance_to_target
    from closest_roads_to_source_target_line r
    join roads_vertices_pgr p ON r.source = p.id
    join roads_vertices_pgr p2 ON r.target = p2.id
    order by distance_to_road limit 1);

    delete from closest_roads_to_source_target_line
    where id = (select road_id from nearest_road_to_point);

    --select *
    --from nearest_road_to_point;


    drop table if exists next_point;

    CREATE TEMP TABLE next_point as
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
      END as point_geom,
      CASE
        WHEN (distance_to_target > distance_to_source)
          THEN target
        ELSE source
      END as point_next_id,
      CASE
        WHEN (distance_to_target > distance_to_source)
          THEN source_geom
        ELSE target_geom
      END as point_next_geom
    from nearest_road_to_point
             );

    drop table nearest_road_to_point;

    CREATE TEMP TABLE current_result_path_in_points(id int PRIMARY KEY, longitude float, latitude float, ord SERIAL);

    INSERT INTO current_result_path_in_points ( select p.id, p.longitude, p.latitude
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
      ST_SetSRID((select source_geom from nearest_point_to_source_and_target_geom), 4326),
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
    JOIN point p ON p.id = pt.id1);


    INSERT INTO current_result_path_in_points (
      select p.id as id, longitude, latitude
      from next_point np
      JOIN Point p ON np.point_next_id = p.id)
    ON CONFLICT DO NOTHING;



    INSERT INTO result_path_in_points
    (select id, longitude, latitude, ord from current_result_path_in_points);

    --DELETE FROM current_starting_point;

   -- INSERT INTO current_starting_point
   --   (
   --     select longitude, latitude
  --      from current_result_path_in_points
  --      order by ord desc
   --     limit 1
  --    );

      UPDATE nearest_point_to_source_and_target_geom
      SET source_geom = (SELECT the_geom as source_geom from roads_vertices_pgr rv where id IN (select id
        from current_result_path_in_points
        order by ord desc
        limit 1));

   END LOOP;
  END
$do$;

--select * from test_point;
--drop table test_point;

INSERT INTO result_path_in_points ( select p.id, p.longitude, p.latitude
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
      ST_SetSRID((select source_geom from nearest_point_to_source_and_target_geom), 4326),
      true
    ) ASC
    LIMIT 1),
    (SELECT source FROM roads
          ORDER BY ST_Distance(
          ST_StartPoint(geom),
      ST_SetSRID((select target_geom from nearest_point_to_source_and_target_geom), 4326),
      true
          ) ASC
      LIMIT 1),
      false,
      false) as pt
    JOIN point p ON p.id = pt.id1);


select id, longitude, latitude from result_path_in_points;

select t.*
from (select t.*, st_length(geom, true), sum(st_length(geom, true)) over (order by st_distance
  (st_makeline(ST_MakePoint(20.983455, 52.231909),ST_MakePoint(20.973400, 52.169647)), -- source and target - przekatna
   geom,
   true
  ), id) as running_amount
      from roads t
      where t.isView = true
     ) t
where running_amount - st_length(geom, true) < 10000
order by st_distance
           (st_makeline(ST_MakePoint(20.983455, 52.231909),ST_MakePoint(20.973400, 52.169647)), -- source and target - przekatna
            geom,
            true
           );
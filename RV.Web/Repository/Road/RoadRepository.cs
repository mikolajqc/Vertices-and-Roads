using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using RV.Web.Configuration;
using RV.Web.Configuration.Extensions;

namespace RV.Web.Repository.Road
{
    public class RoadRepository : IRoadRepository
    {
        private readonly PostgresConfiguration _postgresConfiguration;

        public RoadRepository(IOptions<PostgresConfiguration> postgresConfiguration)
        {
            _postgresConfiguration = postgresConfiguration.Value;
        }

        public Model.Entities.Road FindById(int id)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_postgresConfiguration.ToConnectionString()))
            {
                dbConnection.Open();
                return dbConnection.Query<Model.Entities.Road>(@"SELECT * FROM RoadView WHERE id = @Id",
                    new {Id = id}).FirstOrDefault();
            }
        }

        public IEnumerable<Model.Entities.Road> GetViewRoads(Model.Entities.Point source, Model.Entities.Point target, int minimalLengthOfViewRoads)
        {
            const string query = @"
-----
DROP TABLE IF EXISTS current_starting_point;
DROP TABLE IF EXISTS target_point;
DROP TABLE IF EXISTS closest_roads_to_source_target_line;
DROP TABLE IF EXISTS closest_roads_to_source_target_line;
DROP TABLE IF EXISTS result_path_in_points;

CREATE TEMP TABLE current_starting_point(longitude float, latitude float);
INSERT INTO current_starting_point VALUES (@sourceLongitude, @sourceLatitude); -- to jest punkt startowy, wrzucic tutaj zmienne source node'a
CREATE TEMP TABLE target_point(longitude float, latitude float);
INSERT INTO target_point VALUES (@targetLongitude, @targetLatitude);

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
where running_amount - st_length(geom, true) < @minimalLengthOfViewRoads -- R - REQUIREMENT
order by st_distance
           (st_makeline(
                (select source_geom from nearest_point_to_source_and_target_geom),
                (select target_geom from nearest_point_to_source_and_target_geom)
              ), -- source and target - przekatna
             geom,
             true
           ));

select p.Latitude as sourceLatitude, p.Longitude as sourceLongitude, p2.Latitude as targetLatitude, p2.Longitude as targetLongitude
from closest_roads_to_source_target_line c
  JOIN Point p ON c.source = p.id
  JOIN Point p2 ON c.target = p2.id;";
            using (IDbConnection dbConnection = new NpgsqlConnection(_postgresConfiguration.ToConnectionString()))
            {
                dbConnection.Open();
                return dbConnection.Query<Model.Entities.Road>(
                    query,
                    new
                    {
                        sourceLatitude = source.Latitude,
                        sourceLongitude = source.Longitude,
                        targetLatitude = target.Latitude,
                        targetLongitude = target.Longitude,
                        minimalLengthOfViewRoads = minimalLengthOfViewRoads
                    });
            }
            
        }
    }
}
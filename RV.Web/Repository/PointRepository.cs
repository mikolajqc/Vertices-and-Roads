using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using RV.Web.Configuration;
using Npgsql;
using RV.Model.Entities;
using RV.Web.Configuration.Extensions;

namespace RV.Web.Repository
{
    public class PointRepository : IPointRepository
    {
        private readonly PostgresConfiguration _postgresConfiguration;

        public PointRepository(IOptions<PostgresConfiguration> postgresConfiguration)
        {
            _postgresConfiguration = postgresConfiguration.Value;
        }
        
        internal IDbConnection Connection => new NpgsqlConnection();

        public Point FindById(int id)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_postgresConfiguration.ToConnectionString()))
            {
                dbConnection.Open();
                return dbConnection.Query<Point>(@"SELECT * FROM Point WHERE id = @Id",
                    new {Id = id}).FirstOrDefault();
            }
        }

        public IEnumerable<Point> GetPointsOnShortestPathUsingDijkstra(Point source, Point target)
        {
            const string query = @"
SELECT p.id, p.latitude, p.longitude
FROM pgr_dijkstra(
    'SELECT id, source, target, st_length(geom, true) as cost FROM roads',
    (SELECT source FROM roads
    ORDER BY ST_Distance(
    ST_StartPoint(geom),
ST_SetSRID(ST_MakePoint(@sourceLatitude, @sourceLongitude), 4326),
true
    ) ASC
    LIMIT 1),
(SELECT source FROM roads
    ORDER BY ST_Distance(
    ST_StartPoint(geom),
ST_SetSRID(ST_MakePoint(@targetLatitude, @targetLongitude), 4326),
true
    ) ASC
    LIMIT 1)
    ) as pt
    JOIN point p ON p.id = pt.node;";

            using (IDbConnection dbConnection = new NpgsqlConnection(_postgresConfiguration.ToConnectionString()))
            {
                dbConnection.Open();
                return dbConnection.Query<Point>(
                    query,
                    new
                    {
                        sourceLatitude = source.Latitude,
                        sourceLongitude = source.Longitude,
                        targetLatitude = target.Latitude,
                        targetLongitude = target.Longitude
                    });
            }
            
        }
    }
}
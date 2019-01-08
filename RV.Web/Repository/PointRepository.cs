using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using RV.Model;
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
                return dbConnection.Query<Point>("SELECT * FROM Point WHERE id = @Id",
                    new {Id = id}).FirstOrDefault();
            }
        }
    }
}
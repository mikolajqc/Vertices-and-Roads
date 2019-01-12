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
    }
}
namespace RV.Web.Configuration.Extensions
{
    public static class ConfigurationExtension
    {
        public static string ToConnectionString(this PostgresConfiguration postgresConfiguration)
        {
            return
                $@"User ID={postgresConfiguration.UserId};
Password={postgresConfiguration.Password};
Host={postgresConfiguration.Host};
Port={postgresConfiguration.Port};
Database={postgresConfiguration.Database};
Pooling={postgresConfiguration.Pooling};";
        }
    }
}
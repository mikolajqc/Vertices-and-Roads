namespace RV.Web.Configuration
{
    public class PostgresConfiguration
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public bool Pooling { get; set; }
    }
}
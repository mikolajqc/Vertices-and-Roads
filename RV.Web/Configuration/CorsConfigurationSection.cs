namespace RV.Web.Configuration
{
    public class CorsConfigurationSection
    {
        public string[] Origins { get; set; }
        public string[] Methods { get; set; }
    }
}
namespace webapi_bilheteria_c.Domain.Models
{
    public class ConfigurationKeys
    {
        public ConnectionStrings? ConnectionStrings { get; set; }

        public List<Parameters>? Parameters {get; set;}
    }
}
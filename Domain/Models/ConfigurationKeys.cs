using System.Security.Cryptography.X509Certificates;

namespace webapi_bilheteria_c.Domain.Models
{
    public class ConfigurationKeys
    {
        public ConnectionStrings? ConnectionStrings { get; set; }
        public List<Parameters>? Parameters {get; set;}
        public List<Credentials>? Credentials {get; set;}
        public X509Certificate2? CertificateGerenciaNet { get; set; }        
    }
}
using System.Net;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using Newtonsoft.Json;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Client
{
    public class GerenciaNetClient : IPixClient
    {
        private readonly ConfigurationKeys _configurationKeys;
        private readonly IAuthorizationRepository _authorizationRepository;

        public GerenciaNetClient(ConfigurationKeys configurationKeys, IAuthorizationRepository authorizationRepository){
            _configurationKeys = configurationKeys;                        
            _authorizationRepository = authorizationRepository;
        }

        public async Task GenerateToken(){
            
            var httpClient = InitHttp();            
            
            var json = JsonConvert.SerializeObject(new {grant_type = "client_credentials"});
            var bodyJson = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            var authenticationString = $@"{_configurationKeys.Credentials?.FirstOrDefault(s => s.Origin == "GerenciaNet").Value1}
                :{_configurationKeys.Credentials.FirstOrDefault(s => s.Origin == "GerenciaNet").Value2}";
                            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString)));

            var response = await httpClient.PostAsync("https://api-pix-h.gerencianet.com.br/oauth/token", bodyJson);      
            
            if (response.IsSuccessStatusCode == true){
                var gerenciaNetTokenResponse = JsonConvert.DeserializeObject<GerenciaNetTokenResponse>(
                        response.Content.ToString());

                await _authorizationRepository.UpdateJWT(gerenciaNetTokenResponse.AccessToken, 
                    DateTime.Now.AddSeconds(gerenciaNetTokenResponse.ExpiresIn), "GerenciaNet");
            }
            else
            {
                throw new Exception($@"Error on receive token. Status Code: {response.StatusCode}");
            }            

        }

        private HttpClient InitHttp(){
            var handler = new HttpClientHandler{
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,                
                CheckCertificateRevocationList = false};
            handler.ClientCertificates.Add(_configurationKeys.CertificateGerenciaNet);
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) 
                    => {return true;};            
            return new HttpClient(handler);            
        }
    }
}
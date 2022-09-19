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

        public async Task<QrCodeResponse?> GenerateCharge(Ticket ticket, string key){
            var bearer = await GenerateToken();
            if (string.IsNullOrEmpty(bearer)) return null;

            var json = JsonConvert.SerializeObject(new ChargeRequest{
                 Valor = new Valor{
                    Original = ticket.Value
                 },
                 Chave = key,
                 Calendario = new Calendario(),
                 Devedor = new Devedor {
                    Cpf = ticket.PayerDocument,
                    Nome = ticket.PayerName
                 },
                 SolicitacaoPagador = $@"Bilhete: {ticket.EventName}"
            });            
            var bodyJson = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            
            var httpClient = InitHttp();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            var response = await httpClient.PostAsync(JoinUri("CONF4"), bodyJson).Result.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<ChargeResponse>(response);
            response = await httpClient.GetAsync(string.Format(JoinUri("CONF5"), 
                Convert.ToString(responseJson?.Loc?.Id))).Result.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<QrCodeResponse>(response);            
        }         

        private async Task<string?> GenerateToken(){
            var bearer = _authorizationRepository.SearchJWT("GerenciaNet").Result;

            if (!string.IsNullOrEmpty(bearer))
                return bearer;

            var httpClient = InitHttp();    

            var json = JsonConvert.SerializeObject(new {grant_type = "client_credentials"});
            var bodyJson = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var clientId = _configurationKeys.Credentials?.FirstOrDefault(s => s.Origin == "GerenciaNet").Value1.Trim();
            var clientSecret = _configurationKeys.Credentials.FirstOrDefault(s => s.Origin == "GerenciaNet").Value2.Trim();                      
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
                Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));

            var response = await httpClient.PostAsync(JoinUri("CONF3"), bodyJson);            

            if (response.IsSuccessStatusCode == true)
            {                
                var gerenciaNetResponseToken = JsonConvert.DeserializeObject<GerenciaNetTokenResponse>(
                    response.Content.ReadAsStringAsync().Result);
                
                await _authorizationRepository.UpdateJWT(gerenciaNetResponseToken.AccessToken,
                    DateTime.Now.AddSeconds(gerenciaNetResponseToken.ExpiresIn), "GerenciaNet");

                return gerenciaNetResponseToken.AccessToken;
            }            
            return null;
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

        private string JoinUri(string? code){
            return _configurationKeys.Parameters.FirstOrDefault(s => s.Code == "CONF2").Value +
                _configurationKeys.Parameters.FirstOrDefault(s => s.Code == code).Value;
        }
    }
}
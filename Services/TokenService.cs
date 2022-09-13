using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Services
{
    public class TokenService : ITokenService
    {
        private readonly ConfigurationKeys _configurationKeys;

        public TokenService(ConfigurationKeys configurationKeys){
            _configurationKeys = configurationKeys;
        }

        public string? GetTokenKey(string? token, string key, bool required = true)
        {
            var jwtInput = token.Replace("Bearer", string.Empty).Trim();
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(jwtInput);
            var tokenKey = jwtToken.Claims.FirstOrDefault(i => i.Type == key)?.Value;

            if (required && string.IsNullOrWhiteSpace(tokenKey))
                throw new UnauthorizedAccessException("Sessão de usuário ínvalida");

            return tokenKey;
        }

        public TokenResponse GenerateToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configurationKeys.Parameters.FirstOrDefault(f => f.Code == "CONF1").Value);
            var expires = DateTime.UtcNow.AddHours(4);
            var tokenDescriptor = new SecurityTokenDescriptor           
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("Id", user.Uid)
                }), 
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenResponse {Token = tokenHandler.WriteToken(token), DataExpiracao = expires };
        }
    }

}
namespace webapi_bilheteria_c.Domain.Models
{
    public class TokenResponse
    {
        public DateTime DataExpiracao { get; set; }
        public string? Token { get; set; }
    }
}
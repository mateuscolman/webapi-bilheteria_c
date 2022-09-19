namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IAuthorizationRepository
    {
        Task UpdateJWT(string? jwt, DateTime expiresIn, string? origin);
        Task<string> SearchJWT(string? origin);
    }
}
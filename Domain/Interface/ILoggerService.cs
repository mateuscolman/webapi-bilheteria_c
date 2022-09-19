namespace webapi_bilheteria_c.Domain.Interface
{
    public interface ILoggerService
    {
        Task LogInformation(string request);
        Task LogWarning(string request);
        Task LogError(string request);
    }
}
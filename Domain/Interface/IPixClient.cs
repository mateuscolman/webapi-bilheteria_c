using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IPixClient
    {
        Task<QrCodeResponse?> GenerateCharge(Ticket ticket, string key);
    }
}
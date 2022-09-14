using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IEventsRepository
    {
        Task<string> InsertEvent(Events events); 
        Task InsertEventTime(DateTime date, string? eventUid);
    }
}
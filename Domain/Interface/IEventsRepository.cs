using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IEventsRepository
    {
        Task<string> InsertEvent(Events events); 
        Task InsertEventTime(DateTime date, string? eventUid);
        Task<List<Events>> GetEventsOnDisplayByCompany(string? companyUid);
        Task<List<Events>> GetEventsOnDisplay();
        Task<List<EventsTime>> GetEventsTime(string? eventUid);
        Task<Events> GetEventByUid(string? uid);
    }
}
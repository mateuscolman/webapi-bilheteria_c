using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IEventsService
    {
        bool InsertEvent(Events events);
        List<Events> GetEventsOnDisplay();
        List<Events> GetEventsOnDisplayByCompany(string? companyUid);
        Events GetEventsByUid(string? uid);
    }
}
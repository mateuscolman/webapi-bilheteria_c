using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IEventsService
    {
        bool CreateEvent(Events events, int fullValue);
        List<Events> GetEventsOnDisplay();
        List<Events> GetEventsOnDisplayByCompany(string? companyUid);
        Events GetEventsByUid(string? uid);
    }
}
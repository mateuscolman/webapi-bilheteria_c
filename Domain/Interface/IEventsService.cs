using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IEventsService
    {
        bool CreateEvent(Event events, int fullValue, IFormFile img);
        List<EventOnDisplay> GetEventsOnDisplay();
        List<Event> GetEventsOnDisplayByCompany(string? companyUid);
        Event GetEventsByUid(string? uid);
    }
}
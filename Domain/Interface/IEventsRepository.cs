using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IEventsRepository
    {
        Task<string> InsertEvent(Event events);
        Task InsertEventTime(DateTime date, string? eventUid);
        Task<List<Event>> GetEventsOnDisplayByCompany(string? companyUid);
        Task<List<EventOnDisplay>> GetEventsOnDisplay();
        Task<List<ScheduleTime>> GetEventsTime(string? eventUid);
        Task<Event> GetEventByUid(string? uid);
        Task InsertValueEvent(string eventUid, int fullValue);
        Task<int> GetValueEvents(string valueType, string? eventUid);
    }
}
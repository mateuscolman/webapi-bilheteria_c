using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface IEventsService
    {
        bool InsertEvent(Events events);
    }
}
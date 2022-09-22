using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Domain.Interface
{
    public interface ITicketService
    {
        bool InsertTicketOrder(List<TicketRelatedPersons> ticketRelatedPersons,
                    string? eventUid);
    }
}
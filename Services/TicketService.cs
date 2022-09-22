using webapi_bilheteria_c.Domain.Enum;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Services
{
    public class TicketService : ITicketService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IPixClient _pixClient;
        private readonly ICompanyRepository _companyRepository;
        private readonly ITicketRepository _ticketRepository;

        public TicketService(IEventsRepository eventsRepository, IPixClient pixClient,
            ICompanyRepository companyRepository, ITicketRepository ticketRepository)
        {
            _eventsRepository = eventsRepository;
            _pixClient = pixClient;
            _companyRepository = companyRepository;
            _ticketRepository = ticketRepository;
        }

        public bool InsertTicketOrder(List<TicketRelatedPersons> ticketRelatedPersons,
            string? eventUid)
        {
            var halfValue = ticketRelatedPersons.Where(s => s.HalfPrice == 1).Count()
                * _eventsRepository.GetValueEvents("half_value", eventUid).Result;
            var full_value = ticketRelatedPersons.Where(s => s.HalfPrice == 0).Count()
                * _eventsRepository.GetValueEvents("full_value", eventUid).Result;

            var qrCode = _pixClient.GenerateCharge(new Ticket
            {
                EventName = _eventsRepository.GetEventByUid(eventUid).Result.Name,
                Value = Convert.ToString(halfValue + full_value),
                PayerDocument = ticketRelatedPersons.FirstOrDefault(s => s.Responsible == 1).Document,
                PayerName = ticketRelatedPersons.FirstOrDefault(s => s.Responsible == 1).Name,
            }, _companyRepository.GetPaymentMethod(
                    _eventsRepository.GetEventByUid(eventUid).Result.CompanyUid).
                    Result.FirstOrDefault(s => s.Name == "PIX").PaymentKey);

            if (qrCode != null)
            {
                var responsibleUid = _ticketRepository.
                    InsertRelatedPersons(ticketRelatedPersons.FirstOrDefault(s => s.Responsible == 1)).Result;
                foreach (var person in ticketRelatedPersons)
                {
                    if (person.Responsible == 1) continue;
                    person.LinkedUid = responsibleUid;
                    _ticketRepository.InsertRelatedPersons(person);
                }
            }
            return true;
        }

        private void InsertRelatedPersons()
        {

        }
    }
}
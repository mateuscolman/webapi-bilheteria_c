using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Enum;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IDbConnection _dbConnection;
        public TicketRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<string> InsertRelatedPersons(TicketRelatedPersons ticketRelatedPersons)
        {
            ticketRelatedPersons.Uid = Convert.ToString(Guid.NewGuid());
            var command = $@"
                insert into ticket_order
                values(@uid, @eventUid, @name, @document, @linkedUid, @halfPrice, @specialCondition, {(int)PaymentStatusEnum.NotPaid})                
            ";
            await _dbConnection.ExecuteAsync(command, new
            {
                uid = ticketRelatedPersons.Uid,
                eventUid = ticketRelatedPersons.EventUid,
                name = ticketRelatedPersons.Name,
                document = ticketRelatedPersons.Document,
                halPrice = ticketRelatedPersons.HalfPrice,
                specialCondition = ticketRelatedPersons.SpecialCondition
            });
            return ticketRelatedPersons.Uid;
        }
    }
}
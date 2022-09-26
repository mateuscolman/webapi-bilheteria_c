using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class EventsRepository : IEventsRepository
    {
        private readonly IDbConnection _dbConnection;

        public EventsRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Event>> GetEventsOnDisplayByCompany(string? companyUid)
        {
            var command = $@"
                select
                    uid,
                    name,
                    starts_in as StartsIn,
                    ends_in as EndsIn,
                    on_display as OnDisplay,
                    description,
                    cancelled,
                    reason,
                    company_uid as CompanyUid,
                    published_by as PublishedBy
                from events
                where company_uid = @companyUid
                    and on_display = 1
            ";

            var events = await _dbConnection.QueryAsync<Event>(command, new { companyUid });
            return events.ToList();
        }

        public async Task<Event> GetEventByUid(string? uid)
        {
            var command = $@"
                select
                    uid,
                    name,
                    starts_in as StartsIn,
                    ends_in as EndsIn,
                    on_display as OnDisplay,
                    description,
                    cancelled,
                    reason,
                    company_uid as CompanyUid,
                    published_by as PublishedBy
                from events
                where uid = @uid
            ";

            return await _dbConnection.QueryFirstOrDefaultAsync<Event>(command, new { uid });
        }

        public async Task<List<EventOnDisplay>> GetEventsOnDisplay()
        {
            var command = $@"
                select
                    e.uid,
                    e.name,    
                    e.presentation_image Image,
                    c.name companyName,
                    e.x_dimension dimensionX,
                    e.y_dimension dimensionY
                from event e
                inner join company c on c.uid  = e.company_uid 
                where on_display = 1    
            ";

            var events = await _dbConnection.QueryAsync<EventOnDisplay>(command);
            return events.ToList();
        }

        public async Task<string> InsertEvent(Event events)
        {
            if (!ValidInsert(events.Name, events.CompanyUid).Result) throw new Exception("Events already exist");
            var command = $@"
                insert into event
                values(@uid, @name, @startsIn, @endsIn, 1, @description, 0, @reason, @companyUid, @publishedBy, @presentationImage, 2, 2)";

            await _dbConnection.ExecuteAsync(command, new
            {
                uid = events.Uid,
                name = events.Name,
                startsIn = events.StartsIn,
                endsIn = events.EndsIn,
                description = events.Description,
                reason = events.Reason,
                companyUid = events.CompanyUid,
                publishedBy = events.PublishedBy,
                presentationImage = events.PresentationImage
            });

            return events.Uid;
        }

        public async Task InsertEventTime(DateTime date, string? eventUid)
        {
            var command = $@"
                insert into events_time
                values(uuid(), @date, @date, @eventUid)
            ";
            await _dbConnection.ExecuteAsync(command, new { date, eventUid });
        }

        public async Task<List<ScheduleTime>> GetEventsTime(string? eventUid)
        {
            var command = $@"
                select
                    start,
                    end
                from events_time 
                where event_uid = @eventUid";

            var eventsTime = await _dbConnection.QueryAsync<ScheduleTime>(command, new { eventUid });
            return eventsTime.ToList();
        }

        public async Task InsertValueEvent(string eventUid, int fullValue)
        {
            var halfValue = fullValue / 2;
            var command = $@"
                insert into value_events
                values(uuid(), @eventUid, @fullValue, @halfValue)
            ";

            await _dbConnection.ExecuteAsync(command, new { eventUid, fullValue, halfValue });
        }

        public async Task<int> GetValueEvents(string valueType, string? eventUid)
        {
            var command = $@"
                select 
                    {valueType}
                from value_events
                where event_uid = @eventUid
            ";
            return await _dbConnection.QueryFirstOrDefaultAsync<int>(command, new { eventUid });
        }

        private async Task<bool> ValidInsert(string name, string companyUid)
        {
            var command = $@"
                select
                    uid
                from event
                where 
                    company_uid = @companyUid              
                    and name = @name
                    and on_display = 1                     
            ";

            var uid = await _dbConnection.QueryFirstOrDefaultAsync<string>(command, new { name, companyUid });
            return String.IsNullOrEmpty(uid) ? true : false;
        }

        private async Task<string> LastInsert(string name, string companyUid)
        {
            var command = $@"
                select
                    uid
                from events
                where 
                    company_uid = @companyUid              
                    and name = @name
                    and on_display = 1                     
            ";

            return await _dbConnection.QueryFirstOrDefaultAsync<string>(command, new { name, companyUid });
        }
    }
}
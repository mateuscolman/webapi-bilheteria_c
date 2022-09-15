using System.Data;
using Dapper;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Infra.Repository
{
    public class EventsRepository : IEventsRepository
    {
        private readonly IDbConnection _dbConnection;
        
        public EventsRepository(IDbConnection dbConnection){
            _dbConnection = dbConnection;
        }

        public async Task<List<Events>> GetEventsOnDisplayByCompany(string? companyUid){
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

            var events = await _dbConnection.QueryAsync<Events>(command, new {companyUid});
            return events.ToList();
        }
        
        public async Task<List<Events>> GetEventsOnDisplay(){
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
                where on_display = 1
            ";            

            var events = await _dbConnection.QueryAsync<Events>(command);
            return events.ToList();
        }

        public async Task<string> InsertEvent(Events events){
            if (!ValidInsert(events.Name, events.CompanyUid).Result) throw new Exception("Events already exist"); 
            var command = $@"
                insert into events
                values(uuid(), @name, @startsIn, @endsIn, 1, @description, 0, @reason, @companyUid, @publishedBy)
            ";   
            await _dbConnection.ExecuteAsync(command, new { 
                name = events.Name,
                startsIn = events.StartsIn,
                endsIn = events.EndsIn,
                description = events.Description,
                reason = events.Reason,
                companyUid = events.CompanyUid,
                publishedBy = events.PublishedBy
             });

            return LastInsert(events.Name, events.CompanyUid).Result;                      
        }

        public async Task InsertEventTime(DateTime date, string? eventUid){
            var command = $@"
                insert into events_time
                values(uuid(), @date, @date, @eventUid)
            ";
            await _dbConnection.ExecuteAsync(command, new {date, eventUid});
        }

        public async Task<List<EventsTime>> GetEventsTime(string? eventUid){
            var command = $@"
                select
                    start,
                    end
                from events_time 
                where event_uid = @eventUid";
            
            var eventsTime = await _dbConnection.QueryAsync<EventsTime>(command, eventUid);
            return eventsTime.ToList();             
        }

        private async Task<bool> ValidInsert(string name, string companyUid){
            var command = $@"
                select
                    uid
                from events
                where 
                    company_uid = @companyUid              
                    and name = @name
                    and on_display = 1                     
            ";

            var uid = await _dbConnection.QueryFirstOrDefaultAsync<string>(command, new {name, companyUid});
            return String.IsNullOrEmpty(uid) ? true : false;
        }
        
        private async Task<string> LastInsert(string name, string companyUid){
            var command = $@"
                select
                    uid
                from events
                where 
                    company_uid = @companyUid              
                    and name = @name
                    and on_display = 1                     
            ";

            return await _dbConnection.QueryFirstOrDefaultAsync<string>(command, new {name, companyUid});            
        }
    }
}
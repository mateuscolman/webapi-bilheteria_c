using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;

namespace webapi_bilheteria_c.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;  

        public EventsService(IEventsRepository eventsRepository){
            _eventsRepository = eventsRepository;
        } 

        public Events GetEventsByUid(string? uid){
            var events = _eventsRepository.GetEventByUid(uid).Result;
            events.EventsTimes = _eventsRepository.GetEventsTime(events.Uid).Result;
            return events;
        }

        public List<Events> GetEventsOnDisplay(){
            var events = _eventsRepository.GetEventsOnDisplay().Result;
            foreach (var item in events)
            {                  
                item.EventsTimes = _eventsRepository.GetEventsTime(item.Uid).Result;    
            }                            
            return events;
        }

        public List<Events> GetEventsOnDisplayByCompany(string? companyUid){
            var events = _eventsRepository.GetEventsOnDisplayByCompany(companyUid).Result;
            foreach (var item in events)
            {
                item.EventsTimes = _eventsRepository.GetEventsTime(item.Uid).Result;
            }
            return events;
        } 

        public bool InsertEvent(Events events){
            var eventUid = _eventsRepository.InsertEvent(events).Result;

            var daysToTheEvent = events.EndsIn - events.StartsIn;            
            DateTime insertDate = events.StartsIn;
            
            if (daysToTheEvent.Days == 0)
            {
                _eventsRepository.InsertEventTime(insertDate, eventUid);
                return true;
            }

            int count = 0;
            while (count <= daysToTheEvent.Days)
            {
                _eventsRepository.InsertEventTime(insertDate, eventUid);
                insertDate = insertDate.AddDays(1);
                count++;
            }
            return true;
        }
    }
}
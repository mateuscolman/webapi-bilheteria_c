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
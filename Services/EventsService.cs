using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;
using Microsoft.AspNetCore.Hosting;

namespace webapi_bilheteria_c.Services
{
    public class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventsService(IEventsRepository eventsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _eventsRepository = eventsRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public Event GetEventsByUid(string? uid)
        {
            var events = _eventsRepository.GetEventByUid(uid).Result;
            events.EventsTimes = _eventsRepository.GetEventsTime(events.Uid).Result;
            return events;
        }

        public List<EventOnDisplay> GetEventsOnDisplay()
        {
            var events = _eventsRepository.GetEventsOnDisplay().Result;
            foreach (var item in events)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(item.Image);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                item.Image = base64ImageRepresentation;
            }
            return events;
        }

        public List<Event> GetEventsOnDisplayByCompany(string? companyUid)
        {
            var events = _eventsRepository.GetEventsOnDisplayByCompany(companyUid).Result;
            foreach (var item in events)
            {
                item.EventsTimes = _eventsRepository.GetEventsTime(item.Uid).Result;
            }
            return events;
        }

        public bool CreateEvent(Event events, int fullValue, IFormFile img)
        {
            var eventUid = Convert.ToString(Guid.NewGuid());
            string pathToGo = $@"{_webHostEnvironment.WebRootPath}/imagem/{events.CompanyUid}";
            string newName = $@"{eventUid}_default.jpg";

            if (!Directory.Exists(pathToGo))
            {
                Directory.CreateDirectory(pathToGo);
            }

            string filePath = pathToGo + newName;
            using (var stream = System.IO.File.Create(filePath))
            {
                img.CopyToAsync(stream);
            }

            events.PresentationImage = filePath;
            events.Uid = eventUid;
            var response = _eventsRepository.InsertEvent(events).Result;
            _eventsRepository.InsertValueEvent(eventUid, fullValue);

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
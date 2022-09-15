namespace webapi_bilheteria_c.Domain.Models
{
    public class Events
    {
        public string? Uid { get; set; }   
        public string? Name { get; set; } 
        public DateTime StartsIn { get; set; }
        public DateTime EndsIn { get; set; }
        public int OnDisplay { get; set; }
        public string? Description { get; set; }
        public int Cancelled { get; set; }
        public string? Reason { get; set; }
        public string? CompanyUid { get; set; }
        public string? PublishedBy { get; set; }
        public List<EventsTime>? EventsTimes { get; set; }
    }
}
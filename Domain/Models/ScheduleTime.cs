namespace webapi_bilheteria_c.Domain.Models
{
    public class ScheduleTime
    {
        public string? Uid { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? EventUid { get; set; }
    }
}
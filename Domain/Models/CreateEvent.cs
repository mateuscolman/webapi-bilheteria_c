namespace webapi_bilheteria_c.Domain.Models
{
    public class CreateEvent
    {
        public string? Name { get; set; }
        public DateTime StartsIn { get; set; }
        public DateTime EndsIn { get; set; }
        public string? Description { get; set; }
        public string? CompanyUid { get; set; }
        public int FullValue { get; set; }
    }
}
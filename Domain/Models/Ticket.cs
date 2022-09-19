namespace webapi_bilheteria_c.Domain.Models
{
    public class Ticket
    {
        public string? EvemtUid { get; set; }
        public string? EventName { get; set; }
        public string? Value { get; set; }
        public string? PayerName { get; set; }
        public string? PayerDocument { get; set; }
        public List<string>? AlocateTo { get; set; }
    }
}
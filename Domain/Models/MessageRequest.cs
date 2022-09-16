namespace webapi_bilheteria_c.Domain.Models
{
    public class MessageRequest
    {
        public string? Uid { get; set; }
        public string? Section { get; set; }
        public string? Value { get; set; }
    }
}
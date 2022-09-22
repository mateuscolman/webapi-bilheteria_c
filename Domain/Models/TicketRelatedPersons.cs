namespace webapi_bilheteria_c.Domain.Models
{
    public class TicketRelatedPersons
    {
        public string? Uid { get; set; }
        public string? Name { get; set; }
        public string? EventUid { get; set; }
        public string? Document { get; set; }
        public int Responsible { get; set; }
        public string? LinkedUid { get; set; }
        public int HalfPrice { get; set; }
        public int SpecialCondition { get; set; }
    }
}
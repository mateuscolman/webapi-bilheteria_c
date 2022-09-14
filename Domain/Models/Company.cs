namespace webapi_bilheteria_c.Domain.Models
{
    public class Company
    {
        public string? Uid { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public string? Name { get; set; }
        public int Active { get; set; }
        public string? OwnerUid { get; set; }
    }
}
namespace webapi_bilheteria_c.Domain.Models
{
    public class Parameters
    {
        public string? Code { get; set; }
        public string? Description { get; set; }             
        public string? Value {get; set;}
        public DateTime Created { get; set; }
        public int Active { get; set; } 
    }
}
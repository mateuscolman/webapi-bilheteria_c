namespace webapi_bilheteria_c.Domain.Models
{
        public class Users
    {
        public string? Uid { get; set; }
        public string? Email { get; set; }    
        public string? Password { get; set; }
        public int Privileges { get; set; }
        public DateTime Created { get; set; }                      
        public DateTime LastAcess { get; set; }
        public DateTime Birthday { get; set; }
        public string? Name { get; set; }
    }
}
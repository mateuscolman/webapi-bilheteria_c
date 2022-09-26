namespace webapi_bilheteria_c.Domain.Models
{
    public class EventOnDisplay
    {
        public string? Uid { get; set; }
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? Image { get; set; }
        public int DimensionX { get; set; }
        public int DimensionY { get; set; }
    }
}
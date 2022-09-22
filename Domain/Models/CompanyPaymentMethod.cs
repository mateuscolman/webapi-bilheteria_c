namespace webapi_bilheteria_c.Domain.Models
{
    public class CompanyPaymentMethod
    {
        public string? Uid { get; set; }
        public string? CompanyUid { get; set; }
        public string Name { get; set; }
        public string PaymentKey { get; set; }
    }
}
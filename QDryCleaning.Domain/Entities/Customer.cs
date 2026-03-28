namespace QDryClean.Domain.Entities
{
    public class Customer : Auditable
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? AdditionalPhoneNumber { get; set; }
        public decimal Points { get; set; }
        public ICollection<Order>? Orders { get; set; } = new List<Order>();
    }
}

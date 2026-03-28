namespace QDryClean.Application.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? AdditionalPhoneNumber { get; set; }
        public decimal Points { get; set; } = decimal.Zero;
    }
}

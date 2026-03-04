namespace QDryClean.Application.Dtos
{
    public class CreateOrderItemDto
    {
        public int ItemTypeId { get; set; }
        public string? Colour { get; set; }
        public string? BrandName { get; set; }
        public string? Description { get; set; }
    }
}

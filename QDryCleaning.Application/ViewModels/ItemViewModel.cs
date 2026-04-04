using QDryClean.Application.Dtos;

namespace QDryClean.Application.ViewModels
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public ItemTypeDto ItemType { get; set; }
        public string? Colour { get; set; }
        public string? BrandName { get; set; }
        public string? Description { get; set; }
    }
}

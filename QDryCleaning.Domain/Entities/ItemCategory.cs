namespace QDryClean.Domain.Entities
{
    public class ItemCategory : Auditable
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<ItemType> ItemTypes { get; set; } = new List<ItemType>();
    }
}
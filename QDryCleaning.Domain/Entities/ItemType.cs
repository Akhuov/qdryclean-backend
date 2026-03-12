namespace QDryClean.Domain.Entities
{
    public class ItemType : Auditable
    {
        public string Name { get; set; }
        public int ItemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public int? ChargeId { get; set; }
        public Charge? Charge { get; set; }
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
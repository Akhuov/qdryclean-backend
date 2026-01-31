
namespace QDryClean.Domain.Entities
{
    public class Charge : Auditable
    {
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public ICollection<ItemType> ItemTypes { get; set; } = new List<ItemType>();
    }
}

namespace DeckCart.Data.Entities
{
    public class ItemEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}

namespace DeckCart.Business.Models
{
    public class CartItem
    {
        public int ItemId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}

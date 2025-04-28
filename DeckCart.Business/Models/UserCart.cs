namespace DeckCart.Business.Models
{
    public class UserCart
    {
        public required string Name { get; set; }
        public required List<CartItem> Cart { get; set; }
    }
}

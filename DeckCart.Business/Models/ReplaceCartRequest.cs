namespace DeckCart.Business.Models
{
    public class ReplaceCartRequest
    {
        public int UserId { get; set; }
        public required List<ReplaceCartItem> Cart { get; set; }
    }
}

namespace DeckCart.App.Facade
{
    public class UserCartFacade
    {
        public string Name { get; set; } = string.Empty;
        public required List<CartItemFacade> Cart { get; set; }
    }
}

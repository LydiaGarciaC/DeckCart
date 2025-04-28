using DeckCart.Business.Models;

namespace DeckCart.Business.Handlers.Interfaces
{
    public interface ICartHandler
    {
        Task<UserCart> GetUserCartAsync(int userId);
        Task ReplaceUserCartAsync(ReplaceCartRequest request);
    }
}

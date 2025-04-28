using DeckCart.Data.Entities;

namespace DeckCart.Data.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task RemoveCartItems(IEnumerable<CartEntity> items);
        Task AddCartItems(IEnumerable<CartEntity> items);
        Task SaveChangesAsync();
    }
}

using DeckCart.Data.Entities;

namespace DeckCart.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetUserWithCartByIdAsync(int userId);
    }
}

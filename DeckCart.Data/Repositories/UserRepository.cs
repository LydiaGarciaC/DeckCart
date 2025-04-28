using DeckCart.Data.Entities;
using DeckCart.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeckCart.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetUserWithCartByIdAsync(int userId)
        {
            return await _context.Users
                                 .Include(u => u.CartItems.Where(ci => ci.DeletedOn == null))
                                    .ThenInclude(ci => ci.Item)
                                 .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}

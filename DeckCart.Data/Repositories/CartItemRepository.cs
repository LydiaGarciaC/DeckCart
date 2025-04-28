using DeckCart.Data.Entities;
using DeckCart.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeckCart.Data.Repositories
{
    public class CartItemRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task RemoveCartItems(IEnumerable<CartEntity> items)
        {
            var cartItemIds = items.Select(item => item.Id).ToList();

            var now = DateTimeOffset.UtcNow; //We're separating this in a variable so all CartItems get the same deletedOn value

            _context.Carts
                          .Where(ci => cartItemIds.Contains(ci.Id))
                          .ExecuteUpdate(updates => updates
                              .SetProperty(ci => ci.DeletedOn, now));

            return Task.CompletedTask;
        }

        public Task AddCartItems(IEnumerable<CartEntity> items)
        {
            _context.Carts.AddRange(items);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

using DeckCart.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeckCart.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _context;

        public ItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DoItemsExist(IEnumerable<int> itemIds)
        {
            if (itemIds == null || !itemIds.Any())
            {
                return true;
            }

            var countOfExistingItems = await _context.Items.CountAsync(item => itemIds.Contains(item.Id));

            return countOfExistingItems == itemIds.Count();
        }
    }
}

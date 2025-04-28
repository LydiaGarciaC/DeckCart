namespace DeckCart.Data.Repositories.Interfaces
{
    public interface IItemRepository
    {
        Task<bool> DoItemsExist(IEnumerable<int> itemIds);
    }
}

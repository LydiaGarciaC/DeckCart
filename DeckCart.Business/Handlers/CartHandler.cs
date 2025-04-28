using DeckCart.Business.Handlers.Interfaces;
using DeckCart.Business.Models;
using DeckCart.Data.Repositories.Interfaces;
using Serilog;

namespace DeckCart.Business.Handlers
{
    public class CartHandler : ICartHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ICartRepository _cartRepository;

        public CartHandler(IUserRepository userRepository, IItemRepository itemRepository, ICartRepository cartRepository)
        {
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _cartRepository = cartRepository;
        }

        public async Task<UserCart> GetUserCartAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserWithCartByIdAsync(userId);

                if (user == null)
                {
                    Log.Error($"User with ID '{userId}' was not found");
                    throw new InvalidOperationException();
                }

                var cart = new UserCart
                {
                    Name = user.Name,
                    Cart = user.CartItems
                    .Select(ci => new Models.CartItem
                    {
                        ItemId = ci.ItemId,
                        Name = ci.Item.Name,
                        Price = ci.Item.Price
                    }).ToList()
                };

                return cart;
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Unexpected error");
                throw;
            }
            
        }

        public async Task ReplaceUserCartAsync(ReplaceCartRequest request)
        {
            try
            {
                var user = await _userRepository.GetUserWithCartByIdAsync(request.UserId);
                if (user == null)
                {
                    var errorMessage = $"User with ID '{request.UserId}' was not found";
                    Log.Error(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                var requestedItemIds = request.Cart.Select(c => c.ItemId).Distinct().ToList();
                if (requestedItemIds.Any())
                {
                    //We're gonna return an error if any of the items doesn't exist. Ideally we would specify which item is missing.
                    if (!await _itemRepository.DoItemsExist(requestedItemIds))
                    {
                        var errorMessage = "One or more items in the cart don't exist";
                        Log.Error(errorMessage);
                        throw new InvalidOperationException(errorMessage);
                    }
                }

                await _cartRepository.RemoveCartItems(user.CartItems);

                var newCartItems = requestedItemIds.Select(itemId => new Data.Entities.CartEntity
                {
                    UserId = user.Id,
                    ItemId = itemId,
                    CreatedOn = DateTime.UtcNow
                }).ToList();

                await _cartRepository.AddCartItems(newCartItems);

                await _cartRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error");
                throw;
            }
            
        }
    }
}

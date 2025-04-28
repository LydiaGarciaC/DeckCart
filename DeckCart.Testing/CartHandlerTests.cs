using Moq;
using DeckCart.Business.Handlers;
using DeckCart.Data.Repositories.Interfaces;
using DeckCart.Data.Entities;
using DeckCart.Business.Models;

namespace DeckCart.Business.Tests.Handlers
{
    public class CartHandlerTests
    {
        private readonly CartHandler _cartHandler;

        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;

        public CartHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _itemRepositoryMock = new Mock<IItemRepository>();
            _cartRepositoryMock = new Mock<ICartRepository>();

            _cartHandler = new CartHandler(
                _userRepositoryMock.Object,
                _itemRepositoryMock.Object,
                _cartRepositoryMock.Object);
        }

        [Fact]
        public async Task WhenRequestingCart_IfUserExistsWithItems_ReturnsUserCart()
        {
            // Arrange
            var userId = 3;
            var userEntity = new UserEntity
            {
                Id = userId,
                Name = "Test User",
                CartItems = new List<CartEntity>
                {
                    new CartEntity { ItemId = 3, Item = new ItemEntity { Id = 3, Name = "Item 3", Price = 33.3m } },
                    new CartEntity { ItemId = 4, Item = new ItemEntity { Id = 4, Name = "Item 4", Price = 44.4m } }
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetUserWithCartByIdAsync(userId))
                               .ReturnsAsync(userEntity);

            // Act
            var result = await _cartHandler.GetUserCartAsync(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.GetUserWithCartByIdAsync(userId), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(userEntity.Name, result.Name);
        }

        [Fact]
        public async Task WhenRequestingCart_IfUserDoesntExist_ReturnsError()
        {
            // Arrange
            var userId = 99; // A user ID that doesn't exist

            _userRepositoryMock.Setup(repo => repo.GetUserWithCartByIdAsync(userId))
                               .ReturnsAsync((UserEntity?)null);

            // Act
            var errorResult = await Record.ExceptionAsync(
                () => _cartHandler.GetUserCartAsync(userId)
            );

            // Assert
            _userRepositoryMock.Verify(repo => repo.GetUserWithCartByIdAsync(userId), Times.Once);
            Assert.NotNull(errorResult);
            Assert.IsType<InvalidOperationException>(errorResult);
        }

        [Fact]
        public async Task WhenReplacingCart_IfUserDoesntExist_ReturnsError()
        {
            // Arrange
            var userId = 99; // A user ID that doesn't exist
            var replaceRequest = new ReplaceCartRequest
            {
                UserId = userId,
                Cart = new List<ReplaceCartItem>
                {
                    new ReplaceCartItem { ItemId = 5 },
                    new ReplaceCartItem { ItemId = 6 }
                }
            };

            _userRepositoryMock.Setup(repo => repo.GetUserWithCartByIdAsync(userId))
                               .ReturnsAsync((UserEntity?)null);



            // Act
            var errorResult = await Record.ExceptionAsync(
                () => _cartHandler.ReplaceUserCartAsync(replaceRequest)
            );

            // Assert
            _userRepositoryMock.Verify(repo => repo.GetUserWithCartByIdAsync(userId), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.DoItemsExist(It.IsAny<IEnumerable<int>>()), Times.Never);
            Assert.NotNull(errorResult);
            Assert.IsType<InvalidOperationException>(errorResult);
        }
    }
}
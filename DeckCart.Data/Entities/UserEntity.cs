namespace DeckCart.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<CartEntity> CartItems { get; set; } = new List<CartEntity>();
    }
}

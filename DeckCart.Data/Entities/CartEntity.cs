namespace DeckCart.Data.Entities
{
    public class CartEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual UserEntity? User { get; set; }

        public int ItemId { get; set; }
        public virtual ItemEntity? Item { get; set; }



        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
    }
}

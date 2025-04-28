using System.ComponentModel.DataAnnotations;

namespace DeckCart.App.Facade
{
    public class ReplaceCartRequestFacade
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId has to be an integer bigger than 0")]
        public int UserId { get; set; }
        [Required]
        public required List<ReplaceCartItemFacade> Cart { get; set; }
    }
}

using AutoMapper;
using DeckCart.App.Facade;
using DeckCart.Business.Models;

namespace DeckCart.App.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {            
            CreateMap<ReplaceCartRequestFacade, ReplaceCartRequest>().ReverseMap();
            CreateMap<ReplaceCartItemFacade, ReplaceCartItem>().ReverseMap();
            CreateMap<UserCartFacade, UserCart>().ReverseMap();
            CreateMap<CartItemFacade, CartItem>().ReverseMap();
        }
    }
}

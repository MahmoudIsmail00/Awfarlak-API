using AutoMapper;
using Core.Entities.BasketEntities;

namespace Application.Services.BasketService.Dto
{
    public class BasketProfile : Profile
    {

        public BasketProfile()
        {
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();


        }
    }
}

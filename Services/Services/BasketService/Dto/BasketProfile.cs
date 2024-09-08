using AutoMapper;
using Infrastructure.BasketRepository.BasketEntities;

namespace Services.Services.BasketService.Dto
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

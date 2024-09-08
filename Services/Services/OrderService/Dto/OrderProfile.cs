using AutoMapper;
using Core.Entities.OrderEntities;
using Core.IdentityEntities;

namespace Services.Services.OrderService.Dto
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, ShippingAddress>();

            CreateMap<Order, OrderResultDto>()
                .ForMember(dest => dest.DeliveryMethod, option => option.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, option => option.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductItemId, option => option.MapFrom(src => src.ItemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, option => option.MapFrom(src => src.ItemOrdered.ProductName))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom(src => src.ItemOrdered.PictureUrl))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<OrderItemUrlResolver>());
        }
    }
}

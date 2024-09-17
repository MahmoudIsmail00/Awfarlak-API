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
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, opt => opt.MapFrom(src => src.DeliveryMethod.Price))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductItemId, opt => opt.MapFrom(src => src.ItemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ItemOrdered.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.ItemOrdered.PictureUrl))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));


            CreateMap<OrderItemDto, OrderItem>()
                .ForMember(dest => dest.ItemOrdered, opt => opt.MapFrom(src => new ProductItemOrdered
                {
                    ProductItemId = src.ProductItemId,
                    ProductName = src.ProductName,
                    PictureUrl = src.PictureUrl
                }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }




}

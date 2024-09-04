using AutoMapper;
using Core.Entities.OrderEntities;
using Microsoft.Extensions.Configuration;

namespace Services.Services.OrderService.Dto
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
                return _configuration["BaseUrl"] + source.ItemOrdered.PictureUrl;
            return null;
        }
    }
}

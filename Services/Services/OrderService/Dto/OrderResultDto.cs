using Core.Entities.OrderEntities;

namespace Services.Services.OrderService.Dto
{
    public class OrderResultDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public AddressDto ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public OrderStatus OrderStatus { get; set; } 
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
    }
}

namespace Services.Services.OrderService.Dto
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public string BuyerEmail { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    }
}

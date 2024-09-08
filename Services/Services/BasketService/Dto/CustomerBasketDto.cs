using System.ComponentModel.DataAnnotations;

namespace Services.Services.BasketService.Dto
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> BasketItems { get; set; } 
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}

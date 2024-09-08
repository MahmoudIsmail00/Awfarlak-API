using AutoMapper;
using Infrastructure.BasketRepository;
using Infrastructure.BasketRepository.BasketEntities;
using Services.Services.BasketService.Dto;


namespace Services.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
            => await _basketRepository.DeleteBasketAsync(basketId);


        public async Task<CustomerBasketDto> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if(basket is null)
                return new CustomerBasketDto();

            var mappedBasket = _mapper.Map<CustomerBasketDto>(basket);

            return mappedBasket;
        }

        public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket)
        {
            var customerbasket = _mapper.Map<CustomerBasket>(basket);

            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerbasket);

            var mappedCustomerbasket = _mapper.Map<CustomerBasketDto>(updatedBasket);

            return mappedCustomerbasket;

        }
    }
}

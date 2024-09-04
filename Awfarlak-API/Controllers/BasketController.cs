﻿using Microsoft.AspNetCore.Mvc;
using Services.Services.BasketService;
using Services.Services.BasketService.Dto;

namespace Awfarlak_API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketById(string id)
            => Ok(await _basketService.GetBasketAsync(id));
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasketAsync(CustomerBasketDto basket)
            => Ok(await _basketService.UpdateBasketAsync(basket));
        [HttpDelete]
        public async Task DeleteBasketById(string id)
            => Ok(await _basketService.DeleteBasketAsync(id));

    }
}

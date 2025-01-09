using Application.Services.BasketService;
using Application.Services.BasketService.Dto;
using Application.Services.CacheService;
using Application.Services.OrderService;
using Application.Services.OrderService.Dto;
using Application.Services.PaymentService;
using Application.Services.ProductService;
using Application.Services.ProductService.Dto;
using Application.Services.TokenService;
using Application.Services.UserService;
using Awfarlak_API.HandleResponses;
using Awfarlak_API.Helper;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace Awfarlak_API.Extensions
{
    public static class ApplicationSerivceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ImageHelper>();



            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                    .Where(model => model.Value.Errors.Count > 0)
                                    .SelectMany(model => model.Value.Errors)
                                    .Select(error => error.ErrorMessage).ToList();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //builder.Application.AddAutoMapper(x => x.AddProfile(new ProductProfile()));
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(BasketProfile));
            services.AddAutoMapper(typeof(OrderProfile));

            return services;
        }
    }
}

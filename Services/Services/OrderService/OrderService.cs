using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Services.Services.BasketService;
using Services.Services.OrderService.Dto;
using Services.Services.PaymentService;

namespace Services.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IBasketService basketService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderResultDto> CreateOrderAsync(OrderDto orderDto)
        {
            // Get Basket
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId);
            if (basket == null)
                return null;

            // Fill OrderItems From basket items
            var orderItems = new List<OrderItemDto>();

            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (productItem == null)
                    return null;
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItme = new OrderItem(productItem.Price, item.Quantity, itemOrdered);

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItme);
                orderItems.Add(mappedOrderItem);
            }

            // Get delivery method 
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            if (deliveryMethod == null)
                return null;
            // Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // ToDo => Check if order exist
            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);
            }
         

            // Create Order
            var mappedShppingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order(orderDto.BuyerEmail, mappedShppingAddress, deliveryMethod, mappedOrderItems, subTotal, basket.PaymentIntentId);

            await _unitOfWork.Repository<Order>().Add(order);

            await _unitOfWork.Complete();

            // Delete Basket
            await _basketService.DeleteBasketAsync(orderDto.BasketId);

            var mappedOrder = new OrderResultDto
            {
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                ShippingPrice = order.DeliveryMethod.Price,
                DeliveryMethod = order.DeliveryMethod.ShortName,
                SubTotal = order.SubTotal,
                OrderStatus = order.OrderStatus,
                OrderDate = order.OrderDate,
                Total = order.SubTotal,
                OrderItems = orderItems,
                ShippingAddress = orderDto.ShippingAddress
            };

            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrders()
        {
            var orders = await _unitOfWork.Repository<Order>().GetAllAsync();


            List<OrderResultDto> mappedOrders = new List<OrderResultDto>();

            foreach (var order in orders)
            {
                var orderitemsfromdb = await _unitOfWork.Repository<OrderItem>().GetAllWithSpecificationsAsync(new BaseSpecifications<OrderItem>(x=>x.OrderId == order.Id));

                var orderItems = new List<OrderItemDto>();

                foreach (var orderItem in orderitemsfromdb)
                {
                    var newitem = new OrderItemDto
                    {
                        ProductName = orderItem.ItemOrdered.ProductName,
                        PictureUrl = orderItem.ItemOrdered.PictureUrl,
                        Price = orderItem.Price,
                        ProductItemId = orderItem.ItemOrdered.ProductItemId,
                        Quantity = orderItem.Quantity
                    };
                    orderItems.Add(newitem);
                };

                var mappedorder = new OrderResultDto
                {
                    Id = order.Id,
                    BuyerEmail = order.BuyerEmail,
                    DeliveryMethod = "Free",
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus,
                    ShippingPrice = 0,
                    SubTotal = order.SubTotal,
                    Total = order.SubTotal,
                    OrderItems = orderItems,
                    ShippingAddress = new AddressDto
                    {
                        City = order.ShippingAddress.City,
                        State = order.ShippingAddress.State,
                        Street = order.ShippingAddress.Street,
                        FirstName = order.ShippingAddress.FirstName,
                        LastName = order.ShippingAddress.LastName,
                        ZipCode = order.ShippingAddress.ZipCode,
                    }
                };
                mappedOrders.Add(mappedorder);
            }
            return mappedOrders;

        }

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(buyerEmail);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecificationsAsync(specs);

            List<OrderResultDto> mappedOrders = new List<OrderResultDto>();


            foreach (var order in orders)
            {
                var orderItems = new List<OrderItemDto>();

                foreach (var orderItem in order.OrderItems)
                {
                    var newitem = new OrderItemDto
                    {
                        ProductName = orderItem.ItemOrdered.ProductName,
                        PictureUrl = orderItem.ItemOrdered.PictureUrl,
                        Price = orderItem.Price,
                        ProductItemId = orderItem.ItemOrdered.ProductItemId,
                        Quantity = orderItem.Quantity
                    };
                    orderItems.Add(newitem);
                };
            
                var mappedorder = new OrderResultDto
                {
                    Id = order.Id,
                    BuyerEmail = order.BuyerEmail,
                    DeliveryMethod = order.DeliveryMethod.ShortName,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus,
                    ShippingPrice = order.DeliveryMethod.Price,
                    SubTotal = order.SubTotal,
                    Total = order.SubTotal,
                    OrderItems = orderItems,
                    ShippingAddress = new AddressDto
                    {
                        City = order.ShippingAddress.City,
                        State = order.ShippingAddress.State,
                        Street = order.ShippingAddress.Street,
                        FirstName = order.ShippingAddress.FirstName,
                        LastName = order.ShippingAddress.LastName,
                        ZipCode = order.ShippingAddress.ZipCode,
                    }
                };
                mappedOrders.Add(mappedorder);
            }

            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecification(id, buyerEmail);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            var mappedOrder = _mapper.Map<OrderResultDto>(order);

            return mappedOrder;
        }
    }
}

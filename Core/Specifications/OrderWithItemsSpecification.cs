using Core.Entities.OrderEntities;

namespace Core.Specifications
{
    public class OrderWithItemsSpecification : BaseSpecifications<Order>
    {
        public OrderWithItemsSpecification(string email) : base(order => order.BuyerEmail == email)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }
        public OrderWithItemsSpecification(int id, string email)
            : base(order => order.BuyerEmail == email && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }

    }
}

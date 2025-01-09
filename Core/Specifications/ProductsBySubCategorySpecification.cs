using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class ProductsBySubCategorySpecification : BaseSpecifications<Product>
    {
        public ProductsBySubCategorySpecification(Expression<Func<Product, bool>> criteria, Expression<Func<Product, object>> includeExpression1, Expression<Func<Product, object>> includeExpression2) : base(criteria)
        {
            AddInclude(includeExpression1);
            AddInclude(includeExpression2);
        }
    }
}

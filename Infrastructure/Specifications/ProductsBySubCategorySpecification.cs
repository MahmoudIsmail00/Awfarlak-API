using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class ProductsBySubCategorySpecification : BaseSpecifications<Product>
    {
        public ProductsBySubCategorySpecification(Expression<Func<Product, bool>> criteria, Expression<Func<Product, object>> includeExpression1, Expression<Func<Product, object>> includeExpression2) :base(criteria)
        {
            AddInclude(includeExpression1);
            AddInclude(includeExpression2);
        }
    }
}

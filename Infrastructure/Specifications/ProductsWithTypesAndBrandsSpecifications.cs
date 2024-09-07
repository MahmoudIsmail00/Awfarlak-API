using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class ProductsWithTypesAndBrandsSpecifications : BaseSpecifications<Product>
    {
        public ProductsWithTypesAndBrandsSpecifications(ProductSpecification specification) 
            : base(x => 
                //(string.IsNullOrEmpty(specification.Search) || x.Name.Trim().ToLower().Contains(specification.Search)) &&
                (!specification.BrandId.HasValue || x.ProductBrandId == specification.BrandId) &&
                (!specification.TypeId.HasValue || x.SubCategoryId == specification.TypeId)
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.SubCategory);
            AddOrderBy(p => p.Name);
            ApplyPagination(specification.PageSize * (specification.PageIndex - 1), specification.PageSize);

            if (!string.IsNullOrEmpty(specification.Sort))
            {
                switch (specification.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price); 
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecifications(int? id)
            : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.SubCategory);
        }
    }
}

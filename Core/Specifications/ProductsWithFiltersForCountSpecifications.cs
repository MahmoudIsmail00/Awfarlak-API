using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithFiltersForCountSpecifications : BaseSpecifications<Product>
    {
        public ProductsWithFiltersForCountSpecifications(ProductSpecification specification)
        : base(x =>
            (string.IsNullOrEmpty(specification.Search) || x.Name.Trim().ToLower().Contains(specification.Search)) &&
            (!specification.BrandId.HasValue || x.ProductBrandId == specification.BrandId) &&
            (!specification.TypeId.HasValue || x.SubCategoryId == specification.TypeId)
        )
        {

        }
    }
}

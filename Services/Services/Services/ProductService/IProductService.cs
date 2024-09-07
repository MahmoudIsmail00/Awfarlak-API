using Core.Entities;
using Infrastructure.Specifications;
using Services.Helpers;
using Services.Services.ProductService.Dto;

namespace Services.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductResultDto> GetProductByIdAsync(int? id);
        Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSpecification specification);
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<SubCategory>> GetProductSubCategoryAsync();
        Task<IReadOnlyList<ProductSpecs>> GetProductSpecsAsync();

    }
}

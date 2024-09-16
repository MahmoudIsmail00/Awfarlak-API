using Core.Entities;
using Infrastructure.Specifications;
using Services.Helpers;
using Services.Services.ProductService.Dto;

namespace Services.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductResultDto> GetProductByIdAsync(int? id);

        Task CreateProductWithSpecs(ProductWithSpecsCreationDTO productWithSpecs);

        Task<ProductWithSpecsCreationDTO> UpdateProductWithSpecs(int? id, ProductWithSpecsCreationDTO productWithSpecs);

        Task DeleteProduct(int id);

        Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSpecification specification);

        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods();

        Task<IReadOnlyList<SubCategory>> GetProductSubCategoryAsync();

        Task<IReadOnlyList<ProductResultDto>> GetProductsBySubCategory(int? subCategoryId);

        Task<IReadOnlyList<ProductWithSpecsDto>> GetProductsBySubCategoryWithSpecs(int? subCategoryId);

        Task<IReadOnlyList<ProductResultDto>> GetAllProducts();

        Task<ProductWithSpecsDto> GetProductWithSpecsAsync(int? prodId);

        Task<IReadOnlyList<ProductSpecs>> GetProductSpecsAsync();

    }
}

using Core.Entities;
using Infrastructure.Specifications;
using Services.Helpers;
using Services.Services.ProductService.Dto;

namespace Services.Services.ProductService
{
    public interface IProductService
    {
        // Products
        Task<ProductResultDto> GetProductByIdAsync(int? id);
        Task CreateProductWithSpecs(ProductWithSpecsCreationDTO productWithSpecs);
        Task<ProductWithSpecsCreationDTO> UpdateProductWithSpecs(int? id, ProductWithSpecsCreationDTO productWithSpecs);
        Task DeleteProduct(int id);
        Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSpecification specification);
        Task<IReadOnlyList<ProductResultDto>> GetProductsBySubCategory(int? subCategoryId);
        Task<IReadOnlyList<ProductWithSpecsDto>> GetProductsBySubCategoryWithSpecs(int? subCategoryId);
        Task<IReadOnlyList<ProductResultDto>> GetAllProducts();
        Task<ProductWithSpecsDto> GetProductWithSpecsAsync(int? prodId);


        //Sub Categories
        Task<ProductSubCategoryDto> GetSubCategory(int? subCategoryId);
        Task<IReadOnlyList<SubCategory>> GetProductSubCategoryAsync();
        Task CreateSubCategory(ProductSubCategoryDto subCategory);
        Task<ProductSubCategoryDto> UpdateSubCategory(int? id, ProductSubCategoryDto subCategory);
        Task DeleteSubCategory(int id);


        //Brands
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<ProductBrand> GetProductBrandById(int? id);
        Task CreateBrand(ProductBrand productBrand);
        Task<ProductBrand> UpdateBrand(int? id, ProductBrand productBrand);
        Task DeleteBrand(int id);

        //Types
        Task<IReadOnlyList<ProductType>> GetProductTypes();

        Task<IReadOnlyList<ProductSpecs>> GetProductSpecsAsync();


        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods();




    }
}

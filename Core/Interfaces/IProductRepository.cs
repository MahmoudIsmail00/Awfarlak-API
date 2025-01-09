using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int? id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<SubCategory>> GetProductSubCategoryAsync();
        Task<IReadOnlyList<ProductSpecs>> GetProductSpecsAsync();


    }
}

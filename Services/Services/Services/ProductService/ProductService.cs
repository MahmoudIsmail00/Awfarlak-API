using AutoMapper;
using Core.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Services.Helpers;
using Services.Services.ProductService.Dto;

namespace Services.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
            => await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductSpecs>> GetProductSpecsAsync()
            => await _unitOfWork.Repository<ProductSpecs>().GetAllAsync();

        public async Task<ProductResultDto> GetProductByIdAsync(int? id)
        {
            var specs = new ProductsWithTypesAndBrandsSpecifications(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationsAsync(specs);

            var mappedProduct = _mapper.Map<ProductResultDto>(product);

            return mappedProduct;
        }
        public async Task<Pagination<ProductResultDto>> GetProductsAsync(ProductSpecification specification)
        {
            var specs = new ProductsWithTypesAndBrandsSpecifications(specification);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(specs);

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDto>>(products);

            return new Pagination<ProductResultDto>(specification.PageIndex, specification.PageSize, totalItems, mappedProducts);
        }
        public async Task<IReadOnlyList<SubCategory>> GetProductSubCategoryAsync()
            => await _unitOfWork.Repository<SubCategory>().GetAllAsync();

    }
}

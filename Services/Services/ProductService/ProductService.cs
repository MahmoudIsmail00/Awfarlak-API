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

        ////////////////////////////////////////products by sub category//////////////////////////////////////

        public async Task<IReadOnlyList<ProductResultDto>> GetProductsBySubCategory(int? subCategoryId)
        {
            var specs = new ProductsBySubCategorySpecification
                (x => x.SubCategoryId == subCategoryId, y => y.ProductBrand, z => z.SubCategory);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDto>>(products);

            return mappedProducts;
        }
        /// ////////////////////////////////// Get All Products  /////////////////////////////////       
        public async Task<IReadOnlyList<ProductResultDto>> GetAllProducts()
        {
            var specs = new BaseSpecifications<Product>(x => true);
            specs.Includes.Add(x => x.ProductBrand);
            specs.Includes.Add(x => x.SubCategory);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);
            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDto>>(products);
            return mappedProducts;
        }
        /////////////////////////////Get Product With Specs//////////////////////////////////////////////
        public async Task<ProductWithSpecsDto> GetProductWithSpecsAsync(int? prodId)
        {

            var productSpecifications = new BaseSpecifications<Product>(x => x.Id == prodId);
            productSpecifications.Includes.Add(x => x.ProductBrand);
            productSpecifications.Includes.Add(x => x.SubCategory);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationsAsync(productSpecifications);

            var specs = new BaseSpecifications<ProductSpecs>(x => x.productId == prodId);

            var productSpecs = await _unitOfWork.Repository<ProductSpecs>().GetEntityWithSpecificationsAsync(specs);

            var productWithSpecs = new ProductWithSpecsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                ProductSubCategoryName = product.SubCategory.Name,
                ProductBrandName = product.ProductBrand.Name,
                Storage = productSpecs.Storage,
                RAM = productSpecs.RAM,
                CPU = productSpecs.CPU,
                GPU = productSpecs.GPU,
                Screen = productSpecs.Screen,
                Color = productSpecs.Color,
                Keyboard = productSpecs.Keyboard,
                Warranty = productSpecs.Warranty,
                Panel = productSpecs.Panel,
                Touchscreen = productSpecs.Touchscreen,
                Quantity = productSpecs.Quantity
            };

            return productWithSpecs;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IReadOnlyList<ProductWithSpecsDto>> GetProductsBySubCategoryWithSpecs(int? subCategoryId)
        {
            var specs = new ProductsBySubCategorySpecification
                (x => x.SubCategoryId == subCategoryId, y => y.ProductBrand, z => z.SubCategory);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);

            var ProductSpecs = await _unitOfWork.Repository<ProductSpecs>().GetAllAsync();

            List<ProductSpecs> ProductsSpecs = new List<ProductSpecs>();

            foreach (var item in products)
            {
                ProductsSpecs.Add(ProductSpecs.FirstOrDefault(x => x.productId == item.Id));
            }
            List<ProductWithSpecsDto> result = new List<ProductWithSpecsDto>();

            foreach (var product in products)
            {
                ProductSpecs prods = ProductsSpecs.Find(x => x.productId == product.Id);
                var productWithSpecs = new ProductWithSpecsDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    PictureUrl = product.PictureUrl,
                    ProductSubCategoryName = product.SubCategory.Name,
                    ProductBrandName = product.ProductBrand.Name,
                    Storage = prods.Storage,
                    RAM = prods.RAM,
                    CPU = prods.CPU,
                    GPU = prods.GPU,
                    Screen = prods.Screen,
                    Color = prods.Color,
                    Keyboard = prods.Keyboard,
                    Warranty = prods.Warranty,
                    Panel = prods.Panel,
                    Touchscreen = prods.Touchscreen,
                    Quantity = prods.Quantity
                };
                result.Add(productWithSpecs);
            }
            return result;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IReadOnlyList<SubCategory>> GetProductSubCategoryAsync()
            => await _unitOfWork.Repository<SubCategory>().GetAllAsync();

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
         => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        ////////////////////////////////////Create new product ////////////////////////////////////////

        public async Task CreateProductWithSpecs(ProductWithSpecsCreationDTO productWithSpecs)
        {
            var product = new Product
            {
                Name = productWithSpecs.Name,
                Description = productWithSpecs.Description,
                Price = productWithSpecs.Price,
                ProductBrandId = productWithSpecs.BrandId,
                SubCategoryId = productWithSpecs.SubCategoryId,
                PictureUrl = productWithSpecs.PictureUrl
            };

             await _unitOfWork.Repository<Product>().Add(product);
             await _unitOfWork.Complete();
            //var specification = new BaseSpecifications<Product>(x => x.PictureUrl == productWithSpecs.PictureUrl);

            var lastProduct = _unitOfWork.Repository<Product>().GetAllAsync().Result.LastOrDefault();
            var specs = new ProductSpecs
            {
                Color = productWithSpecs.Color,
                Screen = productWithSpecs.Screen,
                Storage = productWithSpecs.Storage,
                CPU = productWithSpecs.CPU,
                GPU = productWithSpecs.GPU,
                Keyboard = productWithSpecs.Keyboard,
                Warranty = productWithSpecs.Warranty,
                Panel = productWithSpecs.Panel,
                Quantity = productWithSpecs.Quantity,
                RAM = productWithSpecs.RAM,
                Touchscreen = productWithSpecs.Touchscreen,
                productId = lastProduct.Id
            };

            await _unitOfWork.Repository<ProductSpecs>().Add(specs);
            await  _unitOfWork.Complete();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);

            var specs = new BaseSpecifications<ProductSpecs>(x=>x.productId == id);

            var productSpecs = await _unitOfWork.Repository<ProductSpecs>().GetEntityWithSpecificationsAsync(specs);

            if(product == null)
                return;

             _unitOfWork.Repository<ProductSpecs>().Delete(productSpecs);
            await _unitOfWork.Complete();

            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.Complete();

        }
        //////////////////////////////////////////////////////////////////////////////////////////////

    }
}

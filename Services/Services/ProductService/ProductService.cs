﻿using AutoMapper;
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
                (x => x.SubCategoryId == subCategoryId , y=>y.ProductBrand , z=> z.SubCategory);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecificationsAsync(specs);

            var mappedProducts = _mapper.Map<IReadOnlyList<ProductResultDto>>(products);

            return mappedProducts;
        }
        /// ////////////////////////////////// Get All Products  /////////////////////////////////       
        public async Task<IReadOnlyList<ProductResultDto>> GetAllProducts()
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
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
        public async Task<IReadOnlyList<SubCategory>> GetProductSubCategoryAsync()
            => await _unitOfWork.Repository<SubCategory>().GetAllAsync();

       
    }
}

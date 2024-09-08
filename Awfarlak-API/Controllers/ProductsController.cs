using Awfarlak_API.HandleResponses;
using Core.Entities;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;
using Services.Services.ProductService;
using Services.Services.ProductService.Dto;

namespace Awfarlak_API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;


        public ProductsController(IProductService productService)
        {
            _productService = productService;

        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductResultDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();

            return Ok(products);
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductResultDto>>> GetProducts([FromQuery] ProductSpecification specification)
        {
            var products = await _productService.GetProductsAsync(specification);

            return Ok(products);
        }
        [HttpGet("{subCategory}")]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProductsBySubCat(int subCategory)
        {
            var products = await _productService.GetProductsBySubCategory(subCategory);

            return Ok(products);
        }
        [HttpGet("{prodId}")]
        public async Task<ActionResult<ProductWithSpecsDto>> GetProductWithSpecs(int prodId)
        {
            var product = await _productService.GetProductWithSpecsAsync(prodId);

            return Ok(product);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductResultDto>> GetProductById(int? id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }


        [HttpGet]
        [Route("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            => Ok(await _productService.GetProductBrandsAsync());

        [HttpGet]
        [Route("Specs")]
        public async Task<ActionResult<IReadOnlyList<ProductSpecs>>> GetProductSpecs()
            => Ok(await _productService.GetProductSpecsAsync());

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<SubCategory>>> GetProductSubCategories()
            => Ok(await _productService.GetProductSubCategoryAsync());
    }
}

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
        private IWebHostEnvironment _webHostEnvironment;


        public ProductsController(IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductWithSpecs(ProductWithSpecsCreationDTO productWithSpecs)
        {
                //var subCategoryName =  _productService.GetProductSubCategoryAsync().Result.Where(x=> x.Id == productWithSpecs.Id);
                //string RootPath = _webHostEnvironment.WebRootPath;
                //string filename = productWithSpecs.PictureUrl.Split(@"/")[2];
                //var upload = Path.Combine(RootPath, $"images/{subCategoryName}");
                //var ext = Path.GetExtension(productWithSpecs.PictureUrl.Split(@".")[productWithSpecs.PictureUrl.Length - 1]);
                //using (var fileStream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                //{
                //    file.CopyTo(fileStream);
                //}
            await _productService.CreateProductWithSpecs(productWithSpecs);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok();
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
        [HttpGet("{subCategory}")]
        public async Task<ActionResult<IReadOnlyList<ProductWithSpecsDto>>> GetProductsBySubCategoryWithSpecs(int subCategory)
        {
            var products = await _productService.GetProductsBySubCategoryWithSpecs(subCategory);

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

        [HttpGet("Delivery")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
            => Ok(await _productService.GetDeliveryMethods());

    }
}

using Awfarlak_API.HandleResponses;
using Awfarlak_API.Helper;
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
        private readonly ImageHelper _imageHelper;


        public ProductsController(IProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _imageHelper = new ImageHelper(webHostEnvironment);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductWithSpecs([FromForm] ProductWithSpecsCreationDTO productWithSpecs)
        {

            if (productWithSpecs.ImageFile != null)
            {
                productWithSpecs.PictureUrl = await _imageHelper.SaveImageAsync(productWithSpecs.ImageFile);
            }

            await _productService.CreateProductWithSpecs(productWithSpecs);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductWithSpecsCreationDTO>> UpdateProductWithSpecs([FromRoute] int? id, [FromForm] ProductWithSpecsCreationDTO productWithSpecs)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound();


            if (productWithSpecs.ImageFile != null)
            {
                productWithSpecs.PictureUrl = await _imageHelper.UpdateImageAsync(productWithSpecs.ImageFile, existingProduct.PictureUrl);
            }


            var prod = await _productService.UpdateProductWithSpecs(id, productWithSpecs);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            if (!string.IsNullOrEmpty(product.PictureUrl))
            {
                await _imageHelper.DeleteImageAsync(product.PictureUrl);
            }
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
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var types = await _productService.GetProductTypes();

            return Ok(types);
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

        ///////////////////////////////////////// Brands //////////////////////////////////////////////////////////
        [HttpGet]
        [Route("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
            => Ok(await _productService.GetProductBrandsAsync());

        [HttpGet("{brandId}")]
        public async Task<ActionResult<ProductBrand>> GetBrandById(int? brandId)
        {
            return Ok(await _productService.GetProductBrandById(brandId));
        }
        [HttpPost]
        public async Task<ActionResult> CreateNewBrand(ProductBrand brand)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _productService.CreateBrand(brand);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductBrand>> UpdateBrand([FromRoute] int? id, ProductBrand productBrand)
        {
            var brand = await _productService.UpdateBrand(id, productBrand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            await _productService.DeleteBrand(id);
            return Ok();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [Route("Specs")]
        public async Task<ActionResult<IReadOnlyList<ProductSpecs>>> GetProductSpecs()
            => Ok(await _productService.GetProductSpecsAsync());

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<SubCategory>>> GetProductSubCategories()
            => Ok(await _productService.GetProductSubCategoryAsync());

        [HttpGet("{subCategoryId}")]
        public async Task<ActionResult<ProductSubCategoryDto>> GetSubCategoryById(int? subCategoryId)
        {
            return Ok(await _productService.GetSubCategory(subCategoryId));
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewSubCategory(ProductSubCategoryDto subCategory)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _productService.CreateSubCategory(subCategory);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductSubCategoryDto>> UpdateSubCategory([FromRoute] int? id, ProductSubCategoryDto subCategoryDto)
        {
            var prod = await _productService.UpdateSubCategory(id, subCategoryDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubCategory(int id)
        {
            await _productService.DeleteSubCategory(id);
            return Ok();
        }

        [HttpGet("Delivery")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
            => Ok(await _productService.GetDeliveryMethods());

    }
}

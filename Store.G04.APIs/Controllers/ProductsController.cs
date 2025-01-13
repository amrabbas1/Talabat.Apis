using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Attributes;
using Store.G04.APIs.Errors;
using Store.G04.core.Dtos.Products;
using Store.G04.core.Helper;
using Store.G04.core.Services.Contract;
using Store.G04.core.Specifications.Products;

namespace Store.G04.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IproductService _productService;

        public ProductsController(IproductService productService)
        {
            _productService = productService;
        }

        [ProducesResponseType(typeof(PaginationResponse<ProductDto>), StatusCodes.Status200OK)]
        [HttpGet]// Get BaseUrl/api/Products
        [Cached(100)]
        //sort : name,priceAsc,priceDesc
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProducts([FromQuery] ProductSpecParams productSpec)//endpoint
        {
            var result = await _productService.GetAllProductsAsync(productSpec);

            return Ok(result);
        }
        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("brands")]// Get BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }
        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")] //Get BaseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]//Get BaseUrl/api/Products
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));

            var result = await _productService.GetProductByIdAsync(id.Value);

            if (result is null) return NotFound(new ApiErrorResponse(400, $"The produt with Id : {id} not found at DB :("));

            return Ok(result);
        }
    }
}

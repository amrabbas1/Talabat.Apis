using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.G04.core.Services.Contract;

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
        [HttpGet]// Get BaseUrl/api/Products
        public async Task<IActionResult> GetAllProducts()//endpoint
        {
            var result = await _productService.GetAllProductsAsync();
            return Ok(result);
        }
        [HttpGet("brands")]// Get BaseUrl/api/Products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _productService.GetAllBrandsAsync();
            return Ok(result);
        }
        [HttpGet("types")] //Get BaseUrl/api/Products/types
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await _productService.GetAllTypesAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]//Get BaseUrl/api/Products
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest("Invalid Id !!");

            var result = await _productService.GetProductByIdAsync(id.Value);

            if (result is null) return NotFound($"The produt with Id : {id} not found at DB :(");

            return Ok(result);
        }
    }
}

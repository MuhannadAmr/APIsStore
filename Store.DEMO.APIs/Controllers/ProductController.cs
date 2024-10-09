using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DEMO.Core.Services.Contract;
using Store.DEMO.Core.Specifications.Products;

namespace Store.DEMO.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;   
        }

        [HttpGet] //GET BaseUrl/api/product
        public async Task<IActionResult> GetAllProduct([FromQuery] ProductSpecParams param)
        {
           var products = await _productService.GetAllProductsAsync(param);
            return Ok(products);
        }

        [HttpGet("brands")] // BaseUrl/api/product/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _productService.GetAllBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("types")] // BaseUrl/api/product/types
        public async Task<IActionResult> GetAllTypes()
        {
            var types = await _productService.GetAllTypesAsync();
            return Ok(types);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest("InValid id!!");
            var product = await _productService.GetProductByIdAsync(id.Value);
            if(product is null) return NotFound($"The Product With Id: {id} not found at database :(");
            return Ok(product);
        }

    }
}

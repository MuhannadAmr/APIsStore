using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DEMO.APIs.Attributes;
using Store.DEMO.APIs.Errors;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Helper;
using Store.DEMO.Core.Services.Contract;
using Store.DEMO.Core.Specifications.Products;

namespace Store.DEMO.APIs.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;   
        }

        [ProducesResponseType(typeof(PaginationResponse<ProductDto>) , StatusCodes.Status200OK)]
        [HttpGet] //GET BaseUrl/api/product
        [Cached(100)]
        [Authorize]
        public async Task<ActionResult<PaginationResponse<ProductDto>>> GetAllProduct([FromQuery] ProductSpecParams param)
        {
           var products = await _productService.GetAllProductsAsync(param);
            return Ok(products);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("brands")] // BaseUrl/api/product/brands
        [Authorize]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllBrands()
        {
            var brands = await _productService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("types")] // BaseUrl/api/product/types
        [Authorize]
        public async Task<ActionResult<IEnumerable<TypeBrandDto>>> GetAllTypes()
        {
            var types = await _productService.GetAllTypesAsync();
            return Ok(types);
        }


        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400, "InValid id!!"));
            var product = await _productService.GetProductByIdAsync(id.Value);
            if(product is null) return NotFound(new ApiErrorResponse(404 , $"The Product With Id: {id} not found at database :("));
            return Ok(product);
        }

    }
}

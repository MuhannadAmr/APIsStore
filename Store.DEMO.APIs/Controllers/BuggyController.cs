using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DEMO.APIs.Errors;
using Store.DEMO.Repository.Data.Contexts;

namespace Store.DEMO.APIs.Controllers
{

    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var brand = await _context.Brands.FindAsync(100);
            if (brand is null) return NotFound(new ApiErrorResponse(404));
            return Ok(brand);
        }
        [HttpGet("servererror")]
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _context.Brands.FindAsync(100);
            var brandToString = brand.ToString();
            return Ok(brand);
        }
        [HttpGet("badrequest")]
        public async Task<IActionResult> GetBadRequestError()
        {
            
            return BadRequest(new ApiErrorResponse(400));
        }
        [HttpGet("badrequest/{id}")]
        public async Task<IActionResult> GetBadRequestError(int id)
        {

            return Ok();
        }
        [HttpGet("unauthorized")]
        public async Task<IActionResult> GetUnauthorizedError(int id)
        {

            return Unauthorized(new ApiErrorResponse(401));
        }
    }
}

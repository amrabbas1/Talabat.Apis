using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Errors;
using Store.G04.Repository.Data.Contexts;

namespace Store.G04.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public BuggyController(StoreDbContext context)
        {
            _context = context;
        }
        [HttpGet("notfound")]//Get BaseUrl /api/Buggy/notfound
        public async Task<IActionResult> GetNotFoundRequestError()
        {
            var brand = await _context.Brands.FindAsync(100);
            if (brand is null )
                return NotFound(new ApiErrorResponse(404));

            return Ok(brand);
        }
        [HttpGet("servererror")]//Get BaseUrl /api/Buggy/servererror
        public async Task<IActionResult> GetServerError()
        {
            var brand = await _context.Brands.FindAsync(100);
            var brandToString = brand.ToString();//will throw exception(null refeercne exception)

            return Ok(brand);
        }
        [HttpGet("badrequest")]//Get BaseUrl /api/Buggy/badrequest
        public async Task<IActionResult> GetBadRequestError()
        {
            return BadRequest(new ApiErrorResponse(400));
        }
        [HttpGet("badrequest/{id}")]//Get BaseUrl /api/Buggy/badrequest/ahmed
        public async Task<IActionResult> GetBadRequestError(int id)//Validation error
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiErrorResponse(400));
            return Ok();

        }
        [HttpGet("unauthorized")]//Get BaseUrl /api/Buggy/unauthorized
        public async Task<IActionResult> GetUnauthorizedError(int id)//Validation error
        {
            return Unauthorized(new ApiErrorResponse(401));
        }
    }
}

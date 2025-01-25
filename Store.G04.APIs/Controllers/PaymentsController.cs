using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Errors;
using Store.G04.core.Services.Contract;

namespace Store.G04.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var basket = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);
            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(basket);
        }
    }
}

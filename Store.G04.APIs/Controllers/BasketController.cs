using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Errors;
using Store.G04.core.Dtos.Baskets;
using Store.G04.core.Entities;
using Store.G04.core.Repositories.Contract;

namespace Store.G04.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string? id)// Get/api/basket
        {
            if (id == null) return BadRequest(new ApiErrorResponse(400, "Invalid Id !!"));
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket == null) new CustomerBasket() { Id = id };

            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasketDto model)// Post/api/basket
        {
            var basket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(model));

            if (basket == null) return BadRequest(new ApiErrorResponse(400));

            return Ok(basket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }

    }
}

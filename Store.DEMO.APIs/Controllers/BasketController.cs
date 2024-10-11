using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.DEMO.APIs.Errors;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Repositories.Contract;

namespace Store.DEMO.APIs.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustmerBasket>> GetBasket(string? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
           var basket = await _basketRepository.GetBasketAsync(id);
            if(basket is null) new CustmerBasket() { Id = id };
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<CustmerBasket>> CreateOrUpdateBasket(CustmerBasketDto model)
        {
            var basket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustmerBasket>(model));
            if (basket is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(basket);
        }
        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }

    }
}

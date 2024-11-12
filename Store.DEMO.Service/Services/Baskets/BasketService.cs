using AutoMapper;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Repositories.Contract;
using Store.DEMO.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Service.Services.Baskets
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }

        public async Task<CustmerBasketDto> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null) return _mapper.Map<CustmerBasketDto>(new CustmerBasket() { Id = basketId, Items = new List<BasketItem>()});
            return _mapper.Map<CustmerBasketDto>(basket);
        }

        public async Task<CustmerBasketDto> UpdateBasketAsync(CustmerBasketDto basketDto)
        {
            var basket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustmerBasket>(basketDto));
            if (basket is null) return null;
            return _mapper.Map<CustmerBasketDto>(basket);
        }
    }
}

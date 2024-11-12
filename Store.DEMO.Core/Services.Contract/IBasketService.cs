using Store.DEMO.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Services.Contract
{
    public interface IBasketService
    {
        Task<CustmerBasketDto> GetBasketAsync(string basketId);
        Task<CustmerBasketDto> UpdateBasketAsync(CustmerBasketDto basketDto);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}

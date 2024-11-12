using Store.DEMO.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustmerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId);
    }
}

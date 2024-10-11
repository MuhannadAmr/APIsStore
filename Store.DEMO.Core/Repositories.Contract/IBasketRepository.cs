using Store.DEMO.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        public Task<CustmerBasket?> GetBasketAsync(string id);
        public Task<CustmerBasket?> UpdateBasketAsync(CustmerBasket basket);
        public Task<bool> DeleteBasketAsync(string id);

    }
}

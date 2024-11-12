using Store.DEMO.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress);
        Task<IEnumerable<Order>?> GetOrdersForSpecificAsync(string buyerEmail);
        Task<Order?> GetOrderByIdForSpecificAsync(string buyerEmail, int orderId);
    }
}

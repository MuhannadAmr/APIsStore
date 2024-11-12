using Store.DEMO.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Specifications.Orders
{
    public class OrderSpecificationWithPaymentIntentId : BaseSpecifications<Order,int>
    {
        public OrderSpecificationWithPaymentIntentId(string paymentIntentId) : base(O => O.PaymentIntentId == paymentIntentId)
        {
            InCludes.Add(O => O.ShippingAddress);
            InCludes.Add(O => O.Items);
        }
        
    }
}

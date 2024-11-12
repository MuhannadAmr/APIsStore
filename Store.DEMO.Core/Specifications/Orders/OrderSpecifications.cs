using Store.DEMO.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Specifications.Orders
{
    public class OrderSpecifications : BaseSpecifications<Order,int>
    {
        public OrderSpecifications(string buyerEmail,int orderId)
            :base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            InCludes.Add(O => O.DeliveryMethod);
            InCludes.Add(O => O.Items);
        }
        public OrderSpecifications(string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail)
        {
            InCludes.Add(O => O.DeliveryMethod);
            InCludes.Add(O => O.Items);
        }
    }
}

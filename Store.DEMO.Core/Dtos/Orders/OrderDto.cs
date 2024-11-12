using Store.DEMO.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core.Dtos.Orders
{
    public class OrderDto
    {
        public string basketId { get; set; }
        public int deliveryMethodId { get; set; }
        public AddressDto shipToAddress { get; set; }
    }
}

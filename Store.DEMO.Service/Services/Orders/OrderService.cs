using Store.DEMO.Core;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Entites.Order;
using Store.DEMO.Core.Services.Contract;
using Store.DEMO.Core.Specifications.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketService basketService, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _paymentService = paymentService;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);
            var basket = await _basketService.GetBasketAsync(basketId);
            
            var orderItems = new List<OrderItem>();
            if (basket.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    var productItemOrder = new ProductItemOrder() { ProductId = product.Id, ProductName = product.Name, PictureUrl = product.PictureUrl };
                    var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            else { return null; }

            var subTotal = orderItems.Sum(P => P.Price * P.Quantity);

            if (!string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
                var exOrder = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                _unitOfWork.Repository<Order, int>().Delete(exOrder);
            }

            var basketDto = await _paymentService.CreateOrUpdatePaymentIntentIdAsync(basketId);

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basketDto.PaymentIntentId);
            await _unitOfWork.Repository<Order, int>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }

        public async Task<Order?> GetOrderByIdForSpecificAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrdersForSpecificAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var order = await _unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);
            if(order  is null) return null;
            return order;
        }
    }
}

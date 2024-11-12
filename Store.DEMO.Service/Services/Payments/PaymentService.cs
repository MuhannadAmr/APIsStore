using Microsoft.Extensions.Configuration;
using Store.DEMO.Core;
using Store.DEMO.Core.Dtos;
using Store.DEMO.Core.Entites.Order;
using Store.DEMO.Core.Services.Contract;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.DEMO.Core.Entites.Product;

namespace Store.DEMO.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketService basketService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustmerBasketDto> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var service = new PaymentIntentService();

            var basket = await _basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var ship = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = ship.Cost;
            }

            
            if(basket.Items.Count() > 0)
            {
                foreach ( var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            var subTotal = basket.Items.Sum(i=>i.Price * i.Quantity);


            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //Create

                var option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)((subTotal * 100) + (shippingPrice * 100)),
                    PaymentMethodTypes = new List<string>() { "card" },
                    Currency = "usd"
                };

                paymentIntent = await service.CreateAsync(option);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                //Update

                var option = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)((subTotal * 100) + (shippingPrice * 100)),
                };

                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, option);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            
            basket = await _basketService.UpdateBasketAsync(basket);
            if (basket is null) return null;
            return basket;
        }
    }
}

﻿using Microsoft.Extensions.Configuration;
using Store.G04.core;
using Store.G04.core.Entities;
using Store.G04.core.Entities.OdrerEntities;
using Store.G04.core.Repositories.Contract;
using Store.G04.core.Services.Contract;
using Store.G04.core.Specifications.Orders;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.G04.core.Entities.Product;

namespace Store.G04.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntentIdAsync(string basketId)
        {
            //Dowonload Stripe Package

            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            var shippingPrice = 0m;

            if(basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
            }

            if(basket.Items.Count > 0)
            {
                foreach ( var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long) ((subTotal + shippingPrice) * 100),
                    PaymentMethodTypes = new List<string>() {"card"},
                    Currency = "usd"
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                //Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)((subTotal + shippingPrice) * 100)
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId, options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            basket = await _basketRepository.UpdateBasketAsync(basket);
            if (basket == null) return null;

            return basket;
        }

        public async Task<Order> UpdatePaymentIntentForSucceedOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderSpecificationWithPaymentIntentId(paymentIntentId);
            var order = await _unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
            if(flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }

            _unitOfWork.Repository<Order, int>().Update(order);

            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}

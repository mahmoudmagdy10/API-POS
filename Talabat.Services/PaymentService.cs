using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepository;
using Talabat.Core.IService;
using Talabat.Repository.Specification;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IBasketRepo basketRepo, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];

            var Basket = await _basketRepo.GetBasketsAsync(BasketId);

            if (Basket is null) return null;

            var shippingPrice = 0m;

            if(Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                Basket.ShippingCost = DeliveryMethod.Cost;
                shippingPrice = DeliveryMethod.Cost;
            }

            if(Basket?.Items?.Count() > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    if(item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }

            var paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)Basket.Items.Sum(I => I.Price * I.Quantity * 100) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);

                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)Basket.Items.Sum(I => I.Price * I.Quantity * 100) + (long)shippingPrice * 100,
                };

                paymentIntent = await paymentIntentService.UpdateAsync(Basket.PaymentIntentId, options);

                await _basketRepo.UpdateBasketsAsync(Basket);
            }

            return Basket;
        }

        public async Task<Order> UpdatePaymentToSucceededOrFailed(string paymentId, bool isSucceeded)
        {
            var orderRep = _unitOfWork.Repository<Order>();
            var spec = new PaymentIntentSpecification(paymentId);
            var order = await orderRep.GetEntityWithSpecAsync(spec);

            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            orderRep.Update(order);
            await _unitOfWork.Complete();

            return order;
        }
    }
}

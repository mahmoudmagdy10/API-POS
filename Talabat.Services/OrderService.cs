using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepository;
using Talabat.Core.IService;
using Talabat.Repository.Specification;

namespace Talabat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRep<Product> _productRepo;
        //private readonly IGenericRep<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRep<Order> _orderRepo;

        public OrderService(
            IBasketRepo basketRepo,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            //IGenericRep<Product> productRepo,
            //IGenericRep<DeliveryMethod> deliveryMethodRepo,
            //IGenericRep<Order> orderRepo
            )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            // Get Basket
            var Basket = await _basketRepo.GetBasketsAsync(BasketId);
            if (Basket is null) return null;

            // Select Items From Basket
            var OrderItemsList = new List<OrderItem>();

            if(Basket.Items.Count() > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var productRepo = _unitOfWork.Repository<Product>();
                    if(productRepo is not null)
                    {
                        var product = await productRepo.GetByIdAsync(item.Id);
                        var ProductItemOrderd = new ProductItemOrder(product.Id, product.Name, product.PicUrl);
                        var OrderItem = new OrderItem(ProductItemOrderd, product.Price, item.Quantity);
                        OrderItemsList.Add(OrderItem);
                    }
                }
            }
            // Calc SubTotal
            var SubTotal = OrderItemsList.Sum(item => item.Quantity * item.Cost);

            // Get Delivery Method
            var deliveryMethodRepo = _unitOfWork.Repository<DeliveryMethod>();
            var DeliveryMethod = new DeliveryMethod();
            if (deliveryMethodRepo is not null)
            { 
                DeliveryMethod = await deliveryMethodRepo.GetByIdAsync(DeliveryMethodId);
            }


            // Payment Intent Id 
            var OrderRepo = _unitOfWork.Repository<Order>();
            if (OrderRepo is null) return null;
            var _paymentIntentId = Basket?.PaymentIntentId;
            if (_paymentIntentId is null) return null;
            var spec = new PaymentIntentSpecification(_paymentIntentId);
            var OrdersWithPI = await OrderRepo.GetEntityWithSpecAsync(spec);
            if (OrdersWithPI is not null)
            {
               OrderRepo.Delete(OrdersWithPI);
                await _paymentService.CreateOrUpdatePaymentIntent(Basket.Id);
            }

            // Create Order
            var order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItemsList, SubTotal);

            //var OrderRepo = _unitOfWork.Repository<Order>();
            if (OrderRepo is not null)
            {
                await OrderRepo.Add(order);

                // Save To DB
                var result = await _unitOfWork.Complete();

                if(result > 0)
                    return order;
            }

            return null;

        }
        public Task<Order> GetOrderByIdForUserAsync(string BuyerEmail, int OrderId)
        {
            var OrderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrdersOfUserSpecifications(BuyerEmail, OrderId);
            var order = OrderRepo.GetEntityWithSpecAsync(spec);
            return order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var OrderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrdersOfUserSpecifications(BuyerEmail);
            var orders = OrderRepo.GetAllWithSpecAsync(spec);
            return orders;

        }
    }
}

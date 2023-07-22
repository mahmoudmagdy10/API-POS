using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.IService
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address address);
        Task<Order> GetOrderByIdForUserAsync(string BuyerEmail, int OrderId);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string BuyerEmail);
    }
}

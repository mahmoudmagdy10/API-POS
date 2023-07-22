using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.IService
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
        public Task<Order> UpdatePaymentToSucceededOrFailed(string paymentId, bool status);

    }
}

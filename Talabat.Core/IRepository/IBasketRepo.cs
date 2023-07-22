using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.IRepository
{
    public interface IBasketRepo
    {
        Task<CustomerBasket?> GetBasketsAsync(string BasketId);
        Task<CustomerBasket?> UpdateBasketsAsync(CustomerBasket Basket);
        Task<bool> DeleteBasketsAsync(string BasketId);
    }
}

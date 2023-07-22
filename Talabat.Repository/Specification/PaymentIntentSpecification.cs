using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Specification
{
    public class PaymentIntentSpecification : BaseSpecification<Order>
    {
        public PaymentIntentSpecification(string paymentIntentId) : base(O => O.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}

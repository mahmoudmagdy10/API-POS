using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Specification
{
    public class OrdersOfUserSpecifications : BaseSpecification<Order>
    {

        public OrdersOfUserSpecifications(string email):base(O => O.BuyerEmail == email)
        {
            Includes.Add(N => N.DeliveryMethod);
            Includes.Add(N => N.Items);
            AddOrderByDesc(O => O.OrderDate);
        }
        
        public OrdersOfUserSpecifications(string email,int id) : base(O => O.BuyerEmail == email && O.Id == id)
        {
            Includes.Add(N => N.DeliveryMethod);
            Includes.Add(N => N.Items);
            AddOrderByDesc(O => O.OrderDate);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrder productItem, decimal cost, int quantity)
        {
            ProductItem = productItem;
            Cost = cost;
            Quantity = quantity;
        }

        public ProductItemOrder ProductItem { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
    }
}

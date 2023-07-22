namespace Talabat.Core.Entities.Order_Aggregate
{
    public class ProductItemOrder
    {
        public ProductItemOrder()
        {
            
        }
        public ProductItemOrder(int productId, string productName, string picUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PicUrl = picUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PicUrl { get; set; }
    }
}
namespace Talabat.API.DTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PicUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
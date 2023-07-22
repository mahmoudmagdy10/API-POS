using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }
        
        [Required]
        [Range(1,int.MaxValue)]
        public int Quantity { get; set; }
        
        [Required]
        [Range(0.1, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        public string PicUrl { get; set; }
        
        [Required]
        public string Brand { get; set; }
        
        [Required]
        public string Type { get; set; }
    }
}

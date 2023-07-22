using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class AddressDTO
    {
        [Required]
        public string FName { get; set; }
        
        [Required]
        public string LName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Street { get; set; }
    }
}

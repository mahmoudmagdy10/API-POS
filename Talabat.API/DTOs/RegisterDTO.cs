using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class RegisterDTO
    {
        [Required, MinLength(6), MaxLength(15)]
        public string DisplayName { get; set; }
        
        [Required,EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string UserName { get; set; }

        [Required,Phone]
        public string PhoneNumber { get; set; }
    }
}

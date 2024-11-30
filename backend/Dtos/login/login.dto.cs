using System.ComponentModel.DataAnnotations;

namespace backend.Dtos {
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
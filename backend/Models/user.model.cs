
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User 
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [MinLength(12)]
        [MaxLength(24)]
        public required string Password { get; set; }

    }
}
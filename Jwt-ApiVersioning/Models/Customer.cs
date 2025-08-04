using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jwt_ApiVersioning.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [MaxLength(11)]
        public string Mobile { get; set; }
        [Required]
        [MaxLength(90)]
        public string Pass { get; set; }
        [NotMapped]
        public string JwtSecret { get; set; }
        [Required]
        public Roles Role { get; set; }
    }
}

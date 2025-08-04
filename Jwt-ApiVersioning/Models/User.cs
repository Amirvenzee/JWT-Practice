using System.ComponentModel.DataAnnotations;

namespace Jwt_ApiVersioning.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(11)]
        public string Mobile { get; set; }
        [Required]
        public Roles Role  { get; set; }
    }

    public enum Roles
    {
        Admin,
        User

    }

}

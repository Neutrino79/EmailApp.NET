using System.ComponentModel.DataAnnotations;
using EmailApp.Models.IEntities;

namespace EmailApp.Models.Entites
{
    public class User : IUser
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsVerified { get; set; } = false;
    }
}

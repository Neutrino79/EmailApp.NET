using System.ComponentModel.DataAnnotations;

namespace EmailApp.Domain.Models.Entities
{
    public class Admin
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}

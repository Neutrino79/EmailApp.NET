using System.ComponentModel.DataAnnotations;
using EmailApp.Models.IEntities;

namespace EmailApp.Models.Entites
{
    public class Admin :IAdmin
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

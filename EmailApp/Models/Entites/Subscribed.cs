using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmailApp.Models.IEntities;

namespace EmailApp.Models.Entites
{ 
    public class Subscribed : ISubscribed
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}

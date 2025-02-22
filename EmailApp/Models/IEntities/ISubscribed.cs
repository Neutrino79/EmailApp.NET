
using EmailApp.Models.Entites;

namespace EmailApp.Models.IEntities
{ 
    public interface ISubscribed
    {
        int Id { get; set; }
        int UserId { get; set; }
        User User { get; set; }
    }
}

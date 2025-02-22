using EmailApp.Models.Entites;

namespace EmailApp.Data.Repositories.Interface
{
    public interface ISubscribedRepository
    {
        Task<IEnumerable<Subscribed>> GetAllSubscribedUserAsync();
        Task<Subscribed> GetSubscribedByEmailAsync(string email);
        Task<Subscribed> GetSubscribedByIdAsync(int id);
        Task<bool> AddSubscribedAsync(Subscribed subscriber);
        Task<bool> RemoveSubscribedByID(int id);
        Task<bool> RemoveSubscribedByEmailAsync(string Email);
    }
}

using EmailApp.Domain.Models.Entities;

namespace EmailApp.Domain.Interface
{
    public interface ISubscribedRepository
    {
        Task<IEnumerable<Subscribed>> GetAllSubscribedUserAsync();
        Task<Subscribed> GetSubscribedByEmailAsync(string email);
        Task<IEnumerable<User>> GetSubscribedByIdAsync(List<int> ids);
        Task<bool> AddSubscribedAsync(Subscribed subscriber);
        Task<bool> RemoveSubscribedByID(int id);
        Task<bool> RemoveSubscribedByEmailAsync(string Email);
    }
}

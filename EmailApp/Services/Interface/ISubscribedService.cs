using EmailApp.Models.Entites;

namespace EmailApp.Services.Interface
{
    public interface ISubscribedService
    {
        Task<IEnumerable<Subscribed>> GetAllSubscribedUsersAsync();
        Task<Subscribed?> GetSubscribedByEmailAsync(string email);
        Task<Subscribed?> GetSubscribedByIdAsync(int id);
        Task<bool> AddSubscribedAsync(Subscribed subscriber);
        Task<bool> RemoveSubscribedByIdAsync(int id);
        Task<bool> RemoveSubscribedByEmailAsync(string email);
    }
}

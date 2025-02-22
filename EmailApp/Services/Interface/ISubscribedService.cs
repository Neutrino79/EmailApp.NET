using EmailApp.Models.Entites;

namespace EmailApp.Services.Interface
{
    public interface ISubscribedService
    {
        Task<IEnumerable<Subscribed>> GetAllSubscribedUsersAsync();
        Task<Subscribed?> GetSubscribedByEmailAsync(string email);
        Task<IEnumerable<User>> GetSubscribedByIdAsync(List<int> ids);
        Task<bool> AddSubscribedAsync(Subscribed subscriber);
        Task<bool> RemoveSubscribedByIdAsync(int id);
        Task<bool> RemoveSubscribedByEmailAsync(string email);
        Task<bool> SendMailAsync(string selectedUserIds, string subject, string message);
        
    }
}

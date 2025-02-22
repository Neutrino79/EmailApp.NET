using EmailApp.Models.Entites;

namespace EmailApp.Data.Repositories.Interface
{
    public interface IAdmin
    {
        Task<IEnumerable<Admin>> GetAllAdminAsync();
        Task<User> GetAdminByIdAsync(int id);
        Task CreateAdminAsync(User user);
        Task UpdateAdminAsync(User user);
        Task DeleteAdminAsync(int id);
    }
}

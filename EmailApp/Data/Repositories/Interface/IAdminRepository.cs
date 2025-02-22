using EmailApp.Models.Entites;

namespace EmailApp.Data.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAdminAsync();
        Task<User> GetAdminByIdAsync(int id);
        Task<bool> CreateAdminAsync(Admin admin);
        Task<bool> UpdateAdminAsync(Admin admin);
        Task<bool> DeleteAdminAsync(int id);
    }
}

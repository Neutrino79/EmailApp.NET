using EmailApp.Models.Entites;

namespace EmailApp.Data.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAdminAsync();
        Task<Admin> GetAdminByIdAsync(int id);
        Task<Admin> GetAdminByEmailAsync(string email);

        Task<bool> CreateAdminAsync(Admin admin);
        Task<bool> UpdateAdminAsync(Admin admin);
        Task<bool> DeleteAdminAsync(int id);
    }
}

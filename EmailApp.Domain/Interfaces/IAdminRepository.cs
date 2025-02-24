using EmailApp.Domain.Models.Entities;

namespace EmailApp.Domain.Interface
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAdminAsync();
        Task<Admin> GetAdminByIdAsync(int id);
        Task<Admin> GetAdminByUsernameAsync(string username);
        Task<bool> CreateAdminAsync(Admin admin);
        Task<bool> UpdateAdminAsync(Admin admin);
        Task<bool> DeleteAdminAsync(int id);
    }
}

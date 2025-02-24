using EmailApp.Domain.Models.Entities;

namespace EmailApp.Contracts
{
    public interface IAdminService
    {
        Task<IEnumerable<Admin>> GetAllAdminAsync();
        Task<Admin> GetAdminByIdAsync(int id);
        Task<bool> CreateAdminAsync(Admin admin);
        Task<Admin> GetAdminByUsernameAsync(string username);
        Task<bool> UpdateAdminAsync(Admin admin);
        Task<bool> DeleteAdminAsync(int id);
    }
}

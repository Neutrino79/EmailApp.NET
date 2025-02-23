using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using EmailApp.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailApp.Services
{
    public class AdminService : IAdminService // ✅ Ensure it implements the interface
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminAsync()
        {
            return await _adminRepository.GetAllAdminAsync();
        }

        public async Task<Admin?> GetAdminByIdAsync(int id) // ✅ Changed return type to Admin?
        {
            return await _adminRepository.GetAdminByIdAsync(id);
        }

        public async Task<Admin?> GetAdminByEmailAsync(string email)
        {
            return await _adminRepository.GetAdminByEmailAsync(email);
        }

        public async Task<bool> CreateAdminAsync(Admin admin)
        {
            return await _adminRepository.CreateAdminAsync(admin);
        }

        public async Task<bool> UpdateAdminAsync(Admin admin) // ✅ Fixed method name
        {
            return await _adminRepository.UpdateAdminAsync(admin);
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            return await _adminRepository.DeleteAdminAsync(id);
        }
    }
}

using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;

namespace EmailApp.Services
{
    public class AdminService
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

        public async Task<User?> GetAdminByIdAsync(int id)
        {
            return await _adminRepository.GetAdminByIdAsync(id);
        }

        public async Task<bool> CreateAdminAsync(Admin admin)
        {
            return await _adminRepository.CreateAdminAsync(admin);
        }

        public async Task<bool> UpdateUserAsync(Admin admin)
        {
            return await _adminRepository.UpdateAdminAsync(admin);
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            return await _adminRepository.DeleteAdminAsync(id);
        }
    }
}
}

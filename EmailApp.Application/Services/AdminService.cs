using EmailApp.Domain.Models.Entities;
using EmailApp.Contracts;
using EmailApp.Domain.Interface;

namespace EmailApp.Application.Services
{
    public class AdminService : IAdminService
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

        public async Task<Admin?> GetAdminByIdAsync(int id)
        {
            return await _adminRepository.GetAdminByIdAsync(id);
        }

        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            return await _adminRepository.GetAdminByUsernameAsync(username);
        }


        public async Task<bool> CreateAdminAsync(Admin admin)
        {
            return await _adminRepository.CreateAdminAsync(admin);
        }

        public async Task<bool> UpdateAdminAsync(Admin admin)
        {
            return await _adminRepository.UpdateAdminAsync(admin);
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            return await _adminRepository.DeleteAdminAsync(id);
        }
    }
}

using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly ILogger<AdminRepository> _logger;

        public AdminRepository(ApplicationDbContext applicationDb, ILogger<AdminRepository> logger)
        {
            _applicationDb = applicationDb;
            _logger = logger;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminAsync()
        {
            return await _applicationDb.Admins.ToListAsync();
        }

        public async Task<Admin> GetAdminByIdAsync(int id)
        {
            return await _applicationDb.Admins.FindAsync(id);
        }

        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            return await _applicationDb.Admins.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<bool> CreateAdminAsync(Admin admin)
        {
            try
            {
                _applicationDb.Admins.Add(admin);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Admin was not added: {0}", ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateAdminAsync(Admin admin)
        {
            try
            {
                _applicationDb.Entry(admin).State = EntityState.Modified;
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update admin: {0}", ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            try
            {
                var admin = await _applicationDb.Admins.FindAsync(id);
                if (admin == null)
                    return false;

                _applicationDb.Admins.Remove(admin);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete admin: {0}", ex.Message);
                return false;
            }
        }
    }
}

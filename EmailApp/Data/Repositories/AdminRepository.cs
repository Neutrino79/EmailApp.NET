using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.Data.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly ILogger<UserRepository> _logger;

        public AdminRepository(ApplicationDbContext applicationDb, ILogger<UserRepository> logger)
        {
            _applicationDb = applicationDb;
            _logger = logger;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminAsync()
        {
            return await _applicationDb.Admins.ToListAsync();
        }

        public async Task<User> GetAdminByIdAsync(int id)
        {
            return await _applicationDb.Users.FindAsync(id);
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
                _logger.LogError("User was not added: {0}", ex.Message);
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
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            try
            {
                var admin = await _applicationDb.Admins.FindAsync(id);
                _applicationDb.Admins.Remove(admin);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

        }

    }
}

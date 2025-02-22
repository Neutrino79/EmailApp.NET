using EmailApp.Models.Entites;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.Data.Repositories
{
    public class AdminRepository
    {
        private readonly ApplicationDbContext _applicationDb;
        public AdminRepository(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }

        public async Task CreateAdminAsync(Admin admin)
        {
            try
            {
                _applicationDb.Admins.Add(admin);
                await _applicationDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("User was not added: " + ex.Message);
            }

        }

        public async Task DeleteAdminAsync(int id)
        {
            try
            {
                var admin = await _applicationDb.Admins.FindAsync(id);
                _applicationDb.Admins.Remove(admin);
                await _applicationDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<User>> GetAllAdminAsync()
        {
            return (IEnumerable<User>)await _applicationDb.Admins.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _applicationDb.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {

            _applicationDb.Entry(user).State = EntityState.Modified;
            await _applicationDb.SaveChangesAsync();
        }
    }
}

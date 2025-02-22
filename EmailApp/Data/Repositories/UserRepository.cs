using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
        {
            _applicationDb = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _applicationDb.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _applicationDb.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                await _applicationDb.Users.AddAsync(user);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await _applicationDb.Users.FindAsync(user.UserId);
                if (existingUser == null)
                    return false;

                _applicationDb.Entry(existingUser).CurrentValues.SetValues(user);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user with ID {user.UserId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _applicationDb.Users.FindAsync(id);
                if (user == null)
                    return false;

                _applicationDb.Users.Remove(user);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user with ID {id}: {ex.Message}");
                return false;
            }
        }
    }
}

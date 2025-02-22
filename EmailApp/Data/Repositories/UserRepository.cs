using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using Microsoft.EntityFrameworkCore;

namespace EmailApp.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDb;
        public UserRepository(ApplicationDbContext Context)
        {
            _applicationDb  =  Context;
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                _applicationDb.Users.Add(user);
                await _applicationDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("User was not added: " + ex.Message);
            }

        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var user = await _applicationDb.Users.FindAsync(id);
                _applicationDb.Users.Remove(user);
                await _applicationDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _applicationDb.Users.ToListAsync();
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

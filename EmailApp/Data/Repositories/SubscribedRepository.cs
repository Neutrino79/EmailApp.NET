using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using EmailApp.Models.IEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailApp.Data.Repositories
{
    public class SubscribedRepository : ISubscribedRepository
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly ILogger<SubscribedRepository> _logger;

        public SubscribedRepository(ApplicationDbContext context, ILogger<SubscribedRepository> logger)
        {
            _applicationDb = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Subscribed>> GetAllSubscribedUserAsync()
        {
            return await _applicationDb.Subscriptions.AsNoTracking().ToListAsync();
        }

        public async Task<Subscribed?> GetSubscribedByEmailAsync(string email)
        {
            try
            {
                return await _applicationDb.Subscriptions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.User.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving subscriber with email {Email}: {Message}", email, ex.Message);
                return null;
            }
        }

        public async Task<Subscribed?> GetSubscribedByIdAsync(int id)
        {
            try
            {
                return await _applicationDb.Subscriptions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving subscriber with ID {Id}: {Message}", id, ex.Message);
                return null;
            }
        }

        public async Task<bool> AddSubscribedAsync(Subscribed subscriber)
        {
            try
            {
                await _applicationDb.Subscriptions.AddAsync(subscriber);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding subscriber {Email}: {Message}", subscriber.User.Email, ex.Message);
                return false;
            }
        }

        public async Task<bool> RemoveSubscribedByID(int id)
        {
            try
            {
                var sub = await _applicationDb.Subscriptions.FindAsync(id);
                if (sub == null)
                {
                    _logger.LogWarning("No subscriber found with ID {Id}", id);
                    return false;
                }

                _applicationDb.Subscriptions.Remove(sub);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot remove subscriber with ID {Id}: {Message}", id, ex.Message);
                return false;
            }
        }

        public async Task<bool> RemoveSubscribedByEmailAsync(string email)
        {
            try
            {
                var user = await _applicationDb.Subscriptions.FirstOrDefaultAsync(x => x.User.Email == email);
                if (user == null)
                {
                    _logger.LogWarning("No subscriber found with email {Email}", email);
                    return false;
                }

                _applicationDb.Subscriptions.Remove(user);
                await _applicationDb.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Cannot remove subscriber with email {Email}: {Message}", email, ex.Message);
                return false;
            }
        }
    }
}

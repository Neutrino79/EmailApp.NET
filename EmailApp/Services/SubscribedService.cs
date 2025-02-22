using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using EmailApp.Services.Interface;
using Microsoft.Extensions.Logging;

namespace EmailApp.Services
{
    public class SubscribedService : ISubscribedService
    {
        private readonly ISubscribedRepository _subscribedRepository;
        private readonly ILogger<SubscribedService> _logger;

        public SubscribedService(ISubscribedRepository subscribedRepository, ILogger<SubscribedService> logger)
        {
            _subscribedRepository = subscribedRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Subscribed>> GetAllSubscribedUsersAsync()
        {
            return await _subscribedRepository.GetAllSubscribedUserAsync();
        }

        public async Task<Subscribed?> GetSubscribedByEmailAsync(string email)
        {
            return await _subscribedRepository.GetSubscribedByEmailAsync(email);
        }

        public async Task<Subscribed?> GetSubscribedByIdAsync(int id)
        {
            return await _subscribedRepository.GetSubscribedByIdAsync(id);
        }

        public async Task<bool> AddSubscribedAsync(Subscribed subscriber)
        {
            try
            {
                if (await _subscribedRepository.GetSubscribedByEmailAsync(subscriber.User.Email) != null)
                {
                    _logger.LogWarning("Subscription already exists for email: {Email}", subscriber.User.Email);
                    return false;
                }

                return await _subscribedRepository.AddSubscribedAsync(subscriber);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error adding subscription: {Message}", ex.Message);
                return false;
            }
        }

        public async Task<bool> RemoveSubscribedByIdAsync(int id)
        {
            return await _subscribedRepository.RemoveSubscribedByID(id);
        }

        public async Task<bool> RemoveSubscribedByEmailAsync(string email)
        {
            return await _subscribedRepository.RemoveSubscribedByEmailAsync(email);
        }
    }
}

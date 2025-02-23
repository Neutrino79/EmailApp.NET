using System.Net.Mail;
using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using EmailApp.Services.Interface;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;

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

        public async Task<IEnumerable<User>> GetSubscribedByIdAsync(List<int> ids)
        {
            return await _subscribedRepository.GetSubscribedByIdAsync(ids);
        }

        public async Task<bool> AddSubscribedAsync(Subscribed subscriber)
        {
            try
            {
                if (await _subscribedRepository.AddSubscribedAsync(subscriber))
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

        public async Task<bool> SendMailAsync(string selectedUserIds, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(selectedUserIds) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(message))
            {
                _logger.LogWarning("Invalid email request: Missing fields.");
                return false;
            }

            List<int> userIds;
            try
            {
                userIds = selectedUserIds.Split(',').Select(int.Parse).ToList();
            }
            catch (FormatException)
            {
                _logger.LogError("Invalid user IDs format.");
                return false;
            }

            var selectedUsers = await _subscribedRepository.GetSubscribedByIdAsync(userIds);
            var emailList = selectedUsers.Select(u => u.Email).ToList();

            if (!emailList.Any())
            {
                _logger.LogWarning("No valid users found for email sending.");
                return false;
            }

            // Parallel email sending
            await Parallel.ForEachAsync(emailList, async (email, cancellationToken) =>
            {
                await SendEmailToUserAsync(email, subject, message);
            });

            _logger.LogInformation("Emails sent successfully to {Count} users.", emailList.Count);
            return true;
        }

 
        private async Task SendEmailToUserAsync(string recipientEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Demo NewsLater", "no-reply@epam.in"));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = subject;
                message.Body = new TextPart("plain") { Text = body };

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("_smtpCredentials.Email", "_smtpCredentials.Password");//Need to add the email and pass for SMTP 
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email successfully sent to {Email}", recipientEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email to {Email}: {Message}", recipientEmail, ex.Message);
            }
        }

    }
}

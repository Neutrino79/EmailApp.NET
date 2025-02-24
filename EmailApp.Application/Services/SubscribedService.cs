using EmailApp.Domain.Models.Entities;
using EmailApp.Contracts;
using MailKit.Security;
using MimeKit;
using EmailApp.Domain.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailApp.Application.Services
{
    public class SubscribedService : ISubscribedService
    {
        private readonly ISubscribedRepository _subscribedRepository;
        private readonly ILogger<SubscribedService> _logger;
        private readonly SMTPCredentials _smtpCredentials;

        public SubscribedService(ISubscribedRepository subscribedRepository, ILogger<SubscribedService> logger, IOptions<SMTPCredentials> smtp)
        {
            _subscribedRepository = subscribedRepository;
            _logger = logger;
            _smtpCredentials = smtp.Value;
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

            var failedEmails = new List<string>();

            foreach (var email in emailList)
            {
                bool success = await SendEmailToUserAsync(email, subject, message);
                if (!success)
                {
                    failedEmails.Add(email);
                }
            }

            if (failedEmails.Any())
            {
                _logger.LogError("Failed to send emails to the following users: {Emails}", string.Join(", ", failedEmails));
                return false;
            }

            _logger.LogInformation("Emails sent successfully to {Count} users.", emailList.Count);
            return true;
        }

        private async Task<bool> SendEmailToUserAsync(string recipientEmail, string subject, string body)
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
                await client.AuthenticateAsync(_smtpCredentials.Email, _smtpCredentials.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email successfully sent to {Email}", recipientEmail);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email to {Email}: {Message}", recipientEmail, ex.Message);
                return false;
            }
        }

    }
}

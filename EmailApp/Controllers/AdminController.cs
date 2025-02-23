using EmailApp.Models.IEntities;
using EmailApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly ISubscribedService _subscribers;
        public AdminController(ISubscribedService subscribed)
        {
            _subscribers = subscribed;
        }

        [HttpGet("Home")]
        public async Task<IActionResult> Index()
        {
            var subscribers = await _subscribers.GetAllSubscribedUsersAsync();
            return View(subscribers);
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail(string selectedUserIds, string subject, string message)
        {
            bool isSuccess = await _subscribers.SendMailAsync(selectedUserIds, subject, message);

            if (!isSuccess)
            {
                TempData["ErrorMessage"] = "Failed to send email. Error Occured while sending the Emails";
            }
            else
            {
                TempData["SuccessMessage"] = "Email sent successfully!";
            }

            return RedirectToAction("Index");
        }


    }
}

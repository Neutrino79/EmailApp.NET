using System.Security.Claims;
using EmailApp.Contracts;
using EmailApp.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISubscribedService _subscribedService;

        public UserController(IUserService userService, ISubscribedService subscribedService)
        {
            _userService = userService;
            _subscribedService = subscribedService;
        }

        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue("UserId"); // Get logged-in user's ID

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSubscription()
        {
            var userId = User.FindFirstValue("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));

            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Toggle subscription status
            user.IsSubscribed = !user.IsSubscribed;
            await _userService.UpdateUserAsync(user);

            if (user.IsSubscribed)
            {
                var subscription = new Subscribed { UserId = user.UserId };
                await _subscribedService.AddSubscribedAsync(subscription);
            }
            else
            {
                await _subscribedService.RemoveSubscribedByIdAsync(user.UserId);
            }

            return RedirectToAction("Profile");
        }
    }
}

using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using EmailApp.Models;
using EmailApp.Models.Entites;
using EmailApp.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [Authorize] // Ensures only logged-in users can access this controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ISubscribedService _subscribedService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, ISubscribedService subscribedService)
        {
            _logger = logger;
            _userService = userService;
            _subscribedService = subscribedService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

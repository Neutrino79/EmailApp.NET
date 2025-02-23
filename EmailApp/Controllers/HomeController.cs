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

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
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

            return RedirectToAction("Profile");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using EmailApp.Models.Entites;
using EmailApp.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmailApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public AuthController(IUserService userService, IAdminService adminService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid form submission. Please correct the errors below.";
                return View(model);
            }

            var existingUser = await _userService.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                TempData["Error"] = "User registration failed. Email is already in use.";
                return View(model);
            }

            var isCreated = await _userService.CreateUserAsync(model);
            if (isCreated)
            {
                TempData["Success"] = "Account created successfully. You can now log in.";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "Registration failed due to a system error. Please try again later.";
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string emailOrUsername, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(emailOrUsername) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Both email/username and password are required.");
                return View();
            }

            if (role == "Admin")
            {
                var admin = await _adminService.GetAdminByUsernameAsync(emailOrUsername);
                if (admin == null || admin.Password != password)
                {
                    ModelState.AddModelError("", "Invalid admin credentials. Please try again.");
                    TempData["Error"] = "Admin login failed. Check your username and password.";
                    return View();
                }

                var adminClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var adminPrincipal = new ClaimsPrincipal(adminIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminPrincipal);
                return RedirectToAction("Home", "Admin");
            }
            else
            {
                var user = await _userService.GetUserByEmailAsync(emailOrUsername);
                if (user == null || user.Password != password)
                {
                    ModelState.AddModelError("", "Invalid email or password. Please try again.");
                    TempData["Error"] = "User login failed. Check your email and password.";
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserId", user.UserId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Profile", "User");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Success"] = "You have been logged out successfully.";
            return RedirectToAction("Login");
        }
    }
}

using EmailApp.Models.Entites;
using EmailApp.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmailApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

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
                return View(model);
            }

            var existingUser = await _userService.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "User with this email already exists.");
                return View(model);
            }

            // 🔹 Hash the password before saving
            model.Password = _passwordHasher.HashPassword(model, model.Password);

            await _userService.CreateUserAsync(model);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            // 🔹 Check if the user is "admin"
            if (email.ToLower() == "admin")
            {
                var admin = await _adminService.GetAdminByEmailAsync(email); // Change method name accordingly
                if (admin == null || admin.Password != password)  // Direct password comparison
                {
                    ModelState.AddModelError("", "Invalid admin credentials.");
                    return View();
                }

                // Create authentication claims for Admin
                var adminClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username), // Use Username instead of AdminId
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var adminIdentity = new ClaimsIdentity(adminClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var adminPrincipal = new ClaimsPrincipal(adminIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, adminPrincipal);

                return RedirectToAction("Index", "Admin");
            }

            // 🔹 Normal user authentication
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // Verify hashed password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // Create authentication claims for Normal Users
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("UserId", user.UserId.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Profile", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}

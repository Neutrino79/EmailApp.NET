using EmailApp.Models.Entites;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index(int Id, string Name, string Password)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Show validation errors
            }
            //var existingUser = 
            // Redirect to login or success page
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

    }
}

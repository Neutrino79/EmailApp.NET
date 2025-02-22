using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using EmailApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("All")]
        public async Task<IActionResult> Index()
        {
            var Users = await _userService.GetAllUsersAsync();
            return View(Users);
        }
    }
}

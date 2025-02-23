using EmailApp.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var Users = await _userService.GetAllUsersAsync();
            return View(Users);
        }
    }
}

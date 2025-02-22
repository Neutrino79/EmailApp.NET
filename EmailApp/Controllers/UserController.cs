using EmailApp.Data.Repositories.Interface;
using EmailApp.Models.Entites;
using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _UserRepository;

        public UserController(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }

        [Route("All")]
        public async Task<IActionResult> Index()
        {
            var Users = await _UserRepository.GetAllUsersAsync();
            return View(Users);
        }
    }
}

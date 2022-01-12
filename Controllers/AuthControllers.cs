using System.Linq;
using System.Threading.Tasks;
using Caffe.Models.ApiModels;
using Caffe.Models.MongoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Caffe.Controllers
{
    [Route("api/Account")]
    public class AuthControllers : Controller
    {
        private readonly UserManager<MongoUser> _userManager;
        private readonly SignInManager<MongoUser> _signInManager;
        private readonly RoleManager<MongoRole> _roleManager;

        public AuthControllers(UserManager<MongoUser> userManager, SignInManager<MongoUser> signInManager,
            RoleManager<MongoRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Email)
                || string.IsNullOrEmpty(user.Password)
                || string.IsNullOrEmpty(user.PhoneNumber))
                return BadRequest("Необходимо заполнить все поля");
            var appUser = new MongoUser()
            {
                UserName = user.Email,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            var res = await _userManager.CreateAsync(appUser, user.Password);
            if (res.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(appUser, user.Password, false, false);
                return Ok("Регистрация успешна");
            }

            return BadRequest(res.Errors.First().Description);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser != null)
            {
                var result = await _signInManager.PasswordSignInAsync(appUser, user.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok("Успех");
                }
            }

            return BadRequest("Неверный логин или пароль");
        }

        [HttpGet]
        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}